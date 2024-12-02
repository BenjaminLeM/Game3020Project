using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayerGravity : MonoBehaviour
{
    [SerializeField]
    public Vector3 customGravity;
    Rigidbody m_rb;
    bool m_enabled = false;
    private void Awake()
    {
        m_rb = GetComponentInChildren<Rigidbody>();
    }

    public void EnableCustomGravity() 
    {
        m_enabled = true;
        m_rb.useGravity = false;
    }

    public void DisableCustomGravity() 
    {
        m_enabled = false;
        m_rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (m_enabled)
        {
            m_rb.AddForce(customGravity, ForceMode.Force);
        }
    }
}
