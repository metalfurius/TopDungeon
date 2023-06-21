using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    public Animator Animator;
    protected float moveAnimation;
    protected RaycastHit2D Hit;
    protected float ySpeed = 0.85f;
    protected float xSpeed = 1.0f;
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    protected virtual void UpdateMotor(Vector3 input)
    {
        //Reset moveDelta
        moveDelta = new Vector3(input.x*xSpeed,input.y*ySpeed,0);

        //Swap sprite direction
        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Add push vector, if any
        moveDelta += pushDirection;
        //reduce the push every frame based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //move in this direction by casting a box there first, if box is 0 we move
        Hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y)
            , Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Creatures", "Blocking"));
        if (Hit.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        Hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0)
            , Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Creatures", "Blocking"));
        if (Hit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }

        //move animation
        moveAnimation = Mathf.Abs(moveDelta.x) + Mathf.Abs(moveDelta.y);
        Animator.SetFloat("Speed", moveAnimation);
    }
}


