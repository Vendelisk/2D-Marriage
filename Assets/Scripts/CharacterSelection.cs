using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characters;
    // Default character index
    private int selectionIndex = 0;
    private int rotateDir = 1;
    private bool processingButton = false;
    // Secret character
    public bool winners = false;

    private void Start() {
    	characters = GameObject.FindGameObjectsWithTag("Player");
        // Just make sure
    	foreach(GameObject character in characters) {
    		character.SetActive(false);
    	}

        if(String.Equals(characters[selectionIndex].name, "Dog") && !winners)
            selectionIndex++;
    	characters[selectionIndex].SetActive(true);
    }

    private void Update() {
    	if((Input.GetAxisRaw("Horizontal") == -1) && !processingButton) {
            processingButton = true;
    		if(selectionIndex - 1 < 0) 
    			Select(characters.Length -1);
    		else 
    			Select(selectionIndex - 1);
    	}
    	else if((Input.GetAxisRaw("Horizontal") == 1) && !processingButton) {
            processingButton = true;
    		if(selectionIndex + 1 >= characters.Length)
    			Select(0);
    		else
    			Select(selectionIndex + 1);
    	}
        else if(Input.GetAxisRaw("Horizontal") == 0) {
            processingButton = false;
        }

    	if(Input.GetButton("Submit")) {
    		SceneManager.LoadScene("Intro Cutscene");
    	}

    	if(transform.rotation.y >= .2)
    		rotateDir = -1;
    	else if(transform.rotation.y <= -.2)
    		rotateDir = 1;

    	transform.Rotate(new Vector3(0.0f, .1f * rotateDir, 0.0f));
    }

    public void Select(int index) {
    	if(index == selectionIndex) {
            Debug.Log("This should never happen");
    		return;
        }
    	if(index < 0 || index >= characters.Length) {
            Debug.Log("This should never happen");
    		return;
        }

    	characters[selectionIndex].SetActive(false);
    	selectionIndex = index;
        if(String.Equals(characters[index].name, "Dog") && !winners) {
            if(index < characters.Length)
                selectionIndex = index + 1;
            else 
                selectionIndex = 0;
        }
    	characters[selectionIndex].SetActive(true);
    	GameManager.characterIndex = selectionIndex;
    }
}
