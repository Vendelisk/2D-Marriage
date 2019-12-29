using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyFeet : MonoBehaviour
{
	private Rigidbody2D player;

	void Start() {
		player = GetComponentInParent<Rigidbody2D>();
	}

    void OnCollisionEnter2D(Collision2D hit) {
        if(hit.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            player.AddForce(new Vector2(0f, 750f));
        }
    }
}