using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathcapAI : MonoBehaviour
{
	public float EnemySpeed;
	private int XMoveDirection;
	// used in LateUpdate to avoid single objects with 2 colliders reporting 2 collisions
	private bool hasCollided;
    private bool flipping;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        XMoveDirection = 1;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection, 0) * EnemySpeed;
    }

    // LateUpdate is a mirror of Update which occurs after Update has finished
    void LateUpdate() {
    	hasCollided = false;
    }

    void OnCollisionEnter2D(Collision2D hit) {
        // ignore
        if(hit.gameObject.layer == LayerMask.NameToLayer("Item")) {  }
        // kill self
        else if(hit.gameObject.CompareTag("Player") && hit.collider.gameObject.CompareTag("PlayerFeet")) {
            this.gameObject.GetComponent<AudioSource>().Play();
            this.Die();
            this.EnemySpeed = 0f;
        }
        // kill player
        else if(hit.gameObject.CompareTag("Player") && !hit.collider.gameObject.CompareTag("PlayerFeet")) {
            PlayerMovement.PM.Hurt();
        }
    	else if(!hit.gameObject.CompareTag("Ground") && !hasCollided) {
    		//SoundManagerScript.PlaySound("hit");
    		hasCollided = true;
    		Flip();
    	}
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if((col.gameObject.layer == LayerMask.NameToLayer("Enemy") || col.gameObject.layer == LayerMask.NameToLayer("Box")) && !flipping)
        {
            flipping = true;
            Flip();
            StartCoroutine(FlipDelay(.2f));
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
    	if(col.gameObject.CompareTag("EmptyCollider")) { // Don't walk off cliffs
    		Flip();
    	}
    }

    void Flip() {
		// Switch the way the player is labelled as facing.
		XMoveDirection *= -1;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    // enemy dies
    public void Die() {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        animator.SetBool("IsDead", true);
        LevelScore.LS.addScore(100);
        StartCoroutine("Remove");
    }

    IEnumerator Remove() {
        //this.gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }

    IEnumerator FlipDelay(float time)
    {
        yield return new WaitForSeconds(time);
        flipping = false;
    }
}
