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
        
        
       
    }
    private void FixedUpdate()
    {
        if (!fixedCamera)
        {
            if (!RightWall.GetComponent<CameraWall>().IsBlocked())
                if (Player.transform.position.x > this.transform.position.x)
                    this.transform.position = new Vector3((float)System.Math.Round(Player.transform.position.x, 3), this.transform.position.y, this.transform.position.z);
            if (!LeftWall.GetComponent<CameraWall>().IsBlocked())
                if (Player.transform.position.x < this.transform.position.x)
                    this.transform.position = new Vector3((float)System.Math.Round(Player.transform.position.x, 3), this.transform.position.y, this.transform.position.z);
        }

        if (movingCameraToPosition && Vector3.Distance(this.transform.position, newPositon) < 0.01f)
        {
            movingCameraToPosition = false;
            playerMovingWithCamera = false;
            Player.GetComponent<PlayerMovement>().UnFreeze();
        }
        if (movingCameraToPosition)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, newPositon, speed * Time.fixedDeltaTime);
            if (playerMovingWithCamera)
            {
                Player.GetComponent<PlayerMovement>().transform.position = Vector3.MoveTowards(Player.GetComponent<PlayerMovement>().transform.position, newPlayerPos, 1 * Time.fixedDeltaTime);
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
    float multiplier = 1;
    IEnumerator MoveCameraRightOneScreen()
    {
        fixedCamera = true;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = camera.orthographicSize;
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 1.24f * camHalfWidth;

        newPositon = new Vector3(this.transform.position.x + (camWidth * multiplier), this.transform.position.y, this.transform.position.z);
        movingCameraToPosition = true;
        playerMovingWithCamera = true;
        dirToMove = (newPositon - this.transform.position).normalized;
        Debug.Log("Dir to move = " + dirToMove);
        newPlayerPos = Player.GetComponent<PlayerMovement>().transform.position + (dirToMove * 0.6f);
        Player.GetComponent<PlayerMovement>().WasteJumpsAndResetVelocity();
        Debug.Log("Mult: " + multiplier);
        multiplier += 0.61f;
        yield return wait;
    }
}
