using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWall : MonoBehaviour
{

    private bool WallIsBlocked;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().IsTouchingLayers())
        {
            WallIsBlocked = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<Collider2D>().IsTouchingLayers())
        {
            WallIsBlocked = false;
        }
    }

    public bool IsBlocked()
    {
        return WallIsBlocked;
    }
}
