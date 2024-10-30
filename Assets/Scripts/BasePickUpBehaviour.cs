using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePickUpBehaviour : MonoBehaviour
{
    protected Transform pickupVisual;
    private void Awake()
    {
        if(transform.childCount > 0)
            pickupVisual = transform.GetChild(0);
    }
    public void PickUpFloatingMovement() 
    {
        if (pickupVisual)
        {
            pickupVisual.transform.Rotate(0, 15 * Time.deltaTime, 0);
            pickupVisual.transform.position = pickupVisual.transform.position 
                + new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup) * 0.5f * transform.localScale.y * Time.deltaTime, 0);
        }
    }
    public virtual void PickUpAction() 
    {
        
    }
}
