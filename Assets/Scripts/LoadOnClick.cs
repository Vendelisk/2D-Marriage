using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadOnClick : MonoBehaviour
{
	public void LoadScene(int level) {
		SceneManager.LoadScene(level);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
