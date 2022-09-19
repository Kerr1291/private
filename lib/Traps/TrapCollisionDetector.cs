using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nv;
using GameEvents;

public class TrapCollisionDetector : MonoBehaviour
{
    public enum TrapType { FIRE, SPIKE, WATER };
    public TrapType type = TrapType.FIRE;
    CommunicationNode communicationNode = new CommunicationNode();

    //players who just fall into this trap and need to be locked from collisions for a bit
    List<PlatformerAvatar> ignoreCollisionWith = new List<PlatformerAvatar>();

    private void OnEnable()
    {
        communicationNode.EnableNode(this);
        ignoreCollisionWith.Clear();
    }

    private void OnDisable()
    {
        //communicationnode.DisableNode();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        
        PlatformerAvatar playerAvatar = coll.GetComponentInChildren<PlatformerAvatar>();

        if (playerAvatar == null)
            return;
        //check if we are not still processing this avatar
        if (ignoreCollisionWith.Count > 0 && ignoreCollisionWith.Contains(playerAvatar))
        {
            return;
        }
        Debug.Log("Collision processed");
        ignoreCollisionWith.Add(playerAvatar);

        CameraShake.StartShake msg = new CameraShake.StartShake();
        msg.shakeTime = .25f;
        communicationNode.Publish( msg );

        PixelExplosionMaker.Instance.MakePlayerExplosion( playerAvatar.GetAvatarObject().transform.position );

        RequestRemoveBuffFromPlayer(playerAvatar);
    }

    [CommunicationCallback]
    void CollisionCompleted(TrapHitCompleted msg)
    {
        Debug.Log("Collision completed");
        ignoreCollisionWith.Remove(msg.avatar);
    }

    void RequestRemoveBuffFromPlayer(PlatformerAvatar avatar)
    {
        Debug.Log("Remove buff");
        RemoveBuffEvent msg = new RemoveBuffEvent();
        msg.Avatar = avatar;
        msg.amountToRemove = 4;
        msg.removeType = RemoveType.TRAP;
        communicationNode.Publish( msg);
    }

}
