using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject Player;
    public GameObject LeftWall;
    public GameObject RightWall;
    private Camera camera;
    private WaitForSeconds wait;
    public Vector3 newPositon;
    public bool movingCameraToPosition = false;
    public float speed = 3.0f;
    private bool playerMovingWithCamera = false;
    private Vector3 dirToMove;
    public Vector3 newPlayerPos;
    public bool fixedCamera = false;

    private void Start()
    {
        camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        if (!fixedCamera)
        {
            if (!RightWall.GetComponent<CameraWall>().IsBlocked())
                if (Player.transform.position.x > this.transform.position.x)
                    this.transform.position = new Vector3((float)System.Math.Round(Player.transform.position.x, 2), this.transform.position.y, this.transform.position.z);
            if (!LeftWall.GetComponent<CameraWall>().IsBlocked())
                if (Player.transform.position.x < this.transform.position.x)
                    this.transform.position = new Vector3((float)System.Math.Round(Player.transform.position.x, 2), this.transform.position.y, this.transform.position.z);
        }
        
        if (Vector3.Distance(this.transform.position, newPositon) < 0.01f)
        {
            movingCameraToPosition = false;
            playerMovingWithCamera = false;
            Player.GetComponent<PlayerMovement>().UnFreeze();
        }
        if (movingCameraToPosition)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, newPositon, speed * Time.deltaTime);
            if (playerMovingWithCamera)
            {
                Player.GetComponent<PlayerMovement>().transform.position = Vector3.MoveTowards(Player.GetComponent<PlayerMovement>().transform.position, newPlayerPos, 1 * Time.deltaTime);
            }
        }
    }


    IEnumerator MoveCameraDownOneScreen()
    {
        newPositon = new Vector3(this.transform.position.x, this.transform.position.y - (camera.orthographicSize * 2.1f), this.transform.position.z);
        movingCameraToPosition = true;

        Player.GetComponent<PlayerMovement>().Freeze();
        yield return wait;
    }
    IEnumerator MoveCameraRightOneScreen()
    {
        fixedCamera = true;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = camera.orthographicSize;
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 1.24f * camHalfWidth;

        newPositon = new Vector3(this.transform.position.x + camWidth, this.transform.position.y, this.transform.position.z);
        movingCameraToPosition = true;
        playerMovingWithCamera = true;
        dirToMove = (newPositon - this.transform.position).normalized;
        newPlayerPos = Player.GetComponent<PlayerMovement>().transform.position + (dirToMove * 0.6f);
        Player.GetComponent<PlayerMovement>().WasteJumpsAndResetVelocity();
        yield return wait;
    }
}
