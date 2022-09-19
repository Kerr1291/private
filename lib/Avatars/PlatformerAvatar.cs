using UnityEngine;
using System.Collections;

using nv;
using Rewired;

/// Ent is short for Entity and is anything that a player could control in the game.
public class PlatformerAvatar : GameAvatar
{
    protected PlayerInputInterface _owner;
    
    public PlatformerMotor2D platformerController;
    public PlatformerMovement movementController;

    public BaseAction buttonA;
    public BaseAction buttonB;
    public BaseAction buttonC;
    public BaseAction buttonD;

    bool keyboadAdapterA = false;
    bool keyboadAdapterB = false;
    bool keyboadAdapterX = false;
    bool keyboadAdapterY = false;

    [SerializeField]
    CommunicationNode eventSystem;
    
    public override GameObject GetAvatarObject()
    {
        return gameObject;
    }

    public int GetPlayerID()
    {
        if( _owner != null )
            return _owner.PlayerID;
        return -1;
    }

    public bool IsPlayerControlled()
    {
        if( _owner == null )
            return false;

        if( _owner.RewiredPlayer.controllers.joystickCount <= 0 )
            return false;

        return true;
    }

    Vector2 up { get { return Vector2.up; } }
    Vector2 down { get { return Vector2.down; } }
    Vector2 right { get { return Vector2.right; } }
    Vector2 left { get { return Vector2.left; } }

    #region SpecialCallbacks

    ///Called by PlayerInputInterface when SetGameTarget causes a player's GAME input to go to this pix
    public override void NotifyFocus( PlayerInputInterface player )
    {
        _owner = player;
        eventSystem.EnableNode( this );
    }


    ///Called by PlayerInputInterface when SetGameTarget causes a player to leave this item's focus
    public override void NotifyFocusLost( PlayerInputInterface player )
    {
        GameEvents.PlayerQuitEvent quitEvent = new GameEvents.PlayerQuitEvent
        {
            playerThatQuit = _owner
        };
        eventSystem.Publish( quitEvent );
        eventSystem.DisableNode();
        _owner = null;
    }

    #endregion

    /// I use the xbox button inputs for ease of reference
    /// In case you forget :) here's the layout
    /// Also note that pressing down on a stick is a button
    /// 
    ///LTRIGGER                                RTRIGGER  
    /// LBUMPER                                RBUMPER
    /// 
    ///  LSTICK                             Y_BTN
    ///             SELECT (X) START    X_BTN   B_BTN
    ///      DPAD                RSTRICK    A_BTN
    /// 
    #region Buttons

    public override void HandleInputEvent_StartButton( InputActionEventData data ) { }
    public override void HandleInputEvent_SelectButton( InputActionEventData data ) { }

    public override void HandleInputEvent_A_BTN( InputActionEventData data )
    {
        if( buttonA != null )
        {
            if( data.actionId == 0 )
            {
                keyboadAdapterA = !keyboadAdapterA;
                if( keyboadAdapterA )
                    buttonA.TryStartAction();
                else if( !keyboadAdapterA )
                    buttonA.TryStopAction();
            }
            else
            {
                if( keyboadAdapterA )
                    return;

                if( data.eventType == InputActionEventType.ButtonJustPressed )
                    buttonA.TryStartAction();
                else if( data.eventType == InputActionEventType.ButtonJustReleased )
                    buttonA.TryStopAction();
            }
        }
    }

    public override void HandleInputEvent_B_BTN( InputActionEventData data )
    {
        if( buttonB != null )
        {
            if( data.actionId == 0 )
            {
                keyboadAdapterB = !keyboadAdapterB;
                if( keyboadAdapterB )
                    buttonB.TryStartAction();
                else if( !keyboadAdapterX )
                    buttonB.TryStopAction();
            }
            else
            {
                if( keyboadAdapterB )
                    return;

                if( data.eventType == InputActionEventType.ButtonJustPressed )
                    buttonB.TryStartAction();
                else if( data.eventType == InputActionEventType.ButtonJustReleased )
                    buttonB.TryStopAction();
            }
        }
    }

    public override void HandleInputEvent_X_BTN( InputActionEventData data )
    {
        if( buttonC != null )
        {
            //Dev.LogVar( data.actionId );
            if( data.actionId == 0 )
            {
                keyboadAdapterX = !keyboadAdapterX;
                if( keyboadAdapterX )
                    buttonC.TryStartAction();
                else if( !keyboadAdapterX )
                    buttonC.TryStopAction();
            }
            else
            {
                if( keyboadAdapterX )
                    return;

                if( data.eventType == InputActionEventType.ButtonJustPressed )
                    buttonC.TryStartAction();
                else if( data.eventType == InputActionEventType.ButtonJustReleased )
                    buttonC.TryStopAction();
            }
        }
    }

    public override void HandleInputEvent_Y_BTN( InputActionEventData data )
    {
        if( buttonD != null )
        {
            if( data.actionId == 0 )
            {
                keyboadAdapterY = !keyboadAdapterY;
                if( keyboadAdapterY )
                    buttonD.TryStartAction();
                else if( !keyboadAdapterY )
                    buttonD.TryStopAction();
            }
            else
            {
                if( keyboadAdapterY )
                    return;

                if( data.eventType == InputActionEventType.ButtonJustPressed )
                    buttonD.TryStartAction();
                else if( data.eventType == InputActionEventType.ButtonJustReleased )
                    buttonD.TryStopAction();
            }
        }
    }

    public override void HandleInputEvent_L_BUMPER( InputActionEventData data ) { }
    public override void HandleInputEvent_R_BUMPER( InputActionEventData data ) { }
    public override void HandleInputEvent_L_TRIGGER( InputActionEventData data )
    {
    }
    public override void HandleInputEvent_R_TRIGGER( InputActionEventData data )
    {
    }
    public override void HandleInputEvent_L_STICK_BTN( InputActionEventData data ) { }
    public override void HandleInputEvent_R_STICK_BTN( InputActionEventData data ) { }

    #endregion


    #region StickMethods

    public override void HandleInputEvent_LStick_Up( InputActionEventData data )
    {
        //if( data != null )
        //float axis_value = data.GetAxis();
        movementController.Move( up );
    }
    public override void HandleInputEvent_LStick_Down( InputActionEventData data )
    {
        //float axis_value = data.GetAxis();
        movementController.Move( down );
    }
    public override void HandleInputEvent_LStick_Left( InputActionEventData data )
    {
        //float axis_value = data.GetAxis();
        movementController.Move( left );
    }
    public override void HandleInputEvent_LStick_Right( InputActionEventData data )
    {
        //float axis_value = data.GetAxis();
        movementController.Move( right );
    }

    public override void HandleInputEvent_RStick_Up( InputActionEventData data ) { }
    public override void HandleInputEvent_RStick_Down( InputActionEventData data ) { }
    public override void HandleInputEvent_RStick_Left( InputActionEventData data ) { }
    public override void HandleInputEvent_RStick_Right( InputActionEventData data ) { }

    #endregion

    #region DPadMethods

    public override void HandleInputEvent_DPad_Up( InputActionEventData data ) { }
    public override void HandleInputEvent_DPad_Down( InputActionEventData data ) { }
    public override void HandleInputEvent_DPad_Left( InputActionEventData data ) { }
    public override void HandleInputEvent_DPad_Right( InputActionEventData data ) { }

    #endregion
}
