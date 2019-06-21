using UnityEngine;

public class EndTrigger : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D hit) {
		if(hit.gameObject.tag.Equals("Player") && gameObject.tag.Equals("Finish")) {
			gameObject.GetComponent<AudioSource>().Play();
			//hit.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            //hit.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            Time.timeScale = 0f;
			this.gameObject.GetComponent<Animator>().enabled = true;
			GameManager.GM.CompleteLevel();
		}
		else if(hit.gameObject.tag.Equals("Player")) {
			GameManager.GM.EndGame();
		}
	}
}
