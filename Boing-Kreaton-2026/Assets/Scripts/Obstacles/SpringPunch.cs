using UnityEngine;
using UnityEngine.Audio;

public class SpringPunch : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] float desiredRange;
    Vector2 desiredPos;
    Vector2 savedPos;

    [Header("Velocity")]
    [SerializeField] float punchVelocity;
    [SerializeField] float launchVelocity;
    [SerializeField] float retractVelocity;

    [Header("Timers")]
    [SerializeField] float launchTime;
    float launchTimer;

    [Header("VFX")]
    [SerializeField] AudioClip clip;
    AudioSource audioSource;

    bool launched;
    bool retracting;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        savedPos = transform.position;

        desiredPos = new Vector2(savedPos.x, savedPos.y);
        Vector2 rangeInVector = transform.up * desiredRange;
        desiredPos += rangeInVector;

        launchTimer = launchTime;
    }
    void Update()
    {
        if (launchTimer > 0 && !launched && !retracting)
            launchTimer -= Time.deltaTime;
        else if (launchTimer <= 0 && !launched && !retracting)
            launched = true;

        if (launched)
            LaunchMechanics();

        if (retracting)
            RetractMechanics();
    }

    void LaunchMechanics()
    {
        transform.position = Vector2.Lerp(transform.position, desiredPos, launchVelocity);

        Vector2 currentPos = transform.position;
        if (currentPos == desiredPos)
        {
            launched = false;
            retracting = true;
        }
    }

    void RetractMechanics()
    {
        transform.position = Vector2.Lerp(transform.position, savedPos, retractVelocity);

        Vector2 currentPos = transform.position;
        if (Vector2.Distance(currentPos, savedPos) < 0.1f)
        {
            transform.position = savedPos;
            retracting = false;
            launchTimer = launchTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (launched && other.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            audioSource.PlayOneShot(clip);
            Rigidbody2D ballBody = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 savedDirection = (desiredPos - savedPos).normalized;

            VelocityCalculation(ballBody, savedDirection);
        }
    }

    void VelocityCalculation(Rigidbody2D ballBody, Vector2 gloveDirection)
    {
        if (Vector2.Dot(ballBody.linearVelocity.normalized, gloveDirection) >= 0)
        {
            ballBody.linearVelocity = ballBody.linearVelocity + gloveDirection * punchVelocity;
        }
        else
        {
            ballBody.linearVelocity = new Vector2(gloveDirection.x * punchVelocity, ballBody.linearVelocity.y + gloveDirection.y * punchVelocity);
        }
    }
}
