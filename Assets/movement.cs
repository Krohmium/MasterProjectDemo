using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public CharacterController controller;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;

    public bool groundedPlayer;
    public float gravityValue = -0.981f;
    public float velocity = 0f;
    public Vector3 velocityVector = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        // Movement
        groundedPlayer = controller.isGrounded;
        // player movement - forward, backward, left, right
        float horizontal = Input.GetAxis("Horizontal") * playerSpeed;
        float vertical = Input.GetAxis("Vertical") * playerSpeed;
        controller.Move((transform.right * horizontal + transform.forward * vertical) * Time.deltaTime);

        //velocityVector.x +=  horizontal;
        //velocityVector.z += vertical;

        //if(velocityVector.x >0)
        //    velocityVector.x -= playerSpeed/2;
        //else
        //    velocityVector.x += playerSpeed / 2;

        //if(velocityVector.z >0)
        //    velocityVector.z -= playerSpeed/2;
        //else
        //    velocityVector.z += playerSpeed / 2;

        // Gravity
        if (groundedPlayer)
        {
            velocityVector.y = -0.1f;
        }
        else
        {
            velocityVector.y += gravityValue * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            velocityVector.y += Mathf.Sqrt(jumpHeight * -gravityValue);
            //Debug.Log("Horizontal  " + velocityVector.y);
        }

        controller.Move(velocityVector);
    }
}

