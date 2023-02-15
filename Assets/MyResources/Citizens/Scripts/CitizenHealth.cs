using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CitizenHealth : MonoBehaviour
{    
    [SerializeField] float healthPoints;
    Animator an;
    public NavMeshAgent agent;
    CitizenBehaviour CB;
    RagdollActivator ragdollActivator;
    
    void Start(){
        an = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ragdollActivator = GetComponent<RagdollActivator>();
        CB = GetComponent<CitizenBehaviour>();
    }

    public void takeDamage(float amount){
        if(healthPoints>=0){
        CB.FearStateApply();
        healthPoints-=amount;
        if(healthPoints<=25&&healthPoints>0){
           an.SetTrigger("Pray");
           CB.ActivetDesAgent();
        }else if(healthPoints<=0){
           ragdollActivator.RagdollModeOn();
           CB.changeState(4);
        }
        }
    }
}
