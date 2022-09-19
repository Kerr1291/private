using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nv;
using GameEvents;
using ComponentBuff;
public class Collectable : MonoBehaviour
{
    public float activationTimer = 0.5f;
    public Collider2D playerCollider = null;
    public float spanwVelocity = 15f;
    public AudioSource pickupSound;

    float timer = 0.0f;
    bool applyForce = false;
    Vector2 forceToApply = new Vector2();
    Rigidbody2D rigidBody = null;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        timer = activationTimer;
        playerCollider.enabled = false;
    }

    private void Update()
    {
        if (timer <= 0.0f)
            return;

        timer -= Time.deltaTime;
        if(timer<=0.0f)
        {
            playerCollider.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if(applyForce)
        {
            rigidBody.AddForce(forceToApply, ForceMode2D.Impulse);
            applyForce = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //make sure we collided with a player
        PlatformerAvatar playerAvatar = collision.GetComponentInChildren<PlatformerAvatar>();

        if (playerAvatar == null)
            return;

        AddBuffEvent addBuff = new AddBuffEvent();
        addBuff.Avatar = playerAvatar;
        addBuff.buff = new MovementSpeedBuff();

        CommunicationNode.publish(addBuff,this);
        if( pickupSound != null )
            pickupSound.Play();
        gameObject.SetActive(false);
    }

    //angle should come in radians
    public void ApplyForce(float v,float angle)
    {
        //calculate force to apply
        forceToApply = new Vector2(Mathf.Cos(angle)*v, Mathf.Sin(angle) * v);
        applyForce = true;
    }
}
