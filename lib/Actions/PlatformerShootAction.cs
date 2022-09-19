using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
using nv;

public class PlatformerShootAction : BaseAction
{
    public PlatformerMotor2D platformerController;

    [Header("Ususally the tip of the gun")]
    public Transform shotSpawnPoint;

    [Header("Controller triggering this action")]
    public GameAvatar owner;

    [Header("Optional animation to play on shooting")]
    public Animator characterAnimator;

    public string attackTrigger;

    [Header("Object pool that contains the shots")]
    public ShotList shotList;

    [Header("Current type of shot that shooting will produce")]
    public GameObject shotTypePrefab;

    //delay between shots, usually pulled from the shot prefab
    public TimedRoutine shotDelay;

    bool CreateShot()
    {
        ShotData nShot = new ShotData();
        nShot.IsAlive = true;
        nShot.viewPrefab = shotTypePrefab.name;
        nShot.shotData = "Shot: "+shotTypePrefab.name +" "+ shotList.DataCount;

        //set the shot owner and spawn
        nShot.owner = owner;
        nShot.spawnPoint = shotSpawnPoint.position;

        nShot.ignoreList = new List<GameObject>();
        nShot.ignoreList.Add( platformerController.gameObject );

        //set the direction the shot will travel
        if( platformerController.facingLeft )
        {
            nShot.shotDirection = Vector3.left;
        }
        else
        {
            nShot.shotDirection = Vector3.right;
        }

        //set the shot cooldown
        BaseShotBehavior shotBehavior = shotTypePrefab.GetComponent<BaseShotBehavior>();

        shotDelay = new TimedRoutine( shotBehavior.cooldown );

        shotList.Add( nShot );
        return true;
    }

    void TryAttack()
    {
        if( shotDelay.IsRunning )
            return;

        if( !CreateShot() )
            return;

        shotDelay.Start();

        if( characterAnimator == null )
            return;

        if( !string.IsNullOrEmpty(attackTrigger) )
            characterAnimator.SetTrigger( attackTrigger );
    }

    //Call when button is pressed
    public override void TryStartAction()
    {
        TryAttack();
    }

    //Call when button is released
    public override void TryStopAction() { }

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

    void Awake()
    {
        if( shotList.Loaded == false )
            shotList.Setup();

        Enabled = true;
    }

    public void OnActionEnabled()
    {
        //_motor.onJump += PlayRandomJumpSound;
    }

    public void OnActionDisabled()
    {
        //_motor.onJump -= PlayRandomJumpSound;
    }
}
