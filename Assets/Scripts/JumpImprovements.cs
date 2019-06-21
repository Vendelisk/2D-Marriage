using System.Collections;
using UnityEngine;

public class JumpImprovements : MonoBehaviour
{
	public float fallGravity = 7f;
	Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // falling
        if(rb.velocity.y < 0) {
        	rb.velocity += Vector2.up * Physics2D.gravity.y * (fallGravity - 1) * Time.deltaTime;
        }
        // end jump early
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump")) {
        	rb.velocity += Vector2.up * Physics2D.gravity.y * (fallGravity - 1) * Time.deltaTime;
        }
    }
}
