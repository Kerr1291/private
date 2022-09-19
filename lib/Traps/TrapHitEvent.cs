using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameEvents
{
    public class TrapHitEvent
    {
        // Remove buff from avatar
        public PlatformerAvatar avatar;
        public TrapCollisionDetector trap;
    }

    public class TrapHitCompleted
    {
        public PlatformerAvatar avatar;
    }

    public class TryToInteractMsg
    {
        public PlatformerAvatar avatar;
    }
}
