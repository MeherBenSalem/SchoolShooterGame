using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollActivator : MonoBehaviour
{
    [SerializeField] private Collider[] mainColliders;
    [SerializeField] private GameObject thisGuysRig;
    [SerializeField] private Animator An;
    [SerializeField] private NavMeshAgent agent;
 
    void Start(){
        agent = GetComponent<NavMeshAgent>();
        getRagdollBits();
        RagdollModeOff();
    }
    public void RagdollModeOn(){
        An.enabled = false;
        agent.enabled = false;
        foreach(Collider col in ragdollColliders){
           col.enabled = true;
        }
        foreach(Rigidbody col in ragdollRigisbodys){
           col.isKinematic = false;
        }
        foreach(Collider col in mainColliders){
           col.enabled = false;
        }
    }
    public void RagdollModeOff(){
        An.enabled = true;
        agent.enabled = true;
        foreach(Collider col in ragdollColliders){
           col.enabled = false;
        }
        foreach(Rigidbody col in ragdollRigisbodys){
           col.isKinematic = true;
        }
        foreach(Collider col in mainColliders){
           col.enabled = true;
        }
    }

    Collider[] ragdollColliders;
    Rigidbody[] ragdollRigisbodys;
    void getRagdollBits(){
        ragdollColliders = thisGuysRig.GetComponentsInChildren<Collider>();
        ragdollRigisbodys = thisGuysRig.GetComponentsInChildren<Rigidbody>();
    }
}
