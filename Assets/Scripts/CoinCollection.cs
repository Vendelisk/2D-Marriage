using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
	private bool hasCollided;

	void Awake() {
	}

    // LateUpdate is a mirror of Update which occurs after Update has finished
    void LateUpdate() {
    	hasCollided = false;
    }

    void OnTriggerEnter2D(Collider2D hit) {
    	if(hit.gameObject.tag.Equals("Player") && !hasCollided) {
    		this.gameObject.GetComponent<AudioSource>().Play();
    		StartCoroutine("Remove");
    		hasCollided = true;
    		LevelScore.LS.addScore(100);
    		LevelScore.LS.addCoin();
    	}
    }

    IEnumerator Remove() {
    	this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    	this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    	yield return new WaitForSeconds(.3f);
    	Destroy(this.gameObject);
    }
}
