using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRB;
    private Animator playerAnimator;
    public float moveSpeed = 1f;
    public float jumpSpeed = 1f, jumpFrequency = 1f, nextJumpTime;

    private bool facingRight = true;
    private bool isGrounded = false;

    public Transform groundCheckPosition;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove();
        OnGroundCheck();

        if (playerRB.velocity.x < 0 && facingRight)
        {
            FlipFace();
        }
        else if (playerRB.velocity.x > 0 && !facingRight)
        {
            FlipFace();
        }
        
        // kullanıcımız yatay zeminde 0'dan büyük bir değer girmişse yani ("w" veya yukarı ok tuşu) ve zemine değmişse
        //ve bir sonraki atlama süresi oyun başladığında geçen süreden küçükse, ZIPLA!!
        if (Input.GetAxis("Vertical") > 0 && isGrounded && (nextJumpTime < Time.timeSinceLevelLoad))
        {
            nextJumpTime = Time.timeSinceLevelLoad + jumpFrequency;
            //zipla metotu
            Jump();
        }
    }

    void HorizontalMove()
    {
        playerRB.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, playerRB.velocity.y);
        playerAnimator.SetFloat("playerSpeed", Mathf.Abs(playerRB.velocity.x));
    }

    void FlipFace()
    {
        facingRight = !facingRight;
        Vector3 tempLocalScale = transform.localScale;
        tempLocalScale.x *= -1;
        transform.localScale = tempLocalScale;
    }

    void Jump()
    {
        playerRB.AddForce(new Vector2(0f,jumpSpeed));
    }

    void OnGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundCheckLayer);
        playerAnimator.SetBool("isGroundedAnim", isGrounded);
    }
}
