using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] ScreenFade fader;
    
    void Start()
    {
        fader = GetComponentInChildren<ScreenFade>();
    }

    void Update()
    {

    }

    public IEnumerator LoadScene(string sceneName)
    {
        StartCoroutine(fader.FadeOutCoroutine(1));
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
        yield return null;
    }
}
