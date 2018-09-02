using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingScript : MonoBehaviour
{

    public Transform target;
    private float speed = 2;
    private Vector3 startPosition;
    private bool? falling;
    private WaitForSeconds wait = new WaitForSeconds(2);
    

    private void Start()
    {
        startPosition = this.transform.position;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;

        if (falling.HasValue && falling.Value)
        {
            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                falling = null;
                StartCoroutine(StopFalling());
            }
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        else if (falling.HasValue && !falling.Value)
        {
            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                falling = null;
            }
            transform.position = Vector3.MoveTowards(transform.position, startPosition, step * 0.15f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == Tags.Player)
        {
            if (!falling.HasValue)
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        falling = true;
        yield return wait;
    }

    IEnumerator StopFalling()
    {
        yield return wait;
        falling = false;
    }
}
