using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private List<GameObject> characters;
    // Default character index
    private int selectionIndex = 0;
    private int rotateDir = 1;
    private bool processingButton = false;

    private void Start() {
    	characters = new List<GameObject>();
    	foreach(Transform t in transform) {
    		characters.Add(t.gameObject);
    		t.gameObject.SetActive(false);
    	}

    	characters[selectionIndex].SetActive(true);
    }

    private void Update() {
    	if((Input.GetAxisRaw("Horizontal") == -1) && !processingButton) {
            processingButton = true;
    		if(selectionIndex - 1 < 0) 
    			Select(characters.Count -1);
    		else 
    			Select(selectionIndex - 1);
    	}
    	else if((Input.GetAxisRaw("Horizontal") == 1) && !processingButton) {
            processingButton = true;
    		if(selectionIndex + 1 >= characters.Count)
    			Select(0);
    		else
    			Select(selectionIndex + 1);
    	}
        else if(Input.GetAxisRaw("Horizontal") == 0) {
            processingButton = false;
        }

    	if(Input.GetButton("Submit")) {
    		SceneManager.LoadScene("Level01");
    	}

    	if(transform.rotation.y >= .2)
    		rotateDir = -1;
    	else if(transform.rotation.y <= -.2)
    		rotateDir = 1;

    	transform.Rotate(new Vector3(0.0f, .1f * rotateDir, 0.0f));
    }

    public void Select(int index) {
    	if(index == selectionIndex)
    		return;
    	if(index < 0 || index >= characters.Count)
    		return;

    	characters[selectionIndex].SetActive(false);
    	selectionIndex = index;
    	characters[selectionIndex].SetActive(true);
    	GameManager.characterIndex = selectionIndex;
    }
}
