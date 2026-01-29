using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float startPos, length;
    [SerializeField] GameObject cam;
    [SerializeField] float parallaxEffect;


    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;


        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
