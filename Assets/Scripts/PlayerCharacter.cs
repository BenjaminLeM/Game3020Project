using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    float speed;
    [SerializeField]
    float walkSpeed;
    [SerializeField]
    float jumpHeight;
    bool isSprinting = false;
    [SerializeField]
    float sprintSpeed;
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

    #region
    public void setPlayerMoveSpeed(float newSpeed) 
    {
        speed = newSpeed;
    }
    public float getPlayerMoveSpeed() 
    {
        return speed;
    }

    public float WalkSpeed() 
    {
        return walkSpeed;
    }

    public float JumpHeight()
    {
        return jumpHeight;
    }
    public float SprintSpeed()
    {
        return sprintSpeed;
    }

    public float CrouchSpeed() 
    {
        return crouchSpeed;
    }

    public float SlideCooldown() 
    {
        return slideCooldown;
    }

    public float SlideSpeed() 
    {
        return slideSpeed;
    }
#endregion
    public void setCurrentHeight(float newHeight) 
    {
        currentHeight = newHeight;
        setPlayerScale(new Vector3(transform.localScale.x,
                                            currentHeight,
                                            transform.localScale.z));
    }

    public float getCurrentHeight() 
    {
        return currentHeight;
    }

    public float getCrouchHeight() 
    {
        return crouchHeight;
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
}
