using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gameMode
{
    solo,
    local,
}
public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public static gameMode mode;
    public AudioClip menuMusic;
    public AudioClip battleMusic;
    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            inst = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
