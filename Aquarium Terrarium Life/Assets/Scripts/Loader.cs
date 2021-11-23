using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader: MonoBehaviour
{
    [SerializeField]
    private Animator transition;

    public void LoadNext()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(0.9f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
