using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager inst;
    public List<Killable> killables = new List<Killable>();
    public Killable winner;
    public GameObject playerWinScreen;
    public GameObject playerLoseScreen;
    [HideInInspector] public bool gameEnded;

    [SerializeField] GameObject enemiesParent;
    [SerializeField] GameObject player2;
    [SerializeField] bool player2Test;
    [SerializeField] GameObject player1Icon;
    [SerializeField] GameObject player2Icon;
    [SerializeField] GameObject tutorialScreen;
    // Start is called before the first frame update
    void Awake()
    {
        inst = this;
        killables = new List<Killable>();

        if (player2Test) GameManager.mode = gameMode.local;

        switch (GameManager.mode)
        {
            case gameMode.solo:
                player2.SetActive(false);
                enemiesParent.SetActive(true);
                break;
            case gameMode.local:
                player2.SetActive(true);
                enemiesParent.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void WinnerCheck()
    {
        if(inst.killables.Count < 2)
        {
            inst.DeclareWinner(inst.killables[0]);
        }
    }

    public void DeclareWinner(Killable winner)
    {
        inst.winner = winner;

        if(winner.CompareTag("Player1") || winner.CompareTag("Player2"))
        {
            PlayerWin();
        }
        else
        {
            SingleplayerLoss();
        }
    }

    public void SingleplayerLoss()
    {
        if (gameEnded) return;

        playerLoseScreen.SetActive(true);
        gameEnded = true;

        if (tutorialScreen == null) return;
        tutorialScreen.SetActive(false);
    }

    public void PlayerWin()
    {
        if (gameEnded) return;

        playerWinScreen.SetActive(true);
        gameEnded = true;

        if (winner.CompareTag("Player1")) player1Icon.SetActive(true);
        if (winner.CompareTag("Player2")) player2Icon.SetActive(true);

        if (tutorialScreen == null) return;
        tutorialScreen.SetActive(false);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void BackToLevelSelect()
    {
        MenuManager.queueLevelSelect = true;
        SceneManager.LoadScene(0);
    }

    public void TutorialNext()
    {
        Debug.Log("next scene name " + SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name);
        if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name == "Level 1")
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
