using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    BasePickUpBehaviour pickupBehaviour;

    private void Update()
    {
        pickupBehaviour.PickUpFloatingMovement();
    }
    public bool SetPickUpBehaviour(BasePickUpBehaviour Behaviour) 
    {
        pickupBehaviour = Behaviour;
        if (pickupBehaviour != Behaviour)
        {
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupBehaviour.PickUpAction();
        }
    }
}
