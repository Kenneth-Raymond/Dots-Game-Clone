using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DotGameMechs : MonoBehaviour
{
    private int scoreKeeper;
    private float timer;
    private Canvas activeCanvas;

    private void Start()
    {
        scoreKeeper = 0;
        timer = 30.0f;
        activeCanvas = GameObject.FindObjectOfType<Canvas>();
    }
    private void Update()
    {
        activeCanvas.transform.Find("Timer").GetComponent<UnityEngine.UI.Text>().text = "Time Remaining: " + Mathf.RoundToInt(timer);
        if (timer >= 0.0f)
        {
          timer -= Time.deltaTime;
        }
        if(timer <= 0.0f)
        {
            EndGame();
        }
    }
    private void EndGame()
    {
        GameObject levelManager = GameObject.Find("Level Manager");
        levelManager.GetComponent<LevelLoader>().LoadSpecificLevel("EndGame");
    }
    public void RemoveDots(List<GameObject> listOfDots)
    {
        foreach (GameObject dot in listOfDots)
        {
            GameObject.Destroy(dot);
            scoreKeeper++;
        }
        activeCanvas.transform.Find("Score").GetComponent<UnityEngine.UI.Text>().text = "Score: " + scoreKeeper;
    }
}
