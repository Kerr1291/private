using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int numCollisionsForNextAnim = 3;

    Animator anim = null;
    int collisionCounter = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //make sure we are colliding with an avatar, just in case
        PlatformerAvatar playerAvatar = collision.GetComponentInChildren<PlatformerAvatar>();
        if(playerAvatar==null)
        {
            return;
        }

        if (collisionCounter > numCollisionsForNextAnim)
            return;

        collisionCounter++;
        if (collisionCounter == numCollisionsForNextAnim)
        {
            anim.SetTrigger("BloodLevel2");
        }

        if (collisionCounter == 1)
        {
            anim.SetTrigger("BloodLevel1");
        }
    }
    void ResetAnimations()
    {
        collisionCounter = 0;
        anim.SetTrigger("Default");
    }
}
