using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {

    float speed = 8000.0f;
    public Vector2 direction = Vector2.left;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != Tags.Player)
            return;

        if (collision.gameObject.GetComponent<PlayerMovement>().isGrounded)
        {
            float convVelo = speed * Time.deltaTime;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.AddRelativeForce(convVelo * direction);
        }
    }
}
