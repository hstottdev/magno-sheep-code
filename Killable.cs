using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    LayerMask deathLayer;
    [SerializeField] float destroyDelay = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        deathLayer = LayerMask.NameToLayer("Death");

        if(LevelManager.inst != null)
        {
            LevelManager.inst.killables.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == deathLayer)
        {
            Die();
        }
    }

    void Die()
    {
        LevelManager.inst.killables.Remove(this);
        Destroy(gameObject, destroyDelay);


        if (LevelManager.inst.killables.Count < 2)
        {  
            LevelManager.WinnerCheck();
        }
        else if (gameObject.CompareTag("Player1"))
        {
            LevelManager.inst.SingleplayerLoss();
        }
    }
}
