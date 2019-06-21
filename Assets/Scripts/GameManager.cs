using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;

	bool gameHasEnded = false;
	private float menuDelay = .5f;
	private float restartDelay = 2f;
	public GameObject endMsg;
	public GameObject pauseMsg;
	public static int characterIndex = 0;
	private GameObject[] characters;
	public CinemachineVirtualCamera vcam;

    void Start () {
        /*if(GM != null)
            GameObject.Destroy(GM);
        else*/
            GM = this;
         
        //DontDestroyOnLoad(this);
    }

    void OnEnable() {
    	SceneManager.activeSceneChanged += OnSceneWasSwitched;
    }

    void OnDisable() {
		SceneManager.activeSceneChanged -= OnSceneWasSwitched;
	}

	public void CompleteLevel() {
		LevelScore.LS.addFinalScore(); // TODO!!!!!!
		DataManager.DM.SaveData();
	}

	public void EndGame() {
		if(gameHasEnded == false) {
			gameHasEnded = true;
			vcam.enabled = false;
			PlayerMovement.PM.Die();
			Invoke("ShowEndMessage", menuDelay);
			Invoke("Restart", restartDelay);
		}
	}

	public void SetCharacter() {
    	characters = GameObject.FindGameObjectsWithTag("Player");
    	foreach(GameObject character in characters) {
    		character.SetActive(false);
    	}

    	characters[characterIndex].SetActive(true);
    	vcam.m_Follow = characters[characterIndex].transform;
	}

	public void Pause() {
		pauseMsg.SetActive(!pauseMsg.activeInHierarchy);
	}

	void ShowEndMessage() {
		endMsg.SetActive(true);
	}

	void OnSceneWasSwitched(Scene current, Scene next) {
		SetCharacter();
	}

	void Restart() {
		foreach(GameObject character in characters) {
    		character.GetComponent<PlayerMovement>().ResetPosition();
    	}
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
