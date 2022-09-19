using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nv;
using GameEvents;

public class AvatarTrapsReaction : MonoBehaviour {

    CommunicationNode communicationNode = new CommunicationNode();

    void OnEnable()
    {
        communicationNode.EnableNode(this);
    }

    private void OnDisable()
    {
        communicationNode.DisableNode();
    }

    /*[CommunicationCallback]
    void HandleTrapCollision(TrapHitEvent msg)
    {
        //remove the collision so it does not get detect again
        Debug.Log("handle hit");
        RequestRemoveBuffFromPlayer(msg.avatar);
    }

    [CommunicationCallback]
    void HandleTrapCollision(UpdateBuffViewEvent msg)
    {
        if(msg.buffStack.Count>0)
        {
            Debug.Log("Transport player");
        }
        else
        {
            ;
        }
    }*/

    void RequestRemoveBuffFromPlayer(PlatformerAvatar avatar)
    {
        Debug.Log("Remove buff");
        RemoveBuffEvent msg =new RemoveBuffEvent();
        msg.Avatar = avatar;
        msg.amountToRemove = 4;
        msg.removeType = RemoveType.TRAP;
        communicationNode.Publish(msg);
    }
    //Simply move the character to a specific place
}
