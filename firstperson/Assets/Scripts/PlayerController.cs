using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float movementSpeed = 5.0f;
    public float mouseSensibility = 2.0f;
    float verticalRotation = 0;
    float verticalVelocity = 0;
    public float upDownRange = 60.0f;
    public float jumpSpeed = 7;
    public Camera cam;
    CharacterController cc;
	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        cc = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        //Rotation

        float rotLeftRight = Input.GetAxis("Mouse X")* mouseSensibility;
        transform.Rotate(0, rotLeftRight, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensibility;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        //Movement
        float fowardSpeed = Input.GetAxis("Vertical")*movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        
        if (cc.isGrounded){
            //verticalVelocity = 0;
            if (Input.GetButtonDown("Jump")){
                verticalVelocity = jumpSpeed;
            }
        }else{
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 speed = new Vector3(sideSpeed, verticalVelocity, fowardSpeed);
        speed = transform.rotation * speed;
        
        cc.Move(speed*Time.deltaTime);
        //to do: fix this, it loops between 0 and -0.3something
        //Debug.Log(verticalVelocity);
	}
}
