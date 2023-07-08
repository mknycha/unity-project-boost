using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] float cameraShakeIntensity = 4f;
    [SerializeField] float cameraShakeTime = 1f;

    bool isLocked = false;
    bool collisionsDisabled = false;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleDebugKeys();
    }

    private void HandleDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            string message;
            if (collisionsDisabled)
            {
                message = "Enabling colissions";
            }
            else
            {
                message = "Disabling colissions";
            }
            Debug.Log(message);
            collisionsDisabled = !collisionsDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isLocked || collisionsDisabled)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                Debug.Log("Finish");
                StartFinishSequence();
                break;
            default:
                Debug.Log("Hit something");
                StartCrashSequence();
                break;
        }
    }

    private void StartFinishSequence()
    {
        isLocked = true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isLocked = true;
        CinemachineShake.Instance.ShakeCamera(cameraShakeIntensity, cameraShakeTime);
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        Movement movement = GetComponent<Movement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
        Invoke("ReloadLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = CurrentSceneIndex() + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = CurrentSceneIndex();
        SceneManager.LoadScene(currentSceneIndex);
    }

    private static int CurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
