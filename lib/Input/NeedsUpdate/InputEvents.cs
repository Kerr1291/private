using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using Rewired;

namespace GameEvents.ControllerEvents
{
    public class ControllerInputEvent
    {
        public InputActionEventType type;
        public string action;
        public GameAvatar player;
    }

    //////////////////////////////////////////////////////////
    //Stick/Axis events

    public class AxisEvent : ControllerInputEvent
    {
        public float axisValue;
    }

    public class AxisHorizontalEvent : AxisEvent { }
    public class AxisVerticalEvent : AxisEvent { }

    public class AxisLeftEvent : AxisHorizontalEvent { }
    public class AxisRightEvent : AxisHorizontalEvent { }
    public class AxisUpEvent : AxisVerticalEvent { }
    public class AxisDownEvent : AxisVerticalEvent { }

    //////////////////////////////////////////////////////////
    //Button Events

    public class ButtonEvent : ControllerInputEvent { }

    public class ButtonLStickEvent : ButtonEvent { }
    public class ButtonRStickEvent : ButtonEvent { }

    public class ButtonLBumperEvent : ButtonEvent { }
    public class ButtonRBumperEvent : ButtonEvent { }

    public class ButtonAEvent : ButtonEvent { }
    public class ButtonBEvent : ButtonEvent { }
    public class ButtonXEvent : ButtonEvent { }
    public class ButtonYEvent : ButtonEvent { }

    public class ButtonSelectEvent : ButtonEvent { }
    public class ButtonStartEvent : ButtonEvent { }

    public class ButtonLTriggerEvent : ButtonEvent { }
    public class ButtonRTriggerEvent : ButtonEvent { }
}
