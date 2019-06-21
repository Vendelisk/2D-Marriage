using System;
using UnityEngine;
 
public class AudioChecker : MonoBehaviour {
 
	AudioSource[] sources;
             
    void Start () {      
		//Get every single audio sources in the scene.
		sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach(AudioSource audioSource in sources) {
			if(audioSource.transform.parent != null) {
				if(audioSource.transform.parent.parent != null) {
					if(audioSource.transform.parent.parent.parent != null) {
						if(audioSource.transform.parent.parent.parent.parent != null) {
							Debug.Log(audioSource.transform.parent.parent.parent.parent.name+" has "+audioSource.clip.name+ " attached to it");
							continue;
						}
						Debug.Log(audioSource.transform.parent.parent.parent.name+" has "+audioSource.clip.name+ " attached to it");
						continue;
					}
					Debug.Log(audioSource.transform.parent.parent.name+" has "+audioSource.clip.name+ " attached to it");
					continue;
				}
				Debug.Log(audioSource.transform.parent.name+" has "+audioSource.clip.name+ " attached to it");
				continue;
			}
			Debug.Log(audioSource.transform.name+" has "+audioSource.clip.name+ " attached to it");
		}
	}
}