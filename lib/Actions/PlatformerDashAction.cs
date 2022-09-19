using UnityEngine;
using System.Collections;

using nv;

public class PlatformerDashAction : BaseAction
{
    public PlatformerMotor2D motor;

    public AudioClipSet dashSound;

    public bool startEnabled = false;

    void PlayDashSound()
    {
        if( dashSound == null )
            return;

        dashSound.Play();
    }

    void TryDash()
    {
        if( !Enabled )
            return;

        motor.Dash();
    }

    //Call when button is pressed
    public override void TryStartAction()
    {
        TryDash();
    }

    //Call when button is released
    public override void TryStopAction()
    {
    }

    public override bool Enabled {
        get {
            return base.Enabled;
        }

        set {
            if( base.Enabled != value )
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
        motor.onDash += PlayDashSound;
    }

    public void OnActionDisabled()
    {
        motor.onDash -= PlayDashSound;
    }

    void Awake()
    {
        Enabled = startEnabled;
    }
}
