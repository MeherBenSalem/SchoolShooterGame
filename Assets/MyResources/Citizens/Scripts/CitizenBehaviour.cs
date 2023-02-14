using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CitizenBehaviour : MonoBehaviour
{
    private enum StateAI{
        idle,petrol,talk,fear,dead
    }

    [Range(0,60)]
    [SerializeField] float _idleTimer,_talkTimer,checkRange;

    private Animator An;

    private NavMeshAgent agent;

    [SerializeField] LayerMask whatIsGround;

    private Transform lookTarget=null;

    [SerializeField] StateAI state;

    public Vector3 walkPoint;
    bool walkPointSet,isWaiting;
    public float walkPointRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        An = GetComponent<Animator>();
        isWaiting=false;
        An.SetBool("Basic",true);
    }

    private void Update()
    {
        if(state == StateAI.idle)
            IdleState();
        if(state == StateAI.petrol)
            PatrolingState();
        if(state==StateAI.talk)
            TalkState();
        if(state == StateAI.fear)
            FearState();
        if(state == StateAI.dead)
            deadState();
    }

    private void deadState(){
        Collider[] Hit =  Physics.OverlapSphere(transform.position,checkRange*2);
        foreach(Collider t in Hit){
            if(t.transform.GetComponentInParent<CitizenBehaviour>()){
                t.transform.GetComponentInParent<CitizenBehaviour>().FearStateApply();
            }
        }
        Destroy(this);
    }

    private void PatrolingState()
    {
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet){
            agent.SetDestination(walkPoint);
            An.SetBool("Walking",true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f){
            ResetAnimations();
            agent.isStopped=true;
            walkPointSet = false;
            isWaiting=false;
            changeState(0);
            StopAllCoroutines();
        }
    }

    private void IdleState(){
        if(!isWaiting){
            CitizenBehaviour alive = CheckAround();
            if(alive!=null){
                alive.StartTalk(this.transform);
                StartTalk(alive.transform);
            }else {
                RandomState();
                StartCoroutine(waiter(_idleTimer));
                isWaiting=true;
            }
        }
    }

    private void FearState(){
        if(!walkPointSet) SearchWalkPoint();
        if(walkPointSet && agent.isActiveAndEnabled){
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }

    public void FearStateApply(){
        if(state == StateAI.fear)
            return;
        ResetAnimations();
        StopAllCoroutines();
        An.SetTrigger("Panic");
        this.walkPointRange*=2;
        if(!An.GetBool("Injured")){
            changeAgentSpeed(1.5f);
        }
        walkPointSet = false;
        changeState(3);
    }

    private void TalkState(){      
        if(lookTarget!=null){
            transform.LookAt(lookTarget);
        }
        if(!isWaiting){
            StartCoroutine(waiter(_talkTimer));
            isWaiting=true;
        }
    }

    public void StartTalk(Transform lookAt){
        agent.SetDestination(this.transform.position);
        agent.isStopped = true;
        StopAllCoroutines();
        ResetAnimations();
        An.SetBool("Talking",true);
        changeState(2);
        isWaiting=false;
        lookTarget=lookAt;
    }

    private void SearchWalkPoint(){
        RandomState();
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }
    }

    public void changeAgentSpeed(float speed){
        agent.speed*=speed;
    }

    IEnumerator waiter(float timer){
        if(state==StateAI.fear)
           yield return null;
        yield return new WaitForSeconds(timer);
        ResetAnimations();
        changeState(1);
        agent.isStopped = false;
    }

    CitizenBehaviour CheckAround(){
        Collider[] Hit =  Physics.OverlapSphere(transform.position,checkRange);
        foreach(Collider t in Hit){
            if(t.transform.GetComponentInParent<CitizenBehaviour>()&&t.transform.root!=this.transform){
                return t.transform.GetComponentInParent<CitizenBehaviour>();
            }
        }
        return null;
    }

    void ResetAnimations(){
        if(agent==null)
        return;
        lookTarget=null;
        agent.enabled=true;
        An.SetInteger("State",0);
        An.SetBool("Walking",false);
        An.SetBool("Talking",false);
    }
    void RandomState(){
        An.SetInteger("State",Random.Range(0,4));
    }

    public void ActivetDesAgent(){
        if(agent.isActiveAndEnabled){
            agent.enabled = false;
        }
        else 
            agent.enabled = true;
    }
    public void changeState(int state){
        switch(state){
            case 0: 
            this.state = StateAI.idle;
                break;
            case 1:
            this.state = StateAI.petrol;
                break;
            case 2:
            this.state = StateAI.talk;
                break;
            case 3:
                if(agent!=null){
                agent.enabled = false;
                agent.enabled = true;
                }
                this.state = StateAI.fear;
                break;
            case 4:
            Destroy(agent);
            this.state = StateAI.dead;
                break;
        }
    }
}
