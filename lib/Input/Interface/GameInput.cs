using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Rewired;

namespace nv
{
    public class GameInput : GameSingleton<GameInput>
    {
        [SerializeField]
        CommunicationNode comms;

        static public bool IsControllerNew( ControllerStatusChangedEventArgs args )
        {
            foreach( PlayerInputInterface pii in _loaded_players )
            {
                if( true == pii.RewiredPlayer.controllers.ContainsController( ReInput.controllers.GetJoystick( args.controllerId ) ) )
                    return false;
            }

            return true;
        }

        static List<PlayerInputInterface> _loaded_players = new List<PlayerInputInterface>();

        public static PlayerInputInterface GetPlayerControllingAvatar( GameAvatar avt )
        {
            foreach( PlayerInputInterface p_interface in GameInput._loaded_players )
            {
                if( p_interface.GetAvatar() == avt )
                    return p_interface;
            }
            return null;
        }

        public static List<PlayerInputInterface> GetAllPlayers()
        {
            return GameInput._loaded_players;
        }

        public static PlayerInputInterface GetPlayer( int id )
        {
            for( int i = 0; i < _loaded_players.Count; ++i )
            {
                if( _loaded_players[ i ].PlayerID == id )
                    return _loaded_players[ i ];
            }
            return null;
        }

        static public void InitPlayerInput( PlayerInputInterface player )
        {
            if( GameInput._loaded_players.Contains( player ) )
                return;

            int next_size = GameInput._loaded_players.Count + 1;

            if( next_size > ReInput.players.Players.Count )
            {
                Dev.LogWarning( "ReWired is currently setup to support " + ReInput.players.Players.Count + " players. You'll need to add more in the rewired editor if you want more. Cannot add new controller/player: " + player.name );
                return;
            }

            int player_index = player.PlayerID;
            //GameInput._loaded_players.Count;

            //player._player = ReInput.players.Players[ player_index ];
            player.INTERNAL_SetRewiredPlayer( ReInput.players.Players[ player_index ] );
            player.INTERNAL_SetRewiredPlayerIndex( player_index );// GameInput._loaded_players.Count );
                                                                  //player._rewired_player_index = GameInput._loaded_players.Count;

            GameInput._loaded_players.Add( player );
        }

        //static public PlayerInputInterface GetInterfaceFromPlayer( Player player )
        //{
        //    foreach( PlayerInputInterface p_interface in GameInput._loaded_players )
        //    {
        //        if( p_interface._player == player )
        //            return p_interface;
        //    }
        //    return null;
        //}

        void Awake()
        {
            comms.EnableNode( this );
        }
    }
}