using UnityEngine;
using UnityEngine.EventSystems;

public class Spring : MonoBehaviour
{
    [SerializeField] Vector2 savedVelocity;
    [SerializeField] float minimumVelocityAddition;

    [Header("Spring Stretch")]
    [SerializeField] Transform root;

    [SerializeField] float maximumStretch;
    [SerializeField] float minimumStretch;
    float yStretch;

    [Header("Reset Failsafe")]
    [SerializeField] float resetTime;
    public float resetTimer;

    [Header("VSX Boing")]
    [SerializeField] AudioClip[] boingClip;

    [Header("Animation")]
    [SerializeField] AnimationClip boingBoing;

    Transform playerObject;
    Rigidbody2D playerRigidbody;
    AudioSource audioSource;
    Animator animator;

    bool hasSaved;
    int boingClipInt;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        boingClipInt = Random.Range(0, boingClip.Length);

        if (hasSaved && resetTimer > 0)
            resetTimer -= Time.deltaTime;
        else if (hasSaved && resetTimer < 0)
            hasSaved = false;

        if (playerObject == null) // If there is no player on the spring, then reset spring
        {
            root.localScale = Vector2.Lerp(root.localScale, new Vector2(root.localScale.x, 1), .05f);
            return;
        }

        // Compress the spring
        root.localScale = Vector2.Lerp(root.localScale, new Vector2(root.localScale.x, minimumStretch), 0.05f);

        // Add velocity to the player once it has reached far enough on the spring
        if (root.localScale.y <= minimumStretch && hasSaved)
            VelocityCalculation();

        // Limiters and the ability to forget the player
        yStretch = playerObject.position.y - root.position.y;
        yStretch = Mathf.Clamp(yStretch, minimumStretch, maximumStretch);

        root.localScale = new Vector2(root.localScale.x, yStretch);

        if (Vector2.Distance(root.position, playerObject.position) > maximumStretch)
            playerObject = null;
    }

    void VelocityCalculation()
    {
        hasSaved = false;

        animator.Play(boingBoing.name);

        if (Vector2.Dot(playerRigidbody.linearVelocity.normalized, transform.up.normalized) >= 0.5f)
        {
            // Add a smooth direction change with velocity or completely change trijectory
            if (playerRigidbody.linearVelocity.y >= 0)
            {
                playerRigidbody.linearVelocity = transform.up * savedVelocity.y;
                playerRigidbody.linearVelocity += new Vector2(savedVelocity.x, minimumVelocityAddition);
            }
            else
            {
                playerRigidbody.linearVelocity = transform.up * -savedVelocity.y;
                playerRigidbody.linearVelocity += new Vector2(savedVelocity.x, minimumVelocityAddition);
            }
        }
        else
        {
            playerRigidbody.linearVelocity = transform.up * -savedVelocity;
            playerRigidbody.linearVelocity += new Vector2(transform.up.x, transform.up.y) * minimumVelocityAddition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.PlayOneShot(boingClip[boingClipInt]);

        if (other.GetComponent<Rigidbody2D>() == null || hasSaved) return;

        resetTimer = resetTime;

        hasSaved = true;

        playerObject = other.transform;

        Rigidbody2D ballBody = other.GetComponent<Rigidbody2D>();
        savedVelocity = ballBody.linearVelocity;

        playerRigidbody = ballBody;
    }
}
