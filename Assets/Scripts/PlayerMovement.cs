using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    float gravityAtStart;
    
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float gravityClimb = 0f;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }
    void ClimbLadder()
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myRigidbody.gravityScale = gravityAtStart;
            return;
        }
        Vector2 climbVelocity = new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        myRigidbody.linearVelocity = climbVelocity;
        myRigidbody.gravityScale = gravityClimb;
        
    }
}
