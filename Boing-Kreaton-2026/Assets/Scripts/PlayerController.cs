using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float minMaterialBounce;
    [SerializeField] float maxFallVelocity;
    [SerializeField] Vector2 groundCheckPosition;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Vector2 respawnPos;

    Rigidbody2D myRigidbody;
    InputAction moveAction;

    //TEST!!!!
    //InputAction interactAction;
    //TEST!!!!

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Move");

        //TEST!!!!
        //interactAction = InputSystem.actions.FindAction("Interact");
        //TEST!!!!
    }

    private void FixedUpdate()
    {
        CheckGround();
        playerMove();


        //TEST!!!!
        //if (interactAction.IsPressed())
        //{
        //    gameObject.transform.position = respawnPos;
        //}
        //TEST!!!!
    }

    void playerMove()
    {
        if (CheckGround())
        {
            Vector2 moveVector = moveAction.ReadValue<Vector2>();
            myRigidbody.linearVelocity = new Vector2(moveVector.x * moveSpeed, myRigidbody.linearVelocity.y);
        }
        else { return; }
    }

    bool CheckGround()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(transform.position + (Vector3)groundCheckPosition, groundCheckSize, 0);

        return isGrounded;
    }

    bool OnDeath()
    {
        bool isDead = true;

        if (isDead) { gameObject.transform.position = respawnPos; }

        return isDead;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other = other.gameObject.GetComponent<Collider2D>();

        if(other.sharedMaterial == null) { Debug.Log("YOU FORGOT A PHYSICS MATERIAL!!!!"); return; }

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


        //TEST!!!!
        //Gizmos.DrawWireSphere(respawnPos, 1f);
        //TEST!!!!
    }
}
