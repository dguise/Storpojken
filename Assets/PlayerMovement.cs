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
            rb.velocity.y
        );

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
            var groundDetected = ScanForGround();
            var rightWallDetected = ScanForRightWall();
            var leftWallDetected = ScanForLeftWall();
            var wallDetected = rightWallDetected || leftWallDetected;

            // Handle ground states
            if (groundDetected)
            {
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
                anim.SetBool("Jumping", jumping);
                bool NowRunning = Mathf.Abs(movement.x) > 0;
                if (NowRunning && running == false && startedRunning == false)
                {
                    startedRunning = true;
                    anim.SetBool("StartedRunning", true);
                }
                else if (NowRunning && startedRunning)
                {
                    running = true;
                    anim.SetBool("Running", true);

                    startedRunning = false;
                    anim.SetBool("StartedRunning", false);
                }
                else if (NowRunning == false)
                {
                    running = false;
                    anim.SetBool("Running", false);
                }
            }
            else if (!isGrounded && !leftWallDetected && !rightWallDetected)
            {
                if (running)
                {
                    running = false;
                    anim.SetBool("Running", false);

                    anim.SetBool("Jumping", true);
                }
            }
        }
    }

    void PerformJump(bool fullJump = true)
    {
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

    bool ScanForGround()
    {
        // +- 0.1f to bring it in from the edges a bit, the down-raycasts would register on walls when
        // moving towards it otherwise
        var btmLeftEdgeOfPlayer = new Vector2(collider.bounds.min.x + 0.18f, collider.bounds.min.y);
        var btmCenterfPlayer = new Vector2(collider.bounds.center.x, collider.bounds.min.y);
        var btmRightEdgeOfPlayer = new Vector2(collider.bounds.max.x - 0.18f, collider.bounds.min.y);

        var objectBelow = Physics2D.Raycast(btmCenterfPlayer, Vector2.down, 0.02f, layerMask);
        if (objectBelow.collider == null)
            objectBelow = Physics2D.Raycast(btmLeftEdgeOfPlayer, Vector2.down, 0.02f, layerMask);
        if (objectBelow.collider == null)
            objectBelow = Physics2D.Raycast(btmRightEdgeOfPlayer, Vector2.down, 0.02f, layerMask);

        Debug.DrawRay(btmLeftEdgeOfPlayer, Vector2.down * 0.03f, Color.green);
        Debug.DrawRay(btmRightEdgeOfPlayer, Vector2.down * 0.03f, Color.blue);
        Debug.DrawRay(btmCenterfPlayer, Vector2.down * 0.03f, Color.red);

        return objectBelow.collider != null && objectBelow.collider.tag == "Ground";
    }
    bool ScanForRightWall()
    {
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

    public void Freeze()
    {
        this.frozen = true;
        rb.bodyType = RigidbodyType2D.Static;
        rb.simulated = false;
    }
    public void UnFreeze()
    {
        this.frozen = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.simulated = true;
    }

    public void WasteJumpsAndResetVelocity()
    {
        this.jumpsLeft = 0;
        this.rb.velocity.Set(0, 0);
    }
}




