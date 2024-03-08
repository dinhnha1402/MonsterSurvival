using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Animator anim;
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

        if (anim.GetBool("IsDeath") == false)
        {
            transform.position += moveInput * moveSpeed * Time.deltaTime;
        }

        
        if (moveInput != Vector3.zero)
        {
            anim.SetBool("IsMoving", true);

            if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            if (moveInput.x < 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }
}
