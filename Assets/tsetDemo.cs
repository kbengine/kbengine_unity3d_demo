using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tsetDemo : MonoBehaviour
{
    public float speed = 6.0F;

    public float jumpSpeed = 8.0F;

    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    CharacterController moveObj;
    // Start is called before the first frame update
    void Start()
    {
        moveObj = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}




