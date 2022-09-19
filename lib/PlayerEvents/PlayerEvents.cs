using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nv;
 
namespace GameEvents
{

    public class PlayerJoinedEvent
    {
        public PlayerInputInterface playerThatJoined;
    }

    public class PlayerQuitEvent
    {
        public PlayerInputInterface playerThatQuit;
    }
}
