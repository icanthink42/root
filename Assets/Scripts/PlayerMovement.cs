using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpMult;
    [SerializeField] private float sideJumpThreshold;
    Rigidbody2D rb;
    private bool jumpLetGo = true;
    private int jumps;

    private static bool spawned = false;
    // Start is called before the first frame update
    void Start()
    {
        if (spawned)
        {
            Destroy(gameObject);
        }

        spawned = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        float speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
        if (jumps > 0 && Input.GetKey(KeyCode.Space) && jumpLetGo)
        {
            jumpLetGo = false;
            Vector3 force = Vector2.up * jumpForce * -Physics2D.gravity * Time.fixedDeltaTime * 1000;
            if (jumps == 1)
            {
                force *= doubleJumpMult;
            }
            rb.AddForce(force);
            jumps--;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpLetGo = true;
        }
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpLetGo = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Ground") && col.transform.position.y + sideJumpThreshold < transform.position.y)
        {
            jumps = 2;
        }
    }

}
