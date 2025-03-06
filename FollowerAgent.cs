using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerAgent : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] string targetTag;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("No NavmeshAgent component found. Please add one.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);
        if (target == null) return;
        agent.destination = GameObject.FindGameObjectWithTag(targetTag).transform.position;
    }
}
