using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public MagnetController mg;
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float maxSpeed = 5f;
    [SerializeField] Transform nearestTile;
    Vector3 targetPosition = new Vector3();


    [Header("Inputs")]
    public string horizontalInputAxis = "Horizontal";
    public string verticalInputAxis = "Vertical";
    public string switchPolaritiesKey;
    public string impulseKey;
    float moveX;
    float moveZ;


    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on the sphere! Please add one.");
        }
    }

    private void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        // Get player input
        moveX = Input.GetAxisRaw(horizontalInputAxis);
        moveZ = Input.GetAxisRaw(verticalInputAxis);

        if (Input.GetKeyDown(switchPolaritiesKey))
        {
            mg.SwitchPolarities();
        }

        if(Input.GetKeyDown(impulseKey))
        {
            mg.ImpulseBlast(mg.currentCharge);
        }
    }


    private void FixedUpdate()
    {
        MovementForce();
    }

    void MovementForce()
    {
        // Calculate the direction to apply force
        Vector3 forceDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (forceDirection != Vector3.zero)
        {
            // Apply force to move the sphere
            Vector3 force = forceDirection * moveSpeed;
            rb.AddForce(force, ForceMode.Force);

            // Limit the sphere's speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }


    void LerpToPosition()
    {
        if (targetPosition == null) return;

        if(Vector3.Distance(transform.position,targetPosition) > 0.2f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        }

    }
}
