using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyFeet : MonoBehaviour
{
	private Rigidbody2D player;
	private float max = 15f;
	private float cur = 0f;
	private bool jump;

	void Start() {
		player = GetComponentInParent<Rigidbody2D>();
	}

    void OnCollisionEnter2D(Collision2D hit) {
        if(hit.gameObject.layer == 10) {  // 10 IS ENEMY LAYER
        	/*if(hit.gameObject.tag.Equals("Deathcap")) {
        		hit.gameObject.GetComponent<DeathcapAI>().Die();
        	}*/
            jump = true;
        }
    }

    void FixedUpdate() {
    	if((cur < max) && jump) {
    		player.MovePosition(player.position + new Vector2(0f, 4f) * Time.fixedDeltaTime * 2);
    		cur += 5f;
    	}
    	else if((cur >= max) && jump) {
    		jump = false;
    		cur = 0f;
    	}
    }
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyFeet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D hit) {
        if(hit.gameObject.layer == 10) {  // 10 IS ENEMY LAYER
            DeathcapAI.Die(hit.gameObject);
            GetComponentInParent<Rigidbody2D>().AddRelativeForce(Vector2.up * 18, ForceMode2D.Impulse);
        }
    }
}
*/