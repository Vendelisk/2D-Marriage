using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeMusic : MonoBehaviour {

    public AudioClip level1Music;

    private AudioSource source;

    // Use this for initialization
    void Awake () 
    {
        source = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneWasSwitched;
    }

    void OnDisable() {
		SceneManager.activeSceneChanged -= OnSceneWasSwitched;
	}

	void OnSceneWasSwitched(Scene current, Scene next) {
		string sceneName = current.name;
        if (name == "Level01")
        {
            source.clip = level1Music;
            source.Play ();
        }
	}
}