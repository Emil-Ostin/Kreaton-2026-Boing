using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] float followSpeed;
    [SerializeField] float damping;

    [Header("Offset")]
    [SerializeField] float distanceThreshold;
    public Vector3 offset;

    [Header("Follower")]
    public Transform playerObject;

    [Header("Screen Shake")]
    [SerializeField] float shakeDuration = 0.1f;
    [SerializeField] float shakeMagnitude = 0.1f;
    [SerializeField] AnimationCurve shakeFalloff;

    Vector3 velocity = Vector3.zero;
    Vector3 shakeOffset = Vector3.zero;
    float shakeTimeRemaining;

    void FixedUpdate()
    {
        Vector3 targetPosition = playerObject.position + offset;
        targetPosition.z = transform.position.z;

        // Slow readjustment to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping); // Gradual Transition

        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            // The shake intensity with an animation curve (Evaluate is the time in the curve)
            float shakeFactor = shakeFalloff.Evaluate(1f - (shakeTimeRemaining / shakeDuration)); 
            shakeOffset = Random.insideUnitCircle * shakeMagnitude * shakeFactor; // A randomized jitter effect
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        // shake offset
        transform.position += shakeOffset;
    }

    public void ShakeTrigger(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = duration;
    }
}
