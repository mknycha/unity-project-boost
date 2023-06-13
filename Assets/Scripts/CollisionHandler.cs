using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip finish;

    bool isLocked = false;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (isLocked)
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
        audioSource.PlayOneShot(finish);
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
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        // TODO: Add particle effects
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
