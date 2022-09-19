using UnityEngine;
using System.Collections;

public class SwingWeaponAction : BaseAction {

    public Animator characterAnimator;

    public string attackTrigger;

    void TryAttackAnimation()
    {
        if( characterAnimator == null )
            return;

        characterAnimator.SetTrigger( attackTrigger );
    }

    //Call when button is pressed
    public override void TryStartAction()
    {
        TryAttackAnimation();
    }

    //Call when button is released
    public override void TryStopAction() { }

    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }

        set
        {
            base.Enabled = value;
        }
    }

    void Awake()
    {
        Enabled = true;
    }
}
