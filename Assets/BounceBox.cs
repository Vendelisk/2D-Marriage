using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBox : MonoBehaviour {
	private bool needsBounce = false;
	private Vector2 startPos;

    void OnTriggerEnter2D(Collider2D hit) {
    	if(hit.gameObject.tag.Equals("Player") && !hit.gameObject.GetComponent<PlayerMovement>().isBig) {
    		startPos = this.gameObject.transform.position;
    		needsBounce = true;
        }
    }

    // fixed update?
    void Update() {
    	if(needsBounce) {
    		Vector2 endPos = this.gameObject.transform.position;
    		endPos.y += 1f;
    		// curPos.y += .1f;
    		// hit.gameObject.transform.position = curPos;
    		Vector2.Lerp(startPos, endPos, .5f);
    	}
    }
}
