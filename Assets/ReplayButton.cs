using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reloads the active scene in Single mode. Hook <see cref="Replay"/> into a
/// UI Button's OnClick.
/// </summary>
public class ReplayButton : MonoBehaviour
{
    public void Replay()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex, LoadSceneMode.Single);
    }
}
