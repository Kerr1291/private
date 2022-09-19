using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nv;

public class AISimpleGroundMovement : MonoBehaviour
{
    public BasicEnemyAvatar avatar;

    public PlatformerMovement movement;

    public PlatformerMotor2D motor;

    public Raycaster groundCheck;

    public bool avoidFallingOffEdges = true;

    public TimedRoutine cornerCooldown;
    public float cornerCooldownTime = 1f;

    public bool turnAroundAtWalls = true;
    
    public bool jumpAtWalls = true;

    public TimedRoutine turnCooldown;
    public float turnCooldownTime = 1f;

    public PlatformerJumpAction jumpAction;

    public Raycaster wallCheck;

    void Update()
    {
        if( motor.IsJumping() == false && motor.IsFalling() )
        {
            motor.normalizedYMovement = 0f;
            motor.normalizedXMovement = 0f;
        }
        else
        {
            if( avoidFallingOffEdges && motor.IsJumping() == false )
            {
                if( !cornerCooldown.IsRunning && !groundCheck.RaycastHasHit() )
                {
                    cornerCooldown.Start( cornerCooldownTime );
                    motor.facingLeft = !motor.facingLeft;
                }
            }
            
            if( !jumpAtWalls && turnAroundAtWalls )
            {
                if( !turnCooldown.IsRunning && wallCheck.RaycastHasHit() )
                {
                    turnCooldown.Start();
                    motor.facingLeft = !motor.facingLeft;
                }
            }

            if( jumpAtWalls && wallCheck.RaycastHasHit() )
            {
                jumpAction.TryStartAction();
                if( turnAroundAtWalls && !turnCooldown.IsRunning )
                {
                    turnCooldown.Start( TurnAroundAfterFailedJumps, turnCooldownTime );
                }
            }

            if( motor.facingLeft )
                movement.Move( Vector2.left );
            else
                movement.Move( Vector2.right );
        }
    }

    void OnDestroy()
    {
        turnCooldown.Reset();
    }

    void TurnAroundAfterFailedJumps()
    {
        if( wallCheck.RaycastHasHit() )
        {
            motor.facingLeft = !motor.facingLeft;
        }
    }
}
