using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Velocity")]
    [SerializeField] float moveSpeed;
    [SerializeField] float minMaterialBounce;
    [SerializeField] float maxFallVelocity;
    public float glueVelocity;
    public bool glued;

    [Header("Checks")]
    [SerializeField] Transform starterPos;
    [SerializeField] Vector2 groundCheckPosition;
    [SerializeField] Vector2 groundCheckSize;

    [Header("Camera Shake")]
    [SerializeField] float jumpShakeDuration;
    float shakeMagnitude;

    [Header("Respawn Timer")]
    [SerializeField] float respawnTime;
    [SerializeField] GameObject corpsePartsVFX;
    [SerializeField] GameObject dustParticles;
    bool dead;

    [Header("Death SFX")]
    [SerializeField] AudioClip[] deathClip;
    [SerializeField] AudioClip ashClip;

    SaveManager saveManager;
    Rigidbody2D myRigidbody;
    InputAction moveAction;
    PlayerCamera playerCamera;
    AudioSource audioSource;
    Animator anim;

    int deathClipInt, boingClipInt;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        saveManager = FindFirstObjectByType<SaveManager>();
        playerCamera = FindFirstObjectByType<PlayerCamera>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        deathClipInt = Random.Range(0, deathClip.Length);

        if (dead) return;
        CheckGround();
        playerMove();
        //HandleFlip();
    }

    void playerMove()
    {
        if (moveAction.ReadValue<Vector2>().x != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }

        if (CheckGround() && !glued)
        {
            Vector2 moveVector = moveAction.ReadValue<Vector2>();
            myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x + moveVector.x * moveSpeed, myRigidbody.linearVelocity.y);

            myRigidbody.linearVelocity = new Vector2(Mathf.Clamp(myRigidbody.linearVelocity.x, -moveSpeed, moveSpeed), myRigidbody.linearVelocity.y);
        }
        else if (CheckGround() && glued)
        {
            Vector2 moveVector = moveAction.ReadValue<Vector2>();
            myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x + moveVector.x * glueVelocity, myRigidbody.linearVelocity.y);

            myRigidbody.linearVelocity = new Vector2(Mathf.Clamp(myRigidbody.linearVelocity.x, -moveSpeed, moveSpeed), myRigidbody.linearVelocity.y);
        }
        else { return; }
    }

    bool CheckGround()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(transform.position + (Vector3)groundCheckPosition, groundCheckSize, 0);

        if (myRigidbody.linearVelocity.y < 0)
        {
            shakeMagnitude = myRigidbody.linearVelocity.y * -0.025f;
            playerCamera.ShakeTrigger(jumpShakeDuration, shakeMagnitude);
        }

        return isGrounded;
    }

    void HandleFlip()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();

        if (moveVector.x < -0.1)
        {
            // Turn left
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else if (moveVector.x > 0.1)
        {
            // Turn right
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    void OnDeath()
    {
        dead = true;
        myRigidbody.linearVelocity = Vector2.zero;

        if (saveManager.currentSavePoint == null && starterPos != null) { StartCoroutine(DeathRespawn(starterPos.position)); }
        else if (saveManager.currentSavePoint != null) { StartCoroutine(DeathRespawn(saveManager.currentSavePoint.transform.position)); }
        else
        {
            Debug.Log("No starting position!");
            return;
        }
    }

    void PlaySound()
    {
        audioSource.PlayOneShot(deathClip[deathClipInt]);
        audioSource.PlayOneShot(ashClip);
    }

    IEnumerator DeathRespawn(Vector2 respawnPosition)
    {
        PlaySound();

        Instantiate(dustParticles, transform.position, Quaternion.identity);
        GameObject corpseObject = Instantiate(corpsePartsVFX, transform.position, Quaternion.identity);
        playerCamera.playerObject = corpseObject.transform;
        transform.position = respawnPosition;

        yield return new WaitForSeconds(respawnTime);

        playerCamera.playerObject = transform;
        dead = false;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other = other.gameObject.GetComponent<Collider2D>();

        if (other.sharedMaterial == null) { Debug.Log("YOU FORGOT A PHYSICS MATERIAL!!!!"); return; }

        PhysicsMaterial2D otherMaterial = other.sharedMaterial;

        //Debug.Log("other Material: " + otherMaterial.bounciness);

        //Debug.Log("Speed: " + Mathf.Abs(myRigidbody.linearVelocity.y));

        if (otherMaterial.bounciness <= minMaterialBounce && (Mathf.Abs(myRigidbody.linearVelocity.y)) >= maxFallVelocity)
        {
            OnDeath();
        }
        else
        {
            Debug.Log("Survived");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)groundCheckPosition, groundCheckSize);
    }
}