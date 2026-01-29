using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void FQuit()
    {
        Invoke("Quit", 2f);
    }

    private void Quit()
    {
        Quit();
    }
    public void StartLoad(int scene)
    {
        StartCoroutine(LoadScene(scene));
    }
    public IEnumerator LoadScene(int scene)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scene);
        yield return null;
    }
}
