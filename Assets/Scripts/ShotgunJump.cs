using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunJump : MonoBehaviour
{
    float LaunchForce = 30;
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
            m_rb.AddForce(-FindObjectOfType<Camera>().transform.forward * LaunchForce, ForceMode.Impulse);
            onCoolDown = true;
        }
    }

    public void Reload() 
    {
        onCoolDown = false;
    }
}
