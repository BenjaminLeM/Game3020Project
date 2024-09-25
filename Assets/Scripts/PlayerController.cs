using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_rb;
    float speed;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float jumpHeight;
    float xAxis, yAxis;
    [SerializeField]
    float groundedCooldown = 0.1f;
    float groundedTime = 0.0f;
    bool groundedTimerActive = false;
    bool isGrounded = false;
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
    bool isSprinting = false;
    [SerializeField]
    float sprintSpeed;
    float sprintTimer = 0.0f;
    [SerializeField]
    float crouchSpeed;
    [SerializeField]
    float crouchHeight;
    float currentHeight;
    [SerializeField]
    float slideCooldown;
    [SerializeField]
    float slideSpeed;
    [SerializeField]
    Canvas PauseMenu;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();

        speed = walkSpeed;

        setCameraJoints();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        GetPlayerCameraInput();
        GetPlayerMovementInput();
        Move();
        
    }

    private void FixedUpdate()
    {
        setPlayerCamera();
        //checks players hinge joint to see if it is out of bounds
        checkPlayerCameraJoint();

        //sets player height
        setPlayerScale(new Vector3(transform.localScale.x,
                                            currentHeight,
                                            transform.localScale.z));

        if (groundedTimerActive && !isGrounded)
        {
            groundedTimer();
        }
        else if(isGrounded)
        {
            groundedTimerActive = false;
        }
        checkGrounded();
    }

    //player camera functions
    #region
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
    #endregion
    void GetPlayerMovementInput() 
    {
        if (Input.GetAxisRaw("Sprint") > 0)
        {
            if (Input.GetAxisRaw("Crouch") > 0 && sprintTimer >= slideCooldown)
            {
                sprintTimer = 0;
                m_rb.AddForce(transform.right * slideSpeed, ForceMode.Impulse);
                speed = crouchSpeed;
                currentHeight = crouchHeight;
            }
            else if (Input.GetAxisRaw("Crouch") > 0) 
            {
                speed = crouchSpeed;
                currentHeight = crouchHeight;
            }
            else
            {
                sprintTimer += Time.deltaTime;
                speed = sprintSpeed;
                currentHeight = 1;
            }
        }
        else if (Input.GetAxisRaw("Crouch") > 0) 
        {
            speed = crouchSpeed;
            currentHeight = crouchHeight;
        }
        else
        {
            speed = walkSpeed;
            currentHeight = 1;
        }
        xAxis = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        yAxis = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;


        playerMovementVector = transform.position - (transform.forward * xAxis) + (transform.right * yAxis);
        if (isGrounded && Input.GetAxisRaw("Jump") > 0)
        {
            m_rb.AddForce(jumpHeight * transform.up);
        }

        if (Input.GetAxisRaw("Pause") > 0) 
        {
            PauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void checkGrounded() 
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, new Vector3(transform.position.x,
                                                            transform.position.y - (transform.localScale.y + 0.25f),
                                                            transform.position.z),
                                                            out hit, -1,
                                                            QueryTriggerInteraction.Ignore))
        {
            if (hit.collider)
            {
                groundedTimerActive = true;
            }
        }
        else
        {
            setGrounded(false);
        }
    }
    void setGrounded(bool state) 
    {
        isGrounded = state;
    }
    void groundedTimer() 
    {
        groundedTime += Time.deltaTime;
        if (groundedTime >= groundedCooldown)
            {
                setGrounded(true);
                groundedTime = 0;
            }
    }

    void setPlayerScale(Vector3 playerScale) 
    {
        if (transform.localScale == playerScale)
        {
            return;
        }
        else 
        {
            transform.localScale = playerScale;
        }
    }
    void Move() 
    {
        transform.position = playerMovementVector;
    }
}

