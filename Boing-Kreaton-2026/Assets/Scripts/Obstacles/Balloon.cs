using Unity.Mathematics;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathClip;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag != "Player") return;

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
        animator.Play(deathClip.name);
        Destroy(gameObject, 0.4f);
    }
}
