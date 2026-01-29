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
    public void StartLoad(string name)
    {
        StartCoroutine(LoadScene(name));
    }
    public IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);
        yield return null;
    }
}
