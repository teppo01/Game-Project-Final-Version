using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagodollControl : MonoBehaviour
{
    public Rigidbody[] rigidBodies;
    public Animator animator;
    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        deactiveRagdoll();
    }

    public void deactiveRagdoll()
    {
        foreach(var rigid in rigidBodies)
        {
            rigid.isKinematic = true;
        }
    }

    public void activeRagdoll()
    {
        foreach(var rigid in rigidBodies)
        {
            rigid.isKinematic = false;
        }
        animator.enabled = false;
    }
}
