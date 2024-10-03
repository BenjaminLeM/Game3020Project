using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutOfBounds : MonoBehaviour
{
    [SerializeField]
    Transform ResetPosition;
    [SerializeField]
    float VerticalBound = -2;
    private void FixedUpdate()
    {
        if (transform.position.y < VerticalBound) 
        {
            transform.position = ResetPosition.position;
        }
    }
}
