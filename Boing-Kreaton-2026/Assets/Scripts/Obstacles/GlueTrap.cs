using UnityEngine;

public class GlueTrap : MonoBehaviour
{
    [SerializeField] float minimumVelocityThreshhold;
    [SerializeField] float lerpTime;

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Rigidbody2D>() == null) return;

        Rigidbody2D ballBody = other.gameObject.GetComponent<Rigidbody2D>();

        if (ballBody.linearVelocity.magnitude > minimumVelocityThreshhold)
        {
            Vector2 resultVelocity = new Vector2(minimumVelocityThreshhold, 0);
            ballBody.linearVelocity = Vector2.Lerp(ballBody.linearVelocity, resultVelocity, lerpTime);
        }
        else
        {
            ballBody.linearVelocity = new Vector2(ballBody.linearVelocity.x, ballBody.linearVelocity.y);
        }
    }
}
