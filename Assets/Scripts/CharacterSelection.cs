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
    private int hiddenIndex;
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

        if(String.Equals(characters[selectionIndex].name, "Dog") && !winners) {
            hiddenIndex = selectionIndex;
            selectionIndex++;
        }
    	characters[selectionIndex].SetActive(true);
    }

    private void Update() {
    	if((Input.GetAxisRaw("Horizontal") == -1) && !processingButton) {
            processingButton = true;
    		if(selectionIndex - 1 < 0) 
    			Select(characters.Length - 1, false);
    		else 
    			Select(selectionIndex - 1, false);
    	}
    	else if((Input.GetAxisRaw("Horizontal") == 1) && !processingButton) {
            processingButton = true;
    		if(selectionIndex + 1 >= characters.Length)
    			Select(0, true);
    		else
    			Select(selectionIndex + 1, true);
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

    // dir: true = right, false = left
    public void Select(int index, bool dir) {
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

        if(selectionIndex == hiddenIndex) {
            if(dir == true) {
                if(index + 1 > characters.Length - 1)
                    selectionIndex = 0;
                else
                    selectionIndex = index + 1;
            }
            else {
                if(index - 1 < 0)
                    selectionIndex = characters.Length - 1;
                else
                    selectionIndex = index - 1;
            }
        }

    	characters[selectionIndex].SetActive(true);
    	GameManager.characterIndex = selectionIndex;
    }
}
