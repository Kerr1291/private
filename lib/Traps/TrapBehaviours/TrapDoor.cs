using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    Animator trappDoorAnimator = null;
    Collider2D doorCollider = null;

    private void Awake()
    {
        trappDoorAnimator = GetComponent<Animator>();
        doorCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        trappDoorAnimator.SetTrigger("Default");
        doorCollider.enabled = true;
    }
    
    public void Open()
    {
        trappDoorAnimator.SetTrigger("Open");
        doorCollider.enabled = false;
    }

    public void Close()
    {
        trappDoorAnimator.SetTrigger("Close");
        doorCollider.enabled = true;
    }

}
