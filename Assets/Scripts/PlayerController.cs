using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_rb;
    [SerializeField]
    float speed;
    float xAxis, yAxis;
    Vector3 playerMovementVector;
    Vector2 mouseDelta;
    [SerializeField]
    float cameraSpeed;
    Vector2 cameraRotation;
    HingeJoint m_cameraJoint;
    JointSpring m_springJoint;
    float CameraSpring = 2000.0f;
    float CameraSpringDampen = 10.0f;
    float currentAnglePos = 0;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();

        setCameraJoints();
    }

    // Update is called once per frame
    void Update()
    {
        
        GetPlayerCameraInput();
        GetPlayerMovementInput();
        Move();
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Cursor.lockState == CursorLockMode.None) 
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
        }
    }

    private void FixedUpdate()
    {
        setPlayerCamera();
        //checks players hinge joint to see if it is out of bounds
        checkPlayerCameraJoint();
    }

    void setCameraJoints() 
    {
        m_cameraJoint = GetComponent<HingeJoint>();
        m_springJoint = new JointSpring();
        m_cameraJoint.useSpring = true;
        m_springJoint.spring = CameraSpring;
        m_springJoint.damper = CameraSpringDampen;
        m_cameraJoint.spring = m_springJoint;
    }
    void GetPlayerCameraInput() 
    {
        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraRotation += mouseDelta * cameraSpeed * Time.deltaTime;
    }

    void setPlayerCamera() 
    {
        m_rb.GetComponent<Transform>().Rotate(0.0f, cameraRotation.x, 0.0f);

        m_springJoint.targetPosition = currentAnglePos - cameraRotation.y;
        Debug.Log(m_springJoint.targetPosition);
        currentAnglePos -= cameraRotation.y;
        m_cameraJoint.spring = m_springJoint;
        cameraRotation = Vector2.zero;
    }

    void checkPlayerCameraJoint() 
    {
        if (currentAnglePos > m_cameraJoint.limits.max)
        {
            currentAnglePos = m_cameraJoint.limits.max;
        }
        else if (currentAnglePos < m_cameraJoint.limits.min)
        {
            currentAnglePos = m_cameraJoint.limits.min;
        }
    }
    void GetPlayerMovementInput() 
    {
        xAxis = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        yAxis = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        playerMovementVector = transform.position - (transform.forward * xAxis) + (transform.right * yAxis);
    }

    void Move() 
    {
        transform.position = playerMovementVector;
    }
}
