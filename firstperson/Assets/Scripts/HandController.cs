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
    public float GrabDistance = 0.1f;
    public string GrabTag = "Grab";
    public string GrabInput = "Grab";
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
        _isGrabbing = false;
        _lastFramePosition = transform.position;

        _currentGrabObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log(rb.position);
        
        float fowardSpeed = Input.GetAxis("VerticalH") * movementSpeed;
        float sideSpeed = Input.GetAxis("HorizontalH") * movementSpeed;
        rb.transform.position += new Vector3(sideSpeed,0, fowardSpeed);
        */
        if (_currentGrabObject == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, GrabDistance);
            if (colliders.Length > 0)
            {
                if ( colliders[0].tag.Equals(GrabTag))
                {
                    Debug.Log(colliders[0].tag);
                }
                else
                {
                    Debug.Log("NO");

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
