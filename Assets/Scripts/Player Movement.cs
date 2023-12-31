using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Cache references
    Rigidbody rb;
    AudioSource audioSource;
    
    // Parameters
    [SerializeField] private float thrustSpeed = 1000f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float rotInputX = 0.6f;
    [SerializeField] private float tiltSpeed = 100f;
    [SerializeField] AudioClip engineThrust;
    [SerializeField] private ParticleSystem leftSmokeTrail;
    [SerializeField] private ParticleSystem rightSmokeTrail;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // gets the rigidbody component
        audioSource = GetComponent<AudioSource>();
         // gets the audiosource component
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }



    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            startThrusting();

        }
        
        else {
            audioSource.Stop();
            
        }
        
    }

    void ProcessRotation() 
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotateSpeed, rotInputX); 
        }

        else if (Input.GetKey(KeyCode.D)) { // else if = otherwise, meaning if the if before isnt pressed
            
            ApplyRotation(-rotateSpeed, rotInputX); // the parameter rotThisFrame allows a negative input as well
        }

        if (Input.GetKey(KeyCode.W)) {
            ApplyTilt(tiltSpeed);
        }
        else if (Input.GetKey(KeyCode.S)) {
             ApplyTilt(-tiltSpeed);
        }
    }

    private void startThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime);
        rb.AddRelativeForce(Vector3.right * thrustSpeed/5f * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineThrust);


        }
        if (!leftSmokeTrail.isPlaying) leftSmokeTrail.Play();
        if (!rightSmokeTrail.isPlaying) rightSmokeTrail.Play();
    }

    

    private void ApplyRotation(float rotThisFrame, float rotInputFrame) // uses a parameter as a variable for the rotation speed
    {
        //rb.freezeRotation = true; //freezing rotation so we can manually rotate
        //transform.Rotate(Vector3.forward * rotThisFrame * Time.deltaTime);
        // rb.freezeRotation = false; // unfreezing rotation so the physics system can take over

        //rb.AddTorque(Vector3.forward * rotThisFrame * Time.deltaTime * rotInputFrame);
        // This is another possible solution, where the constraints aren't cancelled like in the above
        
        Quaternion deltaRot = Quaternion.Euler(Vector3.right * rotThisFrame * Time.deltaTime * rotInputFrame);
        rb.MoveRotation(rb.rotation * deltaRot); //tilts the player on the z-axis
    }

    void ApplyTilt(float tiltThisFrame) {
        //Allows the player to tilts forward and backward on the y axis
        Quaternion deltaTilt = Quaternion.Euler(Vector3.forward * tiltThisFrame * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaTilt);
    }
}
