using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    public Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;

        animator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            animator.SetFloat("SpeedH", curSpeedX);
            animator.SetFloat("SpeedV", curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
        
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }
}
