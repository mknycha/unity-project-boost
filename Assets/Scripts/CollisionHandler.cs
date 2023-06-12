using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                Debug.Log("Finish");
                LoadNextLevel();
                break;
            default:
                Debug.Log("Hit something");
                ReloadLevel();
                break;
        }
    }

    private static void LoadNextLevel()
    {
        int nextSceneIndex = CurrentSceneIndex() + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private static void ReloadLevel()
    {
        int currentSceneIndex = CurrentSceneIndex();
        SceneManager.LoadScene(currentSceneIndex);
    }

    private static int CurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
