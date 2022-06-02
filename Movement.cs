using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    // PARAMETERS
    // CACHE
    // STATES

    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 10f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;
    [SerializeField] ParticleSystem mainBoosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        NextLevelShortcut();
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

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            LeftThrusting();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RightThrusting();
        }
        else
        {
            StopLeftRightThrusting();
        }
    }

    void NextLevelShortcut()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainBoosterParticles.Stop();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainBoosterParticles.isPlaying)
        {
            mainBoosterParticles.Play();
        }
    }

    

    void StopLeftRightThrusting()
    {
        leftBoosterParticles.Stop();
        rightBoosterParticles.Stop();
    }

    void RightThrusting()
    {
        ApplyRotation(-rotationThrust);
        if (!leftBoosterParticles.isPlaying)
        {
            leftBoosterParticles.Play();
        }
    }

    void LeftThrusting()
    {
        ApplyRotation(rotationThrust);

        if (!rightBoosterParticles.isPlaying)
        {
            rightBoosterParticles.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
