using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using nv;
using GameEvents;

public class TrapDoorLever : MonoBehaviour
{
    public Collider2D leverTriggerSpace = null;
    public Animator leverAnimator = null;
    public float doorTimer = 1f;
    public List<TrapDoor> doorList = new List<TrapDoor>();
    CommunicationNode communicationNode = new CommunicationNode();
    bool open = false;
    TimedRoutine closeDoors = null;

    private void Awake()
    {
        closeDoors = new TimedRoutine( doorTimer, CloseDoorsTimer );
    }
    [CommunicationCallback]
    void TryToInteract(TryToInteractMsg msg)
    {
        if(leverTriggerSpace.OverlapPoint(msg.avatar.GetAvatarObject().transform.position))
        {
            open = !open;
            if (open)
            {
                closeDoors.Start();
            }
            else
            {
                closeDoors.Reset();
            }
            if (leverAnimator!=null)
            {
                if(open)
                {
                    leverAnimator.SetTrigger("On");
                }
                else
                {
                    leverAnimator.SetTrigger("Off");
                }
                
            }
            UpdateDoors(open);
            
        }        
    }

    void UpdateDoors(bool open)
    {
        for(int i=0;i<doorList.Count;++i)
        {
            if (open)
            {
                doorList[i].Open();
            }
            else
            {
                doorList[i].Close();
            }
        }
    }

    void CloseDoorsTimer()
    {
        leverAnimator.SetTrigger("Off");
        UpdateDoors(false);
    }
    private void OnEnable()
    {
        communicationNode.EnableNode(this);
        open = false;
        leverAnimator.SetTrigger("Off");
        
    }     

   
    private void OnDisable()
    {
        communicationNode.DisableNode();
        closeDoors.Reset();
    }
}
