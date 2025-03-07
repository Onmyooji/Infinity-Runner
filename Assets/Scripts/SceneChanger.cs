using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;
    private string nextScene;
    public void FadeToScene (string sceneName)
    {
        nextScene = sceneName;
        animator.SetTrigger("FadeOut");
    }
    public void FadeComplete()
    {
        SceneManager.LoadScene(nextScene);
    }
}
