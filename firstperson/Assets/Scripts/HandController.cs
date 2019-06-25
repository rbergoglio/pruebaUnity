using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //source: https://github.com/ololralph/vrsandboxgame/blob/master/Assets/Scripts/HandGrabbing.cs
    private Rigidbody rb;
    public float movementSpeed = 5f;
    private Transform _currentGrabObject;
    private Rigidbody _currentSpinObject;
    public float GrabDistance = 0.1f;
    private string GrabTag = "Grab";
    private string SpinTag = "Spin";
    private string GrabInput = "Grab";
    private bool _isGrabbing;
    public float ThrowMultiplier = 1.5f;
    private Vector3 _lastFramePosition;



    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, GrabDistance);
    }

    public Transform CurrentGrabObject
    {
        get { return _currentGrabObject; }
        set { _currentGrabObject = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(300,0,0);
        _isGrabbing = false;
        _lastFramePosition = transform.position;
        
        _currentGrabObject = null;
    }

    void FixedUpdate()

    {
        /*
        float fowardSpeed = Input.GetAxis("VerticalH");
        float sideSpeed = Input.GetAxis("HorizontalH");

        Vector3 tempVect = new Vector3(sideSpeed, fowardSpeed,0);
        Debug.Log(tempVect.normalized * movementSpeed);
        tempVect = tempVect.normalized * movementSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);
        */
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        
       
        if (_currentGrabObject == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, GrabDistance);
            if (colliders.Length > 0)
            {
                if (Input.GetAxis(GrabInput) >= 0.01f && colliders[0].transform.CompareTag(SpinTag))

                {
                    Debug.Log("ssssssss");
                    _currentSpinObject = colliders[0].attachedRigidbody;
                    _currentSpinObject.MoveRotation(_currentSpinObject.rotation *Quaternion.Euler(0,30,0));
               
                }

                if (Input.GetAxis(GrabInput) >= 0.01f && colliders[0].transform.CompareTag(GrabTag))
                {
                    if (_isGrabbing)
                    {
                        return;
                    }
                    _isGrabbing = true;

                    colliders[0].transform.SetParent(transform);
                    if (colliders[0].GetComponent<Rigidbody>() == null)
                    {
                        colliders[0].gameObject.AddComponent<Rigidbody>();
                    }
                    colliders[0].GetComponent<Rigidbody>().isKinematic = true;
                    _currentGrabObject = colliders[0].transform;

                }
            }

        }
        else
        {
            
            if (Input.GetAxis(GrabInput) < 0.01f)
            {
                Rigidbody _objectRGB = _currentGrabObject.GetComponent<Rigidbody>();
                _objectRGB.isKinematic = false;
                _objectRGB.collisionDetectionMode = CollisionDetectionMode.Continuous;

                Vector3 CurrentVelocity = (transform.position - _lastFramePosition) / Time.deltaTime;

                _objectRGB.velocity = CurrentVelocity * ThrowMultiplier;

                _currentGrabObject.SetParent(null);

                _currentGrabObject = null;
               
            }
            
        }
        if (Input.GetAxis(GrabInput) < 0.01f && _isGrabbing)
        {
            _isGrabbing = false;
        }
        _lastFramePosition = transform.position;

    }
}
