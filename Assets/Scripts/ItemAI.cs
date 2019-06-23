using System;
using System.Collections;
using UnityEngine;

public class ItemAI : MonoBehaviour
{
	private float ItemSpeed = 3;
	private int XMoveDirection;
	private Animator pointAnimator;

    // Start is called before the first frame update
    void Start()
    {
        XMoveDirection = 1;
        Physics2D.IgnoreLayerCollision(10, 14); // 10 IS ENEMY, 14 IS ITEM
        Physics2D.IgnoreLayerCollision(10, 5); // 5 IS UI
        pointAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection, 0) * ItemSpeed;
    }

    void OnCollisionEnter2D(Collision2D hit) {
    	if(!hit.gameObject.tag.Equals("Ground") && !(hit.gameObject.layer == 10) && !(hit.gameObject.layer == 12)) { // 10 IS ENEMY LAYER, 12 IS BOX
    		//SoundManagerScript.PlaySound("hit");
    		Flip();
    	}
    }

    void OnTriggerEnter2D(Collider2D hit) {
        if(hit.gameObject.tag.Equals("Player")) {
        	// GROW IN SIZE (GetDirection used to maintain flip() integrity)
        	Time.timeScale = 0f;
        	Debug.Log(this.gameObject.GetComponent<AudioSource>().name);
        	this.gameObject.GetComponent<AudioSource>().Play();

        	//Move up
        	Vector3 curPos = hit.gameObject.transform.position;
        	curPos.y += .4f;
        	hit.gameObject.transform.position = curPos;

        	//Get big
        	hit.gameObject.transform.localScale += new Vector3(2f * hit.gameObject.GetComponent<CharacterController2D>().GetDirection(), 2f, 0);
        	hit.gameObject.GetComponent<PlayerMovement>().isBig = true;

        	pointAnimator.enabled = true;
        	LevelScore.LS.addScore(1000);
        	GetComponent<Rigidbody2D>().gravityScale = 0;
        	GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        	Physics2D.IgnoreLayerCollision(14, 8); // 14 IS ITEM, 8 IS PLAYER
        	this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        	Time.timeScale = 1f;
        	// Has to stay alive long enough to play its audio bit
        	StartCoroutine("Remove");
        }
    	else if(hit.gameObject.tag.Equals("EmptyCollider")) { // Don't walk off cliffs
    		Flip();
    	}
    }

    void Flip() {
		// Switch the way the player is labelled as facing.
		XMoveDirection *= -1;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x = Mathf.Abs(theScale.x);
		transform.localScale = theScale;
    }

    IEnumerator Remove() {
    	yield return new WaitForSeconds(.3f);
    	Destroy(this.gameObject);
    }
}
