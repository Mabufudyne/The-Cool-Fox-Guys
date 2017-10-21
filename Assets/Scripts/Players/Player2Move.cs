﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Move : MonoBehaviour
{

    public float acceleration = 1f;
    public float maxSpeed = 5f;

    public float jumpForce = 800f;
    [HideInInspector] public bool jump = false;
    public Transform Player2GroundCheck;

    private bool grounded = false;
    private int groundmask;
    Animator playerAnim;
    SpriteRenderer mySprite;

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        groundmask = 1 << LayerMask.NameToLayer("Ground");
        playerAnim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, Player2GroundCheck.position, groundmask);

        if (Input.GetButtonDown("Player2Jump") && grounded)
        {
            jump = true;
        }
    }

    // Update physics
    void FixedUpdate()
    {

        if (grounded)
        {
            rb2d.gravityScale = 0;
        }
        else
        {
            rb2d.gravityScale = 4;
        }


        float horizontalInput = Input.GetAxisRaw("Player2Horizontal");
        if (horizontalInput > 0)
            mySprite.flipX = false;
        else if (horizontalInput < 0)
            mySprite.flipX = true;
        playerAnim.SetFloat("Walking", horizontalInput);

        if (horizontalInput != 0 && Mathf.Abs(rb2d.velocity.x) < maxSpeed)
        {
            if (horizontalInput * acceleration >= maxSpeed)
            {
                rb2d.velocity = new Vector2(Mathf.Sign(horizontalInput) * maxSpeed, rb2d.velocity.y);
            }
            else
            {
                rb2d.AddForce(new Vector2(horizontalInput * acceleration, 0), ForceMode2D.Impulse);
            }
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
        }

        if (jump)
        {
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }
}
