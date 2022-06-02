using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float timeDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip reachGoal;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    ParticleSystem particleSystem;

    bool isTransitioning = false;
    bool isCollisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        DisableCollisions();
    }

    void DisableCollisions()
    {
        if (Input.GetKey(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || isCollisionDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This is a friendly object.");
                break;
            case "Finish":
                ReachTheGoal();
                break;
            default:
                StartCrashSequence();
                break;
        }

        void StartCrashSequence()
        {
            isTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(crash);
            crashParticles.Play();
            GetComponent<Movement>().enabled = false;
            Invoke("ReloadLevel", timeDelay);
        }

        void ReachTheGoal()
        {
            isTransitioning = true;
            audioSource.Stop();
            audioSource.PlayOneShot(reachGoal);
            successParticles.Play();
            GetComponent<Movement>().enabled = false;
            Invoke("NextLevel", timeDelay);
        }
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}