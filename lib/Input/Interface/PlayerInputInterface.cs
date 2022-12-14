using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using Rewired;

namespace nv
{

    [System.Serializable]
    public class InputPair
    {
        //These vars are setup through the editor script for PlayerInputInterface
        public string _rw_action;
        public int _rw_action_index = 0;
        public InputAction _action;
        public int _action_index = 0;
        public InputActionEventType _action_type;
        //public InputActionContainer _action_obj;

        //these are set on BindActions() below 
        public Player _player = null;
        public PlayerInputInterface _owner = null;
    }

    public class PlayerInputInterface : MonoBehaviour
    {
        [Header("This MUST be unique across the game")]
        [SerializeField]
        private int _playerID = 0;

        public GameAvatar inputTarget = null;

        public int PlayerID {
            get {
                return _playerID;
            }
        }

        public string GetPlayerIDString()
        {
            string player_id = "Player " + Convert.ToString(PlayerID);
            return player_id;
        }

        public string GetPlayerIDStringPlusOne()
        {
            string player_id = "Player " + Convert.ToString(PlayerID + 1);
            return player_id;
        }

        public bool IsControllerConnected {
            get {

                if( _player.controllers.joystickCount <= 0 )
                    return false;

                return ReInput.controllers.IsControllerAssignedToPlayer( ControllerType.Joystick, _player.controllers.Joysticks[ 0 ].id, _player.id );
            }
        }

        //the "true" id
        public int ReWiredPlayerID {
            get {
                return _player.id;
            }
        }

        public Player RewiredPlayer {
            get {
                return _player;
            }
        }

        //player and index are set by GameInput
        [HideInInspector]
        [SerializeField]
        int _rewired_player_index = -1;

        /// <summary>
        /// Only to be used by GameInput
        /// </summary>
        /// <param name="player"></param>
        public void INTERNAL_SetRewiredPlayerIndex( int index )
        {
            _rewired_player_index = index;
        }

        //[HideInInspector]
        [SerializeField]
        Player _player;

        /// <summary>
        /// Only to be used by GameInput
        /// </summary>
        /// <param name="player"></param>
        public void INTERNAL_SetRewiredPlayer( Player player )
        {
            _player = player;
        }

        //this is set in this class's editor script
        [HideInInspector]
        public List<InputPair> INTERNAL_inputBindings = new List<InputPair>();

        /// <summary>
        /// Should only ever be called once to assign the player to this instance
        /// </summary>
        void Init()
        {
            ///this may fail if there are not enough players defined in the rewired input settings
            GameInput.InitPlayerInput( this );

            //in this case the load failed because rewired hasn't been setup for enough players, disable this player
            if( _rewired_player_index == -1 )
            {
                gameObject.SetActive( false );
                return;
            }

            //bind rewired actions to functions
            BindActions();

            if( inputTarget != null )
            {
                SetAvatar( inputTarget );
            }
        }

        void AssignJoystickToPlayer( Joystick j )
        {
            //only oen per player allowed :)
            if( _player.controllers.joystickCount > 0 )
                return;

            _player.controllers.AddController( j, true ); // assign joystick to player
        }

        void OnJoystickConnected( ControllerStatusChangedEventArgs args )
        {
            if( _rewired_player_index != -1 )
                return;

            //only allow controllers :P
            if( args.controllerType != ControllerType.Joystick )
                return;

            Dev.Log( "Controller connected: " + args.name );

            //is this a new connection?
            if( GameInput.IsControllerNew( args ) == false )
            {
                Dev.Log( "Controller already assigned. Old connection should be restored now." );
                return;
            }

            Dev.Log( "controller assigned" );

            //assign it to us
            AssignJoystickToPlayer( ReInput.controllers.GetJoystick( args.controllerId ) );
        }

        void OnJoystickDisconnected( ControllerStatusChangedEventArgs args )
        {
            if( _rewired_player_index != -1 )
                return;

            Dev.Log( "Controller lost: " + args.name );

            //was this our controller?
            if( false == _player.controllers.ContainsController( ReInput.controllers.GetJoystick( args.controllerId ) ) )
                return;

            //remove it
            _player.controllers.RemoveController( ReInput.controllers.GetJoystick( args.controllerId ) );
        }

        void BindActions()
        {
            foreach( InputPair input_action in INTERNAL_inputBindings )
            {
                //Dev.Log( "binding " + input_action._action.GetActionTypeName() + " to action " + input_action._rw_action );

                Action<InputActionEventData> doAction = (InputActionEventData x) =>
                {                    
                    var gameAction = inputTarget.GetType().GetMethod( input_action._action.methodName );

                    gameAction.Invoke(inputTarget, new object[]{ x } );
                };

                input_action._action.DoAction = doAction;

                _player.AddInputEventDelegate( input_action._action.DoAction, UpdateLoopType.Update, input_action._action_type, input_action._rw_action );

                input_action._player = _player;
                input_action._owner = this;
            }
        }

        void UnbindActions()
        {
            foreach( InputPair input_action in INTERNAL_inputBindings )
            {
                _player.RemoveInputEventDelegate( input_action._action.DoAction, UpdateLoopType.Update, input_action._action_type, input_action._rw_action );

                input_action._player = _player;
                input_action._owner = this;
            }
        }

        public void SetAvatar( GameAvatar target )
        {
            //notify previous avatar that it's losing focus
            if( inputTarget != null && inputTarget != target )
                inputTarget.NotifyFocusLost( this );

            //notify the next avatar that it now has focus
            if( target != null )
                target.NotifyFocus( this );

            inputTarget = target;
        }

        public GameAvatar GetAvatar()
        {
            return inputTarget;
        }

        void Awake()
        {
            ReInput.ControllerConnectedEvent += OnJoystickConnected;
            ReInput.ControllerDisconnectedEvent += OnJoystickDisconnected;

            Init();
        }
    }

}