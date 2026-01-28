using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [Header("Pendulum Swing Values")]
    [SerializeField] float baseRotation;
    [SerializeField] float swingSpeed;
    [SerializeField] float rotationalValue;

    [Header("Switch")]
    [SerializeField] float minSwitchDistance;
    bool reverseSwing;

    [Header("GFX")]
    [SerializeField] Transform gfx;

    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, baseRotation - rotationalValue);
        gfx.eulerAngles -= new Vector3(0, 0, transform.eulerAngles.z + rotationalValue);
    }

    void Update()
    {
        if (!reverseSwing)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, baseRotation + rotationalValue),
                swingSpeed);
        }
        else
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
                new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, baseRotation - rotationalValue),
                swingSpeed);
        }

        if (!reverseSwing && Vector3.Distance(transform.eulerAngles, new Vector3(0, 0, baseRotation + rotationalValue)) < minSwitchDistance)
            reverseSwing = true;
        else if (reverseSwing && Vector3.Distance(transform.eulerAngles, new Vector3(0 ,0 , baseRotation - rotationalValue)) < minSwitchDistance)
            reverseSwing = false;
    }
}
