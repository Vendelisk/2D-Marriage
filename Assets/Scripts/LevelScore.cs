using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScore : MonoBehaviour
{
	public static LevelScore LS;

	private float timeLeft = 400f;
	private int levelScore = 0; 
	private int coinsCollected = 0;
	public Text timeUI;
	public Text scoreUI;
	public Text coinsUI;

	void Start() {
        /*if(LS != null)
            GameObject.Destroy(LS);
        else*/
            LS = this;
         
        //DontDestroyOnLoad(this);


        // Retrieves previous highScores
        DataManager.DM.LoadData();
	}
    
    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft < 0.1f) {
        	GameManager.GM.EndGame();
        }
        else {
        	timeUI.text = ("TIME\n" + (int)timeLeft);
        	scoreUI.text = ("SCORE\n" + levelScore);
        	coinsUI.text = ("x " + coinsCollected);
        }
    }

    public void addScore(int scoreToAdd) {
    	levelScore += scoreToAdd;
    }

    public void addCoin() {
    	coinsCollected += 1;
    }

    public int getScore() {
    	return levelScore;
    }

    public void addFinalScore() {
    	levelScore += (int) (timeLeft) * 10; // TODO <-- this should be shown graphically as a tally, not a dump
    	// add final height score thing
    	if(levelScore > DataManager.DM.highScore)
    		DataManager.DM.highScore = levelScore;
    }
}
