using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    
    public Vector2 moveDir;

    public float pickupRange = 1.5f;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame

    void Update()
    {
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");


        moveInput.Normalize();

        // Update moveDir based on moveInput
        moveDir = new Vector2(moveInput.x, moveInput.y);

        if (anim.GetBool("IsDeath") == false)
        {
            transform.position += moveInput * moveSpeed * Time.deltaTime;




            if (moveInput != Vector3.zero)
            {
                anim.SetBool("IsMoving", true);

                if (moveInput.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                if (moveInput.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }

        }
    }
}
