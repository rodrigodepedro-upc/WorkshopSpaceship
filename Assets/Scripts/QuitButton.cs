using UnityEngine;

/// <summary>
/// Quits the game. In a build it calls Application.Quit; in the editor it stops
/// play mode. Hook <see cref="Quit"/> into a UI Button's OnClick.
/// </summary>
public class QuitButton : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
