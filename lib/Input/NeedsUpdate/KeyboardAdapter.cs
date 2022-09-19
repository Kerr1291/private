using UnityEngine;
using System.Collections;

using Rewired;

namespace nv
{

    public class KeyboardAdapter : MonoBehaviour
    {
        [Header("The unity input manager will attempt to use this adapter")]
        public PlayerInputInterface _input_interface;

        void Update()
        {
            if( _input_interface == null )
                return;

            if( _input_interface.inputTarget == null )
                return;

            GameAvatar target = _input_interface.inputTarget;

            InputActionEventData null_data = new InputActionEventData();

            if( Input.GetKey( KeyCode.W ) )
                target.HandleInputEvent_LStick_Up( null_data );
            if( Input.GetKey( KeyCode.S ) )
                target.HandleInputEvent_LStick_Down( null_data );
            if( Input.GetKey( KeyCode.A ) )
                target.HandleInputEvent_LStick_Left( null_data );
            if( Input.GetKey( KeyCode.D ) )
                target.HandleInputEvent_LStick_Right( null_data );


            if( Input.GetKeyDown( KeyCode.E ) || Input.GetKeyUp( KeyCode.E ) || Input.GetKeyDown( KeyCode.Space ) || Input.GetKeyUp( KeyCode.Space ) )
                target.HandleInputEvent_A_BTN( null_data );
            if( Input.GetKeyDown( KeyCode.LeftAlt ) || Input.GetKeyUp( KeyCode.LeftAlt ) || Input.GetKeyDown( KeyCode.Q ) || Input.GetKeyUp( KeyCode.Q ) )
                target.HandleInputEvent_X_BTN( null_data );
            if( Input.GetKeyDown( KeyCode.R ) || Input.GetKeyUp( KeyCode.R ) )
                target.HandleInputEvent_B_BTN( null_data );
            if( Input.GetKeyDown( KeyCode.F ) || Input.GetKeyUp( KeyCode.F ) )
                target.HandleInputEvent_Y_BTN( null_data );
            if( Input.GetKeyDown( KeyCode.Escape ) )
                target.HandleInputEvent_StartButton( null_data );
        }
    }

}