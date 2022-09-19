using UnityEngine;
using System.Collections;

public class BaseMovement : MonoBehaviour {

    //orientation of the object
    public GameObject model;

    //action to take, given this movement request
    public virtual void Move( Vector2 dir ) { }
}
