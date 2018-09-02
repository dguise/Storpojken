using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_LeftRight_Orientation : MonoBehaviour {

    private SpriteRenderer sr;
    private Rigidbody2D rb;

	void Start () {
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        if (rb.velocity.x > 0)
            sr.flipX = true;
        if (rb.velocity.x < 0)
            sr.flipX = false;
    }
}
