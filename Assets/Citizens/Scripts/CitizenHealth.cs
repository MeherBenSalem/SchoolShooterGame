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
        CB.FearStateApply();
        healthPoints-=amount;
        if(healthPoints<=25){
           an.SetTrigger("Pray");
           CB.ActivetDesAgent();
        }
        if(healthPoints<=0){
           ragdollActivator.RagdollModeOn();
           CB.changeState(4);
        }
    }
}
