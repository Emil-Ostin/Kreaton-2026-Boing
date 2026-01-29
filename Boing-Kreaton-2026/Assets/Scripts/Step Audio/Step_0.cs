using UnityEngine;

public class Step_0 : MonoBehaviour
{
    [SerializeField] AudioClip stepClip;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        audioSource.PlayOneShot(stepClip);
    }
}
