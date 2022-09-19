using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using nv;

public class PlatformerJumpAction : BaseAction
{
    public PlatformerMotor2D _motor;

    public AudioClipSet jumpSounds;

    void PlayRandomJumpSound() 
    {
        jumpSounds.PlayRandom();
    }

    void TryJump()
    {
        _motor.Jump();
        _motor.DisableRestrictedArea();
    }

    //Call when button is pressed
    public override void TryStartAction()
    {
        TryJump();
        _motor.jumpingHeld = true;
    }

    //Call when button is released
    public override void TryStopAction()
    {
        _motor.jumpingHeld = false;
    }

    public override bool Enabled {
        get {
            return base.Enabled;
        }

        set {
            if( base.Enabled != value)
            {
                if( value )
                    OnActionEnabled();
                else
                    OnActionDisabled();
            }
            base.Enabled = value;
        }
    }

    public void OnActionEnabled()
    {
        _motor.onJump += PlayRandomJumpSound;
    }

    public void OnActionDisabled()
    {
        _motor.onJump -= PlayRandomJumpSound;
    }

    void Awake()
    {
        Enabled = true;
    }
}
