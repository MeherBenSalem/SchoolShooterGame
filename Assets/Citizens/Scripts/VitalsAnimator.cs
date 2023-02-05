using UnityEngine;

public class VitalsAnimator : MonoBehaviour
{
    [SerializeField] private Animator An;
    [SerializeField] CitizenHealth Ch;
    [SerializeField] Transform parent;
    
    [Header("Points")]
    [SerializeField] int VitalsId;
    [Range(0f,5f)]
    [SerializeField] float multipler;
    [SerializeField] float agentSpeed;
    [SerializeField] bool isMirror;

    void Start() {
        this.gameObject.transform.parent = parent;
    }
    public void Damage(int damage){
        An.SetBool("Injured",true);
        An.SetBool("Basic",false);
        An.SetInteger("VitalHit",VitalsId);
        An.SetBool("Mirror",isMirror);
        Ch.takeDamage(damage*multipler);
        if(agentSpeed!=0)
            Ch.agent.speed = agentSpeed;
    }
}
