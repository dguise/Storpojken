using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{

    // Serialized
    [Header("Moving")]
    public float movementSpeed = 1.3f;
    public bool movementSmoothing = false;
    [Space(10)]
    [Header("Jumping")]
    public int maxJumps = 1;
    public float jumpForce = 3.6f;
    [Range(0, 1)]
    public float smallJumpModifier = 0.3f;
    public float fallForce = 3.6f;
    public bool requireGroundToJump = false;
    public bool enableWallJump = false;
    public bool bufferJumps = true;
    [Range(0, 1)]
    public float bufferJumpTime = 0.03f;
    [Space(10)]
    [Header("Misc/Settings")]

    [SerializeField]
    private LayerMask layerMask;

    // Privates
    private int jumpsLeft = 1;
    private bool shouldJump = false;
    private bool jumpPressed = false;
    private bool jumpReleased = true;
    public bool isGrounded = true;
    private Vector2 movement;

    private bool startedRunning = false;
    private bool running = false;
    private bool jumping = false;

    public bool frozen = false;

    // Components
    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator anim;
    BoxCollider2D collider;

    #region WeaponColors
    //Buster
    Color teal = new Color(0, 0.90980392156862745098039215686275f, 0.84705882352941176470588235294118f);
    Color blue = new Color(0, 0.43921568627450980392156862745098f, 0.92549019607843137254901960784314f);

    //Bubble Man
    Color white = new Color(0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f);
    Color gray = new Color(0.45490196078431372549019607843137f, 0.45490196078431372549019607843137f, 0.45490196078431372549019607843137f);

    //Air Man
    //Color white = new Color(0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f);
    Color darkBlue = new Color(0, 0.43921568627450980392156862745098f, 0.92549019607843137254901960784314f);

    //Quick Man
    Color pink = new Color(0.98823529411764705882352941176471f, 0.76862745098039215686274509803922f, 0.98823529411764705882352941176471f);
    Color darkPink = new Color(0.98823529411764705882352941176471f, 0.45490196078431372549019607843137f, 0.70588235294117647058823529411765f);

    //Heat Man
    Color heatmanTorso = new Color(0.94117647058823529411764705882353f, 0.73725490196078431372549019607843f, 0.23529411764705882352941176470588f);
    Color heatmanHelmet = new Color(0.89411764705882352941176470588235f, 0, 0.34509803921568627450980392156863f);

    Color heatmanTorso_2 = new Color(0.94117647058823529411764705882353f, 0.73725490196078431372549019607843f, 0.23529411764705882352941176470588f);
    Color heatmanHelmet_2 = new Color(0.89411764705882352941176470588235f, 0, 0.34509803921568627450980392156863f);
    Color heatmanOutline_2 = new Color(0.65882352941176470588235294117647f, 0.89411764705882352941176470588235f, 0.98823529411764705882352941176471f);

    Color heatmanTorso_3 = new Color(0.94117647058823529411764705882353f, 0.73725490196078431372549019607843f, 0.23529411764705882352941176470588f);
    Color heatmanHelmet_3 = new Color(0, 0.90980392156862745098039215686275f, 0.84705882352941176470588235294118f);
    Color heatmanOutline_3 = new Color(0.98823529411764705882352941176471f, 0.76862745098039215686274509803922f, 0.84705882352941176470588235294118f);

    Color heatmanTorso_4 = new Color(0.94117647058823529411764705882353f, 0.73725490196078431372549019607843f, 0.23529411764705882352941176470588f);
    Color heatmanHelmet_4 = new Color(0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f);
    Color heatmanOutline_4 = new Color(0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f);

    //Wood Man
    //Color white = new Color(0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f);
    Color green = new Color(0, 0.5803921568627450980392156862745f, 0);

    //Metal Man
    Color metalManWhite = new Color(0.98823529411764705882352941176471f, 0.84705882352941176470588235294118f, 0.65882352941176470588235294117647f);
    Color metalManBrown = new Color(0.53333333333333333333333333333333f, 0.43921568627450980392156862745098f, 0);

    //Flash Man
    Color flashManPurple = new Color(0.98823529411764705882352941176471f, 0.76862745098039215686274509803922f, 0.98823529411764705882352941176471f);
    Color flashManDarkPurple = new Color(0.73725490196078431372549019607843f, 0, 0.73725490196078431372549019607843f);

    //Crash Man
    //Color white = new Color(0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f);
    Color crashManRed = new Color(0.98823529411764705882352941176471f, 0.45490196078431372549019607843137f, 0.37647058823529411764705882352941f);

    //Rush
    //Color white = new Color(0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f, 0.98823529411764705882352941176471f);
    Color rushRed = new Color(0.84705882352941176470588235294118f, 0.15686274509803921568627450980392f, 0);
    #endregion

    public bool IgnoreLeftWallCheck = false;
    public bool IgnoreRightWallCheck = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();

#if UNITY_EDITOR
        if (LayerMask.LayerToName(gameObject.layer) != "Player")
        {
            Debug.LogError(name + " must be on layer called 'Player'. Is currently on layer: '" + LayerMask.LayerToName(gameObject.layer) + "'");
        }
#endif
    }

    void Update()
    {
        if (!frozen)
        {
            // Get movement input
            movement = new Vector2(
            (movementSmoothing ? Input.GetAxis("Horizontal") : Input.GetAxisRaw("Horizontal")) * movementSpeed,
            rb.velocity.y);


            if (Input.GetAxis("Horizontal") > 0)
                sprite.flipX = true;
            else if (Input.GetAxis("Horizontal") < 0)
                sprite.flipX = false;


            // Get jump input states
            if (Input.GetKeyDown(KeyCode.Space))
            {
                shouldJump = true;
                jumpReleased = false;
                jumpPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpReleased = true;
            }
        }
    }

    private bool m_decreaseJumpsOnce = true;
    private bool airborneBecauseOfJump = false;
    private bool m_usedRightWalljump = false;
    private bool m_usedLeftWalljump = false;

    void FixedUpdate()
    {
        if (!frozen)
        {
            //Debug.Log("Grounded = " + isGrounded);
            Vector3? HitPointGround = null;
            var groundDetected = ScanForGround(out HitPointGround);
            var rightWallDetected = ScanForRightWall();
            var leftWallDetected = ScanForLeftWall();
            var wallDetected = rightWallDetected || leftWallDetected;
            //Debug.Log("Ground Detected? " + groundDetected + "    " + Time.timeSinceLevelLoad);
            // Handle ground states
            if (groundDetected)
            {
                //transform.position = new Vector3(

                //    this.transform.position.x,
                //    (float)((Mathf.Round(Mathf.Floor(this.transform.position.y * 100))) / 100.0),
                //    transform.position.z);

                //if (!isGrounded)
                //    if (HitPointGround.HasValue)
                //        transform.position = new Vector3(
                //            this.transform.position.x, HitPointGround.Value.y ,
                //            transform.position.z);

                jumpsLeft = maxJumps;
                isGrounded = true;
                m_decreaseJumpsOnce = true;
                airborneBecauseOfJump = false;
            }
            else
            {
                isGrounded = false;

                // When walking out an edge with requireGroundToJump setting one jump should be removed
                // which results in no jumps if single jump & 1 jump if double jump, etc
                if (requireGroundToJump && !airborneBecauseOfJump && m_decreaseJumpsOnce)
                {
                    jumpsLeft--;
                    m_decreaseJumpsOnce = false;
                }
            }

            // Handle jump
            if (shouldJump)
            {
                var doJump = false;

                // Jumps left, or have Wall jump setting enabled
                if (jumpsLeft > 0 || enableWallJump && wallDetected)
                {
                    if (enableWallJump && !isGrounded && wallDetected)
                    {
                        if (rightWallDetected && !m_usedRightWalljump)
                        {
                            m_usedRightWalljump = true;
                            m_usedLeftWalljump = false;
                            doJump = true;
                        }
                        else if (leftWallDetected && !m_usedLeftWalljump)
                        {
                            m_usedRightWalljump = false;
                            m_usedLeftWalljump = true;
                            doJump = true;
                        }
                        else if (jumpsLeft > 0)
                        {
                            doJump = true;
                        }
                    }
                    else
                    {
                        doJump = true;
                    }
                }

                // Jump buffer setting
                if (!bufferJumps)
                {
                    shouldJump = false;
                }
                else
                {
                    if (!bufferJumpIsRunning && jumpPressed && shouldJump)
                    {
                        StartCoroutine(BufferJump());
                    }
                }

                if (doJump)
                {
                    PerformJump();
                }

                jumpPressed = false;
            }

            // Alter jump height
            if (jumpReleased)
            {
                PerformSmallJump();
            }

            // Apply falling force to combat Unity float
            if (!isGrounded)
            {
                rb.AddForce(Vector2.down * fallForce);
            }

            // Apply movement
            if (!isGrounded && (rightWallDetected && movement.x > 0 || leftWallDetected && movement.x < 0))
            {
                movement.x = 0;
            }
            rb.velocity = movement;


            // Animations
            jumping = Mathf.Abs(movement.y) > 0;

            if (isGrounded)
            {
                if (anim.GetBool("Jumping"))
                anim.SetBool("Jumping", false);
                bool hasXVelocity = Mathf.Abs(movement.x) > 0;
                if (hasXVelocity && anim.GetBool("Running") == false && anim.GetBool("StartedRunning") == false)
                {
                    Debug.Log("1");
                    anim.SetBool("StartedRunning", true);
                }
                else if (hasXVelocity && anim.GetBool("StartedRunning"))
                {
                    Debug.Log("2");
                    anim.SetBool("Running", true);

                    anim.SetBool("StartedRunning", false);
                }
                else if (hasXVelocity == false)
                {
                    Debug.Log("3");
                    anim.SetBool("Running", false);
                }
            }
            else if (!isGrounded && !leftWallDetected && !rightWallDetected)
            {
                if (anim.GetBool("Running"))
                {
                    Debug.Log("4");
                    anim.SetBool("Running", false);

                    anim.SetBool("Jumping", true);
                }
            }
        }
    }

    void PerformJump(bool fullJump = true)
    {
        SwapWeapon();
        movement.y = jumpForce;
        airborneBecauseOfJump = true;
        jumpsLeft--;
        shouldJump = false;
    }

    void PerformSmallJump()
    {
        if (rb.velocity.y > jumpForce * smallJumpModifier)
            movement.y = jumpForce * smallJumpModifier;
    }

    private bool bufferJumpIsRunning = false;
    IEnumerator BufferJump()
    {
        bufferJumpIsRunning = true;
        yield return new WaitForSeconds(bufferJumpTime);
        shouldJump = false;
        bufferJumpIsRunning = false;
    }

    bool ScanForGround(out Vector3? hitPoint)
    {
        // +- 0.1f to bring it in from the edges a bit, the down-raycasts would register on walls when
        // moving towards it otherwise
        float y = collider.bounds.min.y;
        var btmLeftEdgeOfPlayer = new Vector2(collider.bounds.min.x + 0.18f, y);
        var btmCenterfPlayer = new Vector2(collider.bounds.center.x, y);
        var btmRightEdgeOfPlayer = new Vector2(collider.bounds.max.x - 0.18f, y);

        var distanceDown = 0.03f;
        var objectBelow = Physics2D.Raycast(btmCenterfPlayer, Vector2.down, distanceDown, layerMask);
        if (objectBelow.collider == null)
            objectBelow = Physics2D.Raycast(btmLeftEdgeOfPlayer, Vector2.down, distanceDown, layerMask);
        if (objectBelow.collider == null)
            objectBelow = Physics2D.Raycast(btmRightEdgeOfPlayer, Vector2.down, distanceDown, layerMask);

        hitPoint = objectBelow.point;

        Debug.DrawRay(btmLeftEdgeOfPlayer, Vector2.down * distanceDown, Color.green);
        Debug.DrawRay(btmRightEdgeOfPlayer, Vector2.down * distanceDown, Color.blue);
        if (objectBelow.collider != null)
            Debug.DrawRay(btmCenterfPlayer, Vector2.down * distanceDown, Color.red);

        return objectBelow.collider != null && objectBelow.collider.tag == "Ground";
    }
    bool ScanForRightWall()
    {
        if (IgnoreRightWallCheck == true)
            return false;

        // +- 0.1f to bring it in from the edges a bit, the down-raycasts would register on walls when
        // moving towards it otherwise
        var rightTopEdgeOfPlayer = new Vector2(collider.bounds.max.x, collider.bounds.max.y - 0.1f);
        var rightBtmEdgeOfPlayer = new Vector2(collider.bounds.max.x, collider.bounds.min.y + 0.1f);

        Debug.DrawRay(rightTopEdgeOfPlayer, Vector2.right * 0.02f, Color.red);
        Debug.DrawRay(rightBtmEdgeOfPlayer, Vector2.right * 0.02f, Color.red);

        var posToCheck = new Vector3[] { rightTopEdgeOfPlayer, rightBtmEdgeOfPlayer };

        foreach (var pos in posToCheck)
        {
            var wallTouched = Physics2D.Raycast(pos, Vector2.right, 0.02f, layerMask);

            if (wallTouched.collider != null && wallTouched.collider.tag == "Ground")
            {
                m_usedLeftWalljump = false;
                return true;
            }
        }
        return false;
    }
    bool ScanForLeftWall()
    {
        if (IgnoreLeftWallCheck == true)
            return false;

        // +- 0.1f to bring it in from the edges a bit, the down-raycasts would register on walls when
        // moving towards it otherwise
        var leftTopEdgeOfPlayer = new Vector2(collider.bounds.min.x, collider.bounds.max.y - 0.1f);
        var leftBtmEdgeOfPlayer = new Vector2(collider.bounds.min.x, collider.bounds.min.y + 0.1f);
        Debug.DrawRay(leftTopEdgeOfPlayer, -Vector2.right * 0.02f, Color.red);
        Debug.DrawRay(leftBtmEdgeOfPlayer, -Vector2.right * 0.02f, Color.red);
        var posToCheck = new Vector3[] { leftTopEdgeOfPlayer, leftBtmEdgeOfPlayer };
        foreach (var pos in posToCheck)
        {
            var wallTouched = Physics2D.Raycast(pos, -Vector2.right, 0.02f, layerMask);

            if (wallTouched.collider != null && wallTouched.collider.tag == "Ground")
            {
                m_usedRightWalljump = false;
                return true;
            }
        }
        return false;
    }

    private Vector3 savedVelo = Vector3.zero;
    public void Freeze()
    {
        this.frozen = true;
        //savedVelo = rb.velocity;
        //rb.velocity = Vector3.zero;
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = false;
    }
    public void UnFreeze()
    {
        this.frozen = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        //rb.velocity = savedVelo; //I originalet tappar man all velo och faller efter att man är igenom dörren
        rb.simulated = true;
    }

    public void WasteJumpsAndResetVelocity()
    {
        this.jumpsLeft = 0;
        this.rb.velocity.Set(0, 0);
    }


    public void SwapWeapon()
    {
        int i = Random.Range(0, 9);


        switch (i)
        {
            case 0: //Buster
                sprite.material.SetColor("_Color1out", teal); //Mage
                sprite.material.SetColor("_Color2out", blue); //Hjälm, armar, ben, kalsonger
                break;
            case 1: //Bubble Man
                sprite.material.SetColor("_Color1out", white); //Mage
                sprite.material.SetColor("_Color2out", gray); //Hjälm, armar, ben, kalsonger
                break;
            case 2: //Air Man
                sprite.material.SetColor("_Color1out", white); //Mage
                sprite.material.SetColor("_Color2out", darkBlue); //Hjälm, armar, ben, kalsonger
                break;
            case 3: //Quick Man
                sprite.material.SetColor("_Color1out", pink); //Mage
                sprite.material.SetColor("_Color2out", darkPink); //Hjälm, armar, ben, kalsonger
                break;
            case 4: //Heat Man
                sprite.material.SetColor("_Color1out", heatmanTorso); //Mage
                sprite.material.SetColor("_Color2out", heatmanHelmet); //Hjälm, armar, ben, kalsonger
                break;
            case 5: //Wood Man
                sprite.material.SetColor("_Color1out", white); //Mage
                sprite.material.SetColor("_Color2out", green); //Hjälm, armar, ben, kalsonger
                break;
            case 6: //Metal Man
                sprite.material.SetColor("_Color1out", metalManWhite); //Mage
                sprite.material.SetColor("_Color2out", metalManBrown); //Hjälm, armar, ben, kalsonger
                break;
            case 7: //Flash Man
                sprite.material.SetColor("_Color1out", flashManPurple); //Mage
                sprite.material.SetColor("_Color2out", flashManDarkPurple); //Hjälm, armar, ben, kalsonger
                break;
            case 8: //Crash Man
                sprite.material.SetColor("_Color1out", white); //Mage
                sprite.material.SetColor("_Color2out", crashManRed); //Hjälm, armar, ben, kalsonger
                break;
            case 9: //Rush
                sprite.material.SetColor("_Color1out", white); //Mage
                sprite.material.SetColor("_Color2out", rushRed); //Hjälm, armar, ben, kalsonger
                break;
            default:
                break;
        }

    }
}




