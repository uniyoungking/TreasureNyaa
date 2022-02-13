using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController player;

    private Rigidbody2D rigid2D = null;
    private Animator animator = null;

    private bool canMove = true;
    private bool isSuperJump = false;
    private bool isSideJumping = false;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
        


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                Initialize_X_Velocity();
            }
        }

#if UNITY_EDITOR
        float move = Input.GetAxis("Horizontal") * player.speed * Time.deltaTime;
        transform.Translate(new Vector3(move, 0, 0));

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(0) == false)
            {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    if (Camera.main.ScreenToWorldPoint(touch.position).x < gameObject.transform.position.x)
                    {
                        Initialize_X_Velocity();
                        transform.Translate(new Vector3(-player.speed * Time.deltaTime, 0, 0));
                        isMovingLeft = true;
                        isMovingRight = false;
                    }
                    else
                    {
                        Initialize_X_Velocity();
                        transform.Translate(new Vector3(player.speed * Time.deltaTime, 0, 0));
                        isMovingRight = true;
                        isMovingLeft = false;
                    }
                }
            }
        }
        else
        {
            isMovingLeft = false;
            isMovingRight = false;
        }
            
#endif
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8f, 8f), transform.position.y);

        if (Input.GetAxis("Horizontal") < -0.7f || Input.GetAxis("Horizontal") > 0.7f || isMovingLeft || isMovingRight)
        {
            if (!isSideJumping)
                SideJump();
        }

        if (isSideJumping)
        {
            if (Input.GetAxis("Horizontal") > 0f || isMovingRight)
                player.GetPlayerSprite().flipX = true;
            if (Input.GetAxis("Horizontal") < 0f || isMovingLeft)
                player.GetPlayerSprite().flipX = false;
        }
    }


    public void Initialize_X_Velocity()
    {
        rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
    }


    public void Jump(float jumpForce = 2.5f)
    {
        isSideJumping = false;

        if (!isSuperJump)
        {
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, player.jumpPower * jumpForce);

            if (Input.GetAxis("Horizontal") < -0.7f || Input.GetAxis("Horizontal") > 0.7f)
            {
                if (!isSideJumping)
                    SideJump();
            }
            else if (isMovingLeft || isMovingRight)
            {
                if (!isSideJumping)
                    SideJump();
            }
            else
                animator.SetTrigger("Jump");
        }
        else
        {
            isSuperJump = false;
            Jump(5f);
        }
    }

    private void SideJump()
    {
        isSideJumping = true;

        animator.SetTrigger("Side_Jump");
    }

    public void AddForceRandom_X(float xPower)
    {
        int random = Random.Range(0, 2);

        if (random == 1)
        {
            rigid2D.AddForce(new Vector2(xPower, 0f), ForceMode2D.Impulse);
        }
        else
        {
            rigid2D.AddForce(new Vector2(-xPower, 0f), ForceMode2D.Impulse);
        }
    }

    public void EnableCanMove()
    {
        canMove = true;
    }

    public void DisableCanMove()
    {
        canMove = false;
    }
}
