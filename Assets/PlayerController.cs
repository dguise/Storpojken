using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement2 _controller;
    private Animator _animator;
    private SpriteRenderer sprite;

    //Settings
    private float gravity = 3.6f;
    private float smallJumpModifier = 0.3f;
    //private float gravity = 9.8f;
    private float runSpeed = 1.2f;
    private bool jumpReleased = true;

    private void Awake()
    {
        _controller = GetComponent<PlayerMovement2>();
        _animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var velocity = _controller.velocity;

        if (_controller.isGrounded)
            velocity.y = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity.x = runSpeed;
            GoRight();

            if (_controller.isGrounded)
            {
                if (_animator.GetBool("StartedRunning"))
                {
                    if (!_animator.GetBool("Running"))
                    {
                        _animator.SetBool("Running", true);
                        _animator.SetBool("StartedRunning", false);
                    }
                }
                else
                    if (!_animator.GetBool("Running"))
                    _animator.SetBool("StartedRunning", true);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity.x = -runSpeed;
            GoLeft();

            if (_controller.isGrounded)
            {
                if (_animator.GetBool("StartedRunning"))
                {
                    if (!_animator.GetBool("Running"))
                    {
                        _animator.SetBool("Running", true);
                        _animator.SetBool("StartedRunning", false);
                    }
                }
                else
                    if (!_animator.GetBool("Running"))
                    _animator.SetBool("StartedRunning", true);
            }
        }
        else
        {
            velocity.x = 0;

            if (_controller.isGrounded)
            {
                _animator.SetBool("Running", false);
                _animator.SetBool("StartedRunning", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
        {
            velocity = PerformJump(velocity);
            jumpReleased = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpReleased = true;
            //_controller.velocity.y = PerformSmallJump(_controller.velocity.y);
        }

        //Apply gravity before moving
        velocity.y -= (gravity * 1.5f) * Time.deltaTime;
        bool jumping = Mathf.Abs(velocity.y) > 0 && !_controller.isGrounded;
        _animator.SetBool("Jumping", jumping);
        if (jumping)
        {
            _animator.SetBool("Running", false);
            _animator.SetBool("StartedRunning", false);
        }

        _controller.Move(velocity * Time.deltaTime);
    }

    private Vector3 PerformJump(Vector3 velocity)
    {
        var targetJumpHeight = 1.5f;
        velocity.y = Mathf.Sqrt(1.0f * targetJumpHeight * gravity);
        return velocity;
    }

    float PerformSmallJump(float velo)
    {
        if (velo > gravity * smallJumpModifier)
            return gravity * smallJumpModifier;

        return velo;
    }

    private void GoLeft()
    {
        sprite.flipX = false;
    }

    private void GoRight()
    {
        sprite.flipX = true;
    }
}
