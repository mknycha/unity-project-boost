using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1f;
    [SerializeField] float rotationThrust = 1f;

    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;

    [SerializeField] WindZone windZone;


    Rigidbody rb;
    AudioSource audioSource;

    bool isThrusting = false;
    bool isRotating = false;

    bool isWithinWindZone = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WindZone"))
        {
            isWithinWindZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WindZone"))
        {
            isWithinWindZone = false;
        }
    }

    private void FixedUpdate()
    {
        if (isWithinWindZone)
        {
            windZone.ApplyForce(rb);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        if (isRotating || isThrusting)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        isThrusting = true;
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime, ForceMode.Force);
        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    private void StopThrusting()
    {
        isThrusting = false;
        mainBooster.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartRotatingLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            StartRotatingRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void StartRotatingLeft()
    {
        isRotating = true;
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
        rightBooster.Stop();
        ApplyRotation(rotationThrust);
    }

    private void StartRotatingRight()
    {
        isRotating = true;
        leftBooster.Stop();
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
        ApplyRotation(-rotationThrust);
    }

    private void StopRotating()
    {
        isRotating = false;
        rightBooster.Stop();
        leftBooster.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // Fix to the rigidbody rotation system coliding with our input system.
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
