using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum DIRECTION
{
    FORWARD = 1 << 0,  // 1
    BACKWARD = 1 << 1, // 2
    LEFT = 1 << 2,     // 4
    RIGHT = 1 << 3,    // 8
    MAX = 1 << 4       // 16
}

public static class Direction
{
    public const uint BITMASK_FORWARD = (uint)DIRECTION.FORWARD;
    public const uint BITMASK_BACKWARD = (uint)DIRECTION.BACKWARD;
    public const uint BITMASK_LEFT = (uint)DIRECTION.LEFT;
    public const uint BITMASK_RIGHT = (uint)DIRECTION.RIGHT;

    public const uint BITMASK_FORWARDLEFT = (uint)DIRECTION.FORWARD | (uint)DIRECTION.LEFT;
    public const uint BITMASK_FORWARDRIGHT = (uint)DIRECTION.FORWARD | (uint)DIRECTION.RIGHT;
    public const uint BITMASK_BACKWARDLEFT = (uint)DIRECTION.BACKWARD | (uint)DIRECTION.LEFT;
    public const uint BITMASK_BACKWARDRIGHT = (uint)DIRECTION.BACKWARD | (uint)DIRECTION.RIGHT;
}

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 100f;

    private uint bitDirection = 0;
    private PlayerState playerState;

    Animator animator;
    AnimatorStateInfo aimPitchState;
    AnimatorStateInfo aimYawState;

    public float a;
    public float b;

    void Start()
    {
        playerState = GetComponent<PlayerState>();

        animator = GetComponent<Animator>();
        aimPitchState = GetComponent<Animator>().GetNextAnimatorStateInfo(1);
        aimYawState = GetComponent<Animator>().GetNextAnimatorStateInfo(2);
    }

    void Update()
    {
    //  float moveHorizontal = Input.GetAxis("Horizontal");
    //  float moveVertical = Input.GetAxis("Vertical");

        Handle_Bitset();
        Handle_MouseInput();
    }

    void Handle_Bitset()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SetDirection(Direction.BITMASK_FORWARD);
        else if (Input.GetKeyUp(KeyCode.W))
            SetDirection(Direction.BITMASK_FORWARD, false);
        if (Input.GetKeyDown(KeyCode.A))
            SetDirection(Direction.BITMASK_LEFT);
        else if (Input.GetKeyUp(KeyCode.A))
            SetDirection(Direction.BITMASK_LEFT, false);
        if (Input.GetKeyDown(KeyCode.S))
            SetDirection(Direction.BITMASK_BACKWARD);
        else if (Input.GetKeyUp(KeyCode.S))
            SetDirection(Direction.BITMASK_BACKWARD, false);
        if (Input.GetKeyDown(KeyCode.D))
            SetDirection(Direction.BITMASK_RIGHT);
        else if (Input.GetKeyUp(KeyCode.D))
            SetDirection(Direction.BITMASK_RIGHT, false);
    }

    void Handle_MouseInput()
    {
        Vector3 camLook = GameInstance.Instance.mbCamera.transform.forward.normalized;
        float pitch = Mathf.Atan2(camLook.y, Mathf.Sqrt(Mathf.Pow(camLook.x, 2f) + Mathf.Pow(camLook.z, 2f)));
        float yaw = 0.5f * (Vector3.Dot(transform.right.normalized, camLook) + 1f);
        pitch = Mathf.Clamp(Utility.ProportionalRatio(pitch, GameInstance.minRadPitch * .5f, GameInstance.maxRadPitch * .5f), 0f, 1f);

        animator.SetLayerWeight(1, 2f * Mathf.Abs(0.5f - pitch));
        animator.SetLayerWeight(2, 2f * Mathf.Abs(0.5f - yaw));
        animator.Play(aimPitchState.fullPathHash, 1, pitch);
        animator.Play(aimYawState.fullPathHash, 2, yaw);
    }    

    void Handle_KeyInput()
    {
        switch (bitDirection)
        {
            case Direction.BITMASK_FORWARD:
            {
                if (Input.GetKeyDown(KeyCode.LeftControl))
                    playerState.SetState(RG_STATE.SPRINT);
            }
            break;
            case Direction.BITMASK_BACKWARD:
            {
            }
            break;
            case Direction.BITMASK_LEFT:
            {
            }
            break;
            case Direction.BITMASK_RIGHT:
            {
            }
            break;
            case Direction.BITMASK_FORWARDLEFT:
            {
            }
            break;
            case Direction.BITMASK_FORWARDRIGHT:
            {
            }
            break;
            case Direction.BITMASK_BACKWARDLEFT:
            {
            }
            break;
            case Direction.BITMASK_BACKWARDRIGHT:
            {
            }
            break;
        }
    }

    public void SetDirection(uint bitmask, bool value = true)
    {
        if (value)
            bitDirection |= bitmask;
        else
            bitDirection &= ~bitmask;
    }

    public bool IsDirection(uint direction)
    {
        return (bitDirection & direction) != 0;
    }
}
