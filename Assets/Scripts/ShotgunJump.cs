using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunJump : MonoBehaviour
{
    [SerializeField]
    Transform LookDirection;
    [SerializeField]
    float LaunchForce = 5;
    bool onCoolDown = false;
    Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    public void Launch() 
    {
        if (!onCoolDown)
        {
            m_rb.AddForce(-LookDirection.forward * LaunchForce, ForceMode.Impulse);
            onCoolDown = true;
        }
    }

    public void Reload() 
    {
        onCoolDown = false;
    }
}
