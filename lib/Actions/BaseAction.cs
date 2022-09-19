using UnityEngine;
using System.Collections;

public class BaseAction : MonoBehaviour
{
    //Call when button is pressed
    public virtual void TryStartAction() { }

    //Call when button is released
    public virtual void TryStopAction() { }

    public virtual bool Enabled
    {
        get; set;
    }
}
