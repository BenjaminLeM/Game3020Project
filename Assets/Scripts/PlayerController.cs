using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Rigidbody oth_rb;
    Rigidbody m_rb;
    PlayerCharacter character;
    CustomPlayerGravity gravity;
    float xAxis, yAxis;
    float sprintTimer = 0.0f;
    [SerializeField]
    LayerMask grounddetection;
    [SerializeField]
    float groundedCooldown = 0.1f;
    float groundedTime = 0.0f;
    bool groundedTimerActive = false;
    bool isGrounded = false;
    float timeSinceLastGrounded = 0.0f;
    float coyoteJump = 0.5f;

    //if player is wallRiding used to detect if they can wall jump
    bool isWallRiding = false;
    //used to detect the last side of the character touched a wall
    bool LeftRightWall = false;

    Vector3 WallJumpForce = Vector3.zero;
    bool wallRidingTimerActive = false;
    float wallRidingCooldown = 1.0f;
    float wallRidingTime = 0.0f;
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
    [SerializeField]
    Camera fpsCamera;

    GameManager m_gameManager;

    float cameraIncresedFOVAmount = 0;

    ShotgunJump gunJump;

    AudioClip slidingSound;

    bool isSprinting = false;
    bool isCrouching = false;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Debug.Log(m_rb.name);
        oth_rb = transform.GetChild(0).GetComponent<Rigidbody>();
        Debug.Log(oth_rb.name);
        
        character = GetComponent<PlayerCharacter>();

        m_gameManager = FindAnyObjectByType<GameManager>();

        gravity = GetComponent<CustomPlayerGravity>();

        slidingSound = Resources.Load<AudioClip>("SFX/SlidingNoise");

        character.setPlayerMoveSpeed(character.WalkSpeed());

        setCameraJoints();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        GetPlayerCameraInput();
        SetPlayerSpeed();
        Move();
    }

    private void FixedUpdate()
    {
        setPlayerCamera();
        //checks players hinge joint to see if it is out of bounds
        checkPlayerCameraJoint();

        //sets player height
        character.setCurrentHeight(character.getCrouchHeight());

        checkGrounded();

        //check for if the player can wall ride
        if (!isGrounded) 
        {
            checkIsWallRiding();
        }

        if (wallRidingTimerActive) 
        {
            wallRidingTimer();
        }
    }

    //player camera functions
    #region
    void setCameraJoints() 
    {
        m_cameraJoint = GetComponentInChildren<HingeJoint>();
        m_springJoint = new JointSpring();
        m_cameraJoint.useSpring = true;
        m_springJoint.spring = CameraSpring;
        m_springJoint.damper = CameraSpringDampen;
        m_cameraJoint.spring = m_springJoint;
    }
    void GetPlayerCameraInput() 
    {
        cameraRotation += mouseDelta * cameraSpeed * Time.deltaTime;
    }

    void setPlayerCamera() 
    {
        transform.Rotate(0.0f, cameraRotation.x, 0.0f);

        m_springJoint.targetPosition = currentAnglePos - cameraRotation.y;
        currentAnglePos += cameraRotation.y;
        m_cameraJoint.spring = m_springJoint;
        cameraRotation = Vector2.zero;
    }

    void cameraIncreaseFOV(float amount) 
    {
        fpsCamera.fieldOfView += amount;
        cameraIncresedFOVAmount += amount;
    }
    void resetCameraFOV() 
    {
        fpsCamera.fieldOfView -= cameraIncresedFOVAmount;
        cameraIncresedFOVAmount = 0;
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
    void SetPlayerSpeed() 
    {
        if (isSprinting)
        {
            if (sprintTimer == 0 && Input.GetAxisRaw("Crouch") == 0)
            {
                cameraIncreaseFOV(10.0f);
            }
            if (isCrouching && sprintTimer >= character.SlideCooldown())
            {
                sprintTimer = 0;
                //player can only slide if the player is considered grounded
                if (isGrounded)
                {
                    m_rb.AddForce(transform.forward * character.SlideSpeed(), ForceMode.Impulse);
                    
                    GetComponent<AudioSource>().PlayOneShot(slidingSound, 0.5f);
                }
                character.setPlayerMoveSpeed(character.CrouchSpeed());
                character.setCurrentHeight(character.getCrouchHeight());
            }
            else if (isCrouching) 
            {
                character.setPlayerMoveSpeed(character.CrouchSpeed());
                character.setCurrentHeight(character.getCrouchHeight());
            }
            else
            {
                sprintTimer += Time.deltaTime;
                character.setPlayerMoveSpeed(character.SprintSpeed());
                character.setCurrentHeight(1);
            }
        }
        else if (isCrouching) 
        {
            sprintTimer = 0;
            character.setPlayerMoveSpeed(character.CrouchSpeed());
            character.setCurrentHeight(character.getCrouchHeight());
            resetCameraFOV();
        }
        else
        {
            sprintTimer = 0;
            character.setPlayerMoveSpeed(character.WalkSpeed());
            character.setCurrentHeight(1);
            resetCameraFOV();
        }
    }
    public void WASDMovement(InputAction.CallbackContext callback) 
    {
        xAxis = callback.ReadValue<Vector2>().x;
        yAxis = callback.ReadValue<Vector2>().y;
    }

    public void Jump(InputAction.CallbackContext callback) 
    {
        if (callback.performed) 
        {
            float calculatedJumpForce = Mathf.Sqrt(character.JumpHeight() * -2 * (Physics.gravity.y));
            if (isGrounded)
            {
                m_rb.AddForce(calculatedJumpForce * transform.up, ForceMode.Impulse);
                setGrounded(false);
            }
            else if (isWallRiding)
            {
                m_rb.AddForce(calculatedJumpForce * transform.up * 3, ForceMode.Impulse);
                m_rb.AddForce(WallJumpForce, ForceMode.Impulse);
                wallRidingTimerActive = true;
            }
        }
    }

    public void CameraControl(InputAction.CallbackContext callback) 
    {
        mouseDelta = callback.ReadValue<Vector2>();
    }

    public void Sprint(InputAction.CallbackContext callback) 
    {
        if (callback.performed)
        {
            isSprinting = true;
        }
        else 
        {
            isSprinting = false;
        }
    }

    public void Crouch(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }

    public void Shoot(InputAction.CallbackContext callback) 
    {
        if(gunJump != null && callback.performed)
            gunJump.Launch();
    }
    public void Reload(InputAction.CallbackContext callback)
    {
        if (gunJump != null && callback.performed)
            gunJump.Reload();
    }

    public void Pause(InputAction.CallbackContext callback) 
    {
        if(callback.performed)
            m_gameManager.pauseGame();
    }

    void checkIsWallRiding() 
    {
        RaycastHit hit;

        //checks for rightside of the player for collisions
        if (Physics.Linecast(transform.position, transform.position + (transform.right * (0.25f + transform.localScale.x)),
                                                                out hit, -1,
                                                                QueryTriggerInteraction.Ignore)
                && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) 
                && !wallRidingTimerActive)
        {
            WallJumpForce = (transform.forward * (character.JumpHeight()))
                - (transform.right * (character.JumpHeight() / 2));
            if (!isWallRiding && hit.transform != transform)
            {
                oth_rb.transform.Rotate(Vector3.forward * 15);
                LeftRightWall = true;
                isWallRiding = true;
                //sets gravity to a custom amount, ie low gravity
                gravity.EnableCustomGravity();
            }

        }
        //checks for the leftside of the player for collisions
        else if (Physics.Linecast(transform.position, transform.position - (transform.right * (0.25f + transform.localScale.x)),
                                                            out hit, -1,
                                                            QueryTriggerInteraction.Ignore)
            && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            && !wallRidingTimerActive)
        {
            WallJumpForce = (transform.forward * (character.JumpHeight()))
                + (transform.right * (character.JumpHeight() / 2));
            if (!isWallRiding && hit.transform != transform)
            {
                oth_rb.transform.Rotate(Vector3.forward * -15);
                LeftRightWall = false;
                isWallRiding = true;
                //sets gravity to a custom amount, ie low gravity
                gravity.EnableCustomGravity();
            }
        }
        else
        {
            if (isWallRiding)
            {
                oth_rb.transform.rotation = m_rb.transform.rotation;
                isWallRiding = false;
            }
            WallJumpForce = Vector3.zero;
            gravity.DisableCustomGravity();
        }
    }
    void wallRidingTimer() 
    {
        wallRidingTime += Time.deltaTime;
        if (wallRidingTime >= wallRidingCooldown)
        {
            wallRidingTimerActive = false;
            wallRidingTime = 0;
        }
    }
    void checkGrounded() 
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, transform.position - transform.up * 0.75f,
                                                            out hit, grounddetection,
                                                            QueryTriggerInteraction.Ignore))
        {
            if (!isGrounded)
            {
                groundedTime += Time.deltaTime;
                //Debug.Log(groundedTime);
                if (groundedTime >= groundedCooldown)
                {
                    setGrounded(true);
                    groundedTime = 0;
                }
            }
        }
        else if (timeSinceLastGrounded > coyoteJump)
        {
            setGrounded(false);
            timeSinceLastGrounded = 0;
        }
        else 
        {
            timeSinceLastGrounded += Time.deltaTime;
        }
    }
    void setGrounded(bool state) 
    {
        isGrounded = state;
    }
    void Move() 
    {
        playerMovementVector = ((transform.right * xAxis) + (transform.forward * yAxis)) * character.getPlayerMoveSpeed() * Time.deltaTime;
        transform.position += playerMovementVector;
    }

    public void setGunComp(ShotgunJump gun) 
    {
        gunJump = gun;
    }
}

