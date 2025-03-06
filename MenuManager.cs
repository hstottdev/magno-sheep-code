using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager inst;
    public GameObject homeMenu;
    public GameObject levelSelect;
    [SerializeField] GameObject pvpControlPrompts;
    public static bool queueLevelSelect;

    // Start is called before the first frame update
    void Awake()
    {
        inst = this;


    }

    private void Start()
    {
        AudioManager.SetMusic(GameManager.inst.menuMusic);
        if (queueLevelSelect)
        {
            queueLevelSelect = false;
            LevelSelection();
        }
        else
        {
            HomeMenu();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tutorial()
    {
        GameManager.mode = gameMode.solo;
        AudioManager.SetMusic(GameManager.inst.menuMusic);
        SceneManager.LoadScene("Tutorial");
    }

    public void Solo()
    {
        GameManager.mode = gameMode.solo;
        LevelSelection();
    }

    public void Local()
    {
        GameManager.mode = gameMode.local;
        LevelSelection();
        
    }

    public void HomeMenu()
    {
        homeMenu.SetActive(true);
        levelSelect.SetActive(false);
    }

    public void LevelSelection()
    {
        levelSelect.SetActive(true);
        homeMenu.SetActive(false);

        pvpControlPrompts.SetActive(GameManager.mode == gameMode.local);
    }

    public void SelectStage(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        AudioManager.SetMusic(GameManager.inst.battleMusic);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
