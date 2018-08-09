using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject gameOverPanel;
    public GameObject winPanel;

    bool gameOver = false;
    bool wonGame = false;

	// Use this for initialization
	void Start () {
        instance = this;
        Time.timeScale = 1f;
        RenderSettings.fog = true;
        Debug.Log("Turned fog on from this script");
	}

    public void WonGame() {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
        wonGame = true;
    }

    public void GameOver() {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        gameOver = true;
    }

    void LoadNewGame() {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOver = false;
        wonGame = false;
        SceneManager.LoadScene("Main");
    }
	
	// Update is called once per frame
	void Update () {
        if ((gameOver || wonGame) && Input.GetMouseButtonDown(0)) {
            LoadNewGame();
        }
	}
}
