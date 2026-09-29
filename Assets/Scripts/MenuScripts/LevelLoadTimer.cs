using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadTimer : MonoBehaviour
{
    public void Begin(float delay)
    {
        DontDestroyOnLoad(gameObject);
        Invoke(nameof(LoadNextLevel), delay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
    }
}