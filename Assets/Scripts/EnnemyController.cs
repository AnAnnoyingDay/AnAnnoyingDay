using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : EntityController {
    
    private Animator animator;                          //Variable of type Animator to store a reference to the enemy's Animator component.
    private Transform target;                           //Transform to attempt to move toward each turn.
    public float speed = 0.15f;

    protected override void Start()
    {
        base.Start();

        //Get and store a reference to the attached Animator component.
        animator = GetComponent<Animator>();

        //Find the Player GameObject using it's tag and store a reference to its transform component.
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.tryToAction();
    }

    public void tryToAction()
    {
        //If the difference in positions is approximately zero (Epsilon) do the following:
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon){
            this.attack();
        } else {
            this.move();
        }

    }

    public void attack(){
        //TODO
    }

    public void move()
    {
        Vector2 movement = new Vector2(target.position.y, target.position.x);
        transform.Translate(movement * speed);
    }
}
