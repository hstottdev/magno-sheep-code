using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField] bool _static;
    [SerializeField] GameObject followerPrefab;
    [SerializeField] float moveForce;
    [SerializeField] bool canSwitchPolarity = true;
    [SerializeField] bool canImpulse = true;
    [SerializeField] float lossOfControlRange = 8;
    [Header("Actions")]
    [SerializeField] int impulseInterval = 100;//every x frames on average
    [SerializeField] int switchInterval = 200;

    FollowerAgent followerAgent;
    Rigidbody rb;
    MagnetController mg;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on the sphere! Please add one.");
        }

        mg = GetComponent<MagnetController>();

        if (!_static) SpawnFollower();
    }

    void SpawnFollower()
    {
        followerAgent = Instantiate(followerPrefab, transform.position, transform.rotation).GetComponent<FollowerAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(!_static) FollowAgent();

        ImpulseCheck();
    }

    void ImpulseCheck()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player1");
        if (player == null) return;
        SphereController playerInst = player.GetComponent<SphereController>();
        if (Vector3.Distance(transform.position,player.transform.position) < 5 && mg.currentCharge == playerInst.mg.currentCharge)
        {
            if (Random.Range(0, impulseInterval) < 2 && canImpulse) mg.ImpulseBlast(mg.currentCharge);
        }
        if (Random.Range(0, switchInterval) < 2 && canSwitchPolarity) mg.SwitchPolarities();
    }

    void FollowAgent()
    {
        if(transform.position.y > 0 && Mathf.Abs(transform.position.z) < lossOfControlRange && Mathf.Abs(transform.position.x) < lossOfControlRange)
        {
            transform.LookAt(followerAgent.transform);
            rb.AddForce(transform.forward * moveForce);
        }

    }
}
