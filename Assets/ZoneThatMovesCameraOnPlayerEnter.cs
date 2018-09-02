using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneThatMovesCameraOnPlayerEnter : MonoBehaviour
{
    public GameObject MainCamera;
    public bool Right;
    private WaitForSeconds wait;
    private WaitForSeconds waitForCameraToCatchUp = new WaitForSeconds(1.5f);
    private WaitForSeconds waitBetweenSegments = new WaitForSeconds(0.1f);
    private float aTimeStamp;

    private void Start()
    {
        aTimeStamp = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == Tags.Player)
        {
            if (aTimeStamp + 4 < Time.time)
            {
                aTimeStamp = Time.time;
                if (!Right)
                    MainCamera.GetComponent<CameraScript>().StartCoroutine("MoveCameraDownOneScreen");
                else if (Right)
                {
                    StartCoroutine(OpenAndCloseBossDoor());
                }
            }
        }
    }



    IEnumerator OpenAndCloseBossDoor()
    {

        Transform[] children = this.GetComponentsInChildren<Transform>();
        MainCamera.GetComponent<CameraScript>().Player.GetComponent<PlayerMovement>().Freeze();
        foreach (var child in children)
        {
            if (child == this.transform) continue;

            child.gameObject.SetActive(false);
            yield return waitBetweenSegments;
        }
        MainCamera.GetComponent<CameraScript>().StartCoroutine("MoveCameraRightOneScreen");
        System.Array.Reverse(children);
        yield return waitForCameraToCatchUp;
        foreach (var child in children)
        {
            if (child == this.transform) continue;

            child.gameObject.SetActive(true);
            yield return waitBetweenSegments;
        }
        yield return wait;
    }
    IEnumerator Enable(GameObject child, bool enable)
    {
        child.SetActive(enable);
        yield return waitBetweenSegments;
    }
}
