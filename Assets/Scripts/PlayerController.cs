using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {

    //[SerializeField]
    //private Stat health;
    //private float maxHealth = 100;

    //[SerializeField]
    //private Stat mana;
    //private float maxMana = 50;

    protected Vector2 direction;

    public bool isMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    //void Dash()
    //{
    //    Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    //    float newMoveX = move.x;
    //    float newMoveY = move.y;
    //    float newPosX = transform.position.x;
    //    float newPosY = transform.position.y;
    //    if (newMoveX != 0)
    //    {
    //        if (move.x > 0)
    //        {
    //            newMoveX += dashLength;
    //            newPosX += dashLength;
    //        }
    //        else
    //        {
    //            newMoveX -= dashLength;
    //            newPosX -= dashLength;
    //        }
    //    }
    //    if (newMoveY != 0)
    //    {
    //        if (move.y > 0)
    //        {
    //            newMoveY += dashLength;
    //            newPosY += dashLength;
    //        }
    //        else
    //        {
    //            newMoveY -= dashLength;
    //            newPosY -= dashLength;
    //        }
    //    }

    //    //rb.AddForce( new Vector3(newMoveX, newMoveY, 0), ForceMode.Force);
    //    //Debug.Log("Dash");

    //    transform.Translate(new Vector2(newMoveX, newMoveY));

    //    //rigidbody.velocity = new Vector2(rigidbody.velocity.x * 3f, rigidbody.velocity.y);
    //}

    void Moving()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector2(moveHorizontal, moveVertical);
        rb2d.velocity = direction.normalized * speed;
    }

    // Update at same time always an not by frame (Avoid differencies between platform)
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Moving();
        // if (Input.GetButtonDown("Dash"))
        // Dash();
    }

	
	// Update is called once per frame
	void Update () {
        HandleLayers();		
	}

    void HandleLayers()
    {
        if (isMoving)
        {
            Debug.Log("Walk");
            ActivateLayer("WalkLayer");
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);
        }
        else
        {
            Debug.Log("Idle");

            ActivateLayer("IdleLayer");
        }
    }
}
