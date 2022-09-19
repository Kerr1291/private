using UnityEngine;
using System.Collections;
using System.Reflection;
using Rewired;

namespace nv
{
    /// <summary>
    /// Base class for all input actions
    /// </summary>
    [System.Serializable]
    public class InputAction
    {
        public PlayerInputInterface inputInterface;

        //public virtual string GetActionTypeName() { return "None"; }
        public string name;
        public string methodName;

        //public delegate void ActionCallback( InputActionEventData player );
        
        public System.Action<InputActionEventData> DoAction;

        public void DefaultAction( InputActionEventData player ) { Debug.LogError( string.Format("Nothing assigned to action {0}!",name) ); }

        public InputAction()
        {
            name = "DefaultAction";
            DoAction = DefaultAction;
        }
    }
}