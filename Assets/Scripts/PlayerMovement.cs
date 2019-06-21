using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement PM;

	public CharacterController2D controller;
	public Animator animator;

	float horizontalMove = 0f;
	public float runSpeedOrig = 40f;
    public float runSpeed = 40f;
	bool jump = false;
	bool crouch = false;
    private bool isColliding;
    private bool boxHit;
    private static bool paused;
    private Color playerColor;

    // bit shift 1<<5 meaning "5th layer mask"
    private LayerMask enemyLayer = (1<<10);
    private LayerMask boxLayer = (1<<12);
    
    private bool dead;
    private float deathY = .1f;
    private bool reachedDeathY = false;
    private float amtFallen = 0f;
    public bool isBig;

    private Rigidbody2D playerRB;

    public AudioSource DeathSound;
    public AudioSource HurtSound;

    // Start is called before the first frame update
    void Start()
    {
        /*if(PM != null)
            GameObject.Destroy(PM);
        else*/
            PM = this;
         
        //DontDestroyOnLoad(this);
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        playerColor = gameObject.GetComponent<SpriteRenderer>().material.color;
        Physics2D.IgnoreLayerCollision(8, 5); // 5 IS UI, 8 IS PLAYER
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawRay(new Vector2(transform.position.x-.2f, transform.position.y), Vector2.up * .2f, Color.blue);
        //Debug.DrawRay(new Vector2(transform.position.x+.2f, transform.position.y), Vector2.up * .2f, Color.blue);
        //Debug.DrawRay(new Vector2(transform.position.x+.5f, transform.position.y-.5f), Vector2.down *.3f, Color.blue);


        if(dead) {
            DeathHop();
        }
        else if(Input.GetButtonDown("Pause")) {
            if(!paused) {
                paused = true;
                GameManager.GM.Pause();
                Time.timeScale = 0f;
            }
            else {
                paused = false;
                GameManager.GM.Pause();
                Time.timeScale = 1f;
            }
        }
        else if(!paused) {
            if(!isColliding) 
                horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("MoveSpeed", Mathf.Abs(horizontalMove));

            // JUMP ANIMATIONS
            if(playerRB.velocity.y > 2f) {
                animator.SetBool("IsJumping", true);
            }
            else if(playerRB.velocity.y < -2f) {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", true);
            }
            else {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }

            if(Input.GetButtonDown("Jump") && controller.isGrounded()) {
                gameObject.GetComponent<AudioSource>().Play();
                jump = true;
            }

            if((Input.GetAxisRaw("Vertical") == 1) && controller.isGrounded()) {
            	crouch = true;
            }
            else {
            	crouch = false;
            }

            // RUN FASTER
            if(Input.GetButton("Run") && controller.isGrounded()) {
                runSpeed = 60f;
            }
            else if((runSpeed != runSpeedOrig) && !controller.isGrounded()) { /* Do Nothing */ }
            else {
                runSpeed = runSpeedOrig;
            }

            PlayerRaycast();
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if(!controller.isGrounded() && !(col.gameObject.layer == 10)) { // 10 IS ENEMY LAYER
            isColliding = true;
            horizontalMove = 0f;
        }
        else {
            isColliding = false;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        isColliding = false;
    }

    public void OnLanding() {
        //animator.SetBool("IsJumping", false);
    	//animator.SetBool("IsFalling", false);
        boxHit = false;
    }

    public void OnCrouching(bool isCrouching) {
    	animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate() {
    	// Move Character
    	controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
    	jump = false;
    }

    void PlayerRaycast() {
        // HIT FROM BENEATH
        RaycastHit2D rayUpBoxL = Physics2D.Raycast(new Vector2(transform.position.x-.2f, transform.position.y), Vector2.up, .2f, boxLayer);
        RaycastHit2D rayUpBoxR = Physics2D.Raycast(new Vector2(transform.position.x+.2f, transform.position.y), Vector2.up, .2f, boxLayer);
        if(isBig) {
            rayUpBoxL = Physics2D.Raycast(new Vector2(transform.position.x-.2f, transform.position.y), Vector2.up, .4f, boxLayer);
            rayUpBoxR = Physics2D.Raycast(new Vector2(transform.position.x+.2f, transform.position.y), Vector2.up, .4f, boxLayer);
        }

        if(rayUpBoxL && (rayUpBoxL.distance < 1f) && !boxHit) {
            boxHit = true;
            if(rayUpBoxL.collider.gameObject.tag.Equals("CoinBox") && !rayUpBoxL.collider.gameObject.GetComponent<Animator>().GetBool("IsCoin")) {
                rayUpBoxL.collider.gameObject.GetComponent<Animator>().SetBool("IsCoin", true);
                rayUpBoxL.collider.gameObject.GetComponent<AudioSource>().Play();
                LevelScore.LS.addScore(200);
                LevelScore.LS.addCoin();
            }
            else if(rayUpBoxL.collider.gameObject.tag.Equals("ItemBox") && !rayUpBoxL.collider.gameObject.GetComponent<Animator>().GetBool("IsCrab")) {
                rayUpBoxL.collider.gameObject.GetComponent<Animator>().SetBool("IsCrab", true);
                rayUpBoxL.collider.gameObject.GetComponent<AudioSource>().Play();
                StartCoroutine("NoAnimator", rayUpBoxL.collider.gameObject.GetComponent<Animator>());
            }
            else if(rayUpBoxL.collider.gameObject.tag.Equals("BreakBox")) {
                if(!rayUpBoxL.collider.gameObject.GetComponent<AudioSource>().isPlaying)
                    rayUpBoxL.collider.gameObject.GetComponent<AudioSource>().Play();
                if(isBig) {
                    rayUpBoxL.collider.gameObject.GetComponent<Animator>().SetBool("IsBroken", true);
                    StartCoroutine("Remove", rayUpBoxL.collider.gameObject);
                }
            }
        }
        else if(rayUpBoxR && (rayUpBoxR.distance < 1f) && !boxHit) {
            boxHit = true;
            if(rayUpBoxR.collider.gameObject.tag.Equals("CoinBox") && !rayUpBoxR.collider.gameObject.GetComponent<Animator>().GetBool("IsCoin")) {
                rayUpBoxR.collider.gameObject.GetComponent<Animator>().SetBool("IsCoin", true);
                rayUpBoxR.collider.gameObject.GetComponent<AudioSource>().Play();
                LevelScore.LS.addScore(200);
                LevelScore.LS.addCoin();
            }
            else if(rayUpBoxR.collider.gameObject.tag.Equals("ItemBox") && !rayUpBoxR.collider.gameObject.GetComponent<Animator>().GetBool("IsCrab")) {
                rayUpBoxR.collider.gameObject.GetComponent<Animator>().SetBool("IsCrab", true);
                rayUpBoxR.collider.gameObject.GetComponent<AudioSource>().Play();
                StartCoroutine("NoAnimator", rayUpBoxR.collider.gameObject.GetComponent<Animator>());
            }
            else if(rayUpBoxR.collider.gameObject.tag.Equals("BreakBox")) {
                if(!rayUpBoxR.collider.gameObject.GetComponent<AudioSource>().isPlaying)
                    rayUpBoxR.collider.gameObject.GetComponent<AudioSource>().Play();
                if(isBig) {
                    rayUpBoxR.collider.gameObject.GetComponent<Animator>().SetBool("IsBroken", true);
                    StartCoroutine("Remove", rayUpBoxR.collider.gameObject);
                }
            }
        }
    }

    void DeathHop() {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        for(int i = 0; i < 32; ++i) {
            Physics2D.IgnoreLayerCollision(8, i);
        }
        if(!reachedDeathY && dead) {
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y+.3f), .5f);
            deathY += .3f;
            if(deathY >= 5f)
                reachedDeathY = true;
        }
        else if(amtFallen < 15f && dead) {
            amtFallen += .15f;
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y-.15f), .5f);
        }
    }

    public void Hurt() {
        if(!isBig)
            Die();
        else {
            StartCoroutine("Invincible");
            isBig = false;
            HurtSound.Play();
            transform.localScale = new Vector3(5f * controller.GetDirection(), 5f, 1f);
        }
    }

    // player dies
    public void Die() {
        DeathSound.Play();
        animator.Play("Die");
        dead = true;
        GameManager.GM.EndGame();
    }

    public void ResetPosition() {
        dead = false;
        for(int i = 0; i < 32; ++i) {
            Physics2D.IgnoreLayerCollision(8, i, false);
        }
    }

    IEnumerator Remove(GameObject toDestroy) {
        yield return new WaitForSeconds(.25f);
        Destroy(toDestroy);
    }

    IEnumerator NoAnimator(Animator animator) {
        yield return new WaitForSeconds(.25f);
        animator.enabled = false;
    }

    IEnumerator Invincible() {
        Physics2D.IgnoreLayerCollision(8, 10);
        playerColor.a = .5f;
        gameObject.GetComponent<SpriteRenderer>().material.color = playerColor;
        yield return new WaitForSeconds(2f);
        Physics2D.IgnoreLayerCollision(8, 10, false);
        playerColor.a = 1f;
        gameObject.GetComponent<SpriteRenderer>().material.color = playerColor;
    }
}
