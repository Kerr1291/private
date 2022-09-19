using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nv;
using GameEvents;

public class PlatformerInteract : BaseAction
{
    public PlatformerAvatar avatar = null;

    public override void TryStartAction()
    {
        TryToInteractMsg msg=new TryToInteractMsg();
        msg.avatar = this.avatar;
        CommunicationNode.publish(msg, this);
        avatar.platformerController.DisableRestrictedArea();
        avatar.platformerController.ForceJump( 0.001f );
    }
}
