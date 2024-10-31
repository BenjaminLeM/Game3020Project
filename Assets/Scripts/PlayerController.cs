using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_rb;
    PlayerCharacter character;
    CustomPlayerGravity gravity;
    float xAxis, yAxis;
    float sprintTimer = 0.0f;
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
    [SerializeField]
    Camera ThirdPersonCamera;

    ShotgunJump gunJump;

    [SerializeField]
    Canvas PauseMenu;

    AudioClip slidingSound;
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();

        character = GetComponent<PlayerCharacter>();

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
        GetPlayerMovementInput();
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
        m_cameraJoint = GetComponent<HingeJoint>();
        m_springJoint = new JointSpring();
        m_cameraJoint.useSpring = true;
        m_springJoint.spring = CameraSpring;
        m_springJoint.damper = CameraSpringDampen;
        m_cameraJoint.spring = m_springJoint;
    }
    void GetPlayerCameraInput() 
    {
        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
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
            if (Input.GetAxisRaw("Crouch") > 0 && sprintTimer >= character.SlideCooldown())
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
            else if (Input.GetAxisRaw("Crouch") > 0) 
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
        else if (Input.GetAxisRaw("Crouch") > 0) 
        {
            character.setPlayerMoveSpeed(character.CrouchSpeed());
            character.setCurrentHeight(character.getCrouchHeight());
        }
        else
        {
            character.setPlayerMoveSpeed(character.WalkSpeed());
            character.setCurrentHeight(1);
        }
        xAxis = Input.GetAxisRaw("Horizontal") * character.getPlayerMoveSpeed() * Time.deltaTime;
        yAxis = Input.GetAxisRaw("Vertical") * character.getPlayerMoveSpeed() * Time.deltaTime;

        
        playerMovementVector = transform.position + (transform.right * xAxis) + (transform.forward * yAxis);

        float calculatedJumpForce = Mathf.Sqrt(character.JumpHeight() * -2 * (Physics.gravity.y));
        if (isGrounded && Input.GetAxisRaw("Jump") > 0)
        {
            m_rb.AddForce(calculatedJumpForce * transform.up, ForceMode.Impulse);
            setGrounded(false);
        }
        //checks for wallriding
        else if (isWallRiding && Input.GetAxisRaw("Jump") > 0 && !wallRidingTimerActive) 
        {
            m_rb.AddForce(calculatedJumpForce * transform.up * 3, ForceMode.Impulse);
            m_rb.AddForce(WallJumpForce, ForceMode.Impulse);
            Debug.Log(wallRidingTime);
            wallRidingTimerActive = true;
        }

        if (Input.GetAxisRaw("Pause") > 0) 
        {
            PauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetAxisRaw("CameraSwitch") > 0)
        {
            
                fpsCamera.enabled = false;
                ThirdPersonCamera.enabled = true;
            
        }
        else if (Input.GetAxisRaw("CameraSwitch") < 0) 
        {
            fpsCamera.enabled = true;
            ThirdPersonCamera.enabled = false;
        }

        if (gunJump != null)
        {
            if (Input.GetAxisRaw("Fire1") > 0)
            {
                gunJump.Launch();
            }

            if (Input.GetAxisRaw("Fire2") > 0)
            {
                gunJump.Reload();
            }
        }
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
                transform.Rotate(Vector3.forward * 15);
                LeftRightWall = true;
                isWallRiding = true;
                //sets gravity to a custom amount, ie low gravity
                gravity.EnableCustomGravity();
                Debug.Log("right");
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
                transform.Rotate(Vector3.forward * -15);
                LeftRightWall = false;
                isWallRiding = true;
                //sets gravity to a custom amount, ie low gravity
                gravity.EnableCustomGravity();
                Debug.Log("left");
            }
        }
        else
        {
            if (isWallRiding)
            {
                if (LeftRightWall)
                {
                    transform.Rotate(Vector3.forward * -15);
                }
                else
                {
                    transform.Rotate(Vector3.forward * 15);
                }
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
        else if (timeSinceLastGrounded > coyoteJump)
        {
            setGrounded(false);
            timeSinceLastGrounded = 0;
        }
        else 
        {
            timeSinceLastGrounded += Time.deltaTime;
        }

        if (groundedTimerActive && !isGrounded)
        {
            groundedTime += Time.deltaTime;
            if (groundedTime >= groundedCooldown)
            {
                setGrounded(true);
                groundedTime = 0;
            }
        }
        else if (isGrounded)
        {
            groundedTimerActive = false;
        }
    }
    void setGrounded(bool state) 
    {
        isGrounded = state;
    }
    void Move() 
    {
        transform.position = playerMovementVector;
    }

    public void setGunComp(ShotgunJump gun) 
    {
        gunJump = gun;
    }
}

