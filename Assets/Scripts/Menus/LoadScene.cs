using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] Animator transition = null;
    [SerializeField] float transitionDuration = 1f;

    public void LoadNextScene(int scene)
    {
        StartCoroutine(WaitThenLoad(scene, transitionDuration));
    }

    private IEnumerator WaitThenLoad(int scene, float seconds)
    {
        if (transition != null)
            transition.SetTrigger("transition");

        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(scene);
    }
}
