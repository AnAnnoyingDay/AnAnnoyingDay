using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {
    
    private Animator animator;
    private GameObject hitBox;
    private float timeToHit;
    public int damage ;
    public float coolDown = 10f;
    private bool inPlayerCollider;
    private PlayerController playerAttacked;



    protected override void Start()
    {
        base.Start();

        //Get and store a reference to the attached Animator component.
        animator = GetComponent<Animator>();
        //hitBox = GetComponent("HitBox");
        timeToHit = Time.time + coolDown;
        damage = 1;
    }

    protected override void FixedUpdate()
    {
        if (isAttacking)
        {
            StartCoroutine(ContinuousAttack());
        }
        base.FixedUpdate();
    }
        
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerAttacked = coll.gameObject.GetComponentInParent<PlayerController>();
            playerAttacked.takeDamage(this.damage);
            isAttacking = true;
            inPlayerCollider = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
            inPlayerCollider = false;
    }

    private IEnumerator ContinuousAttack(){
        isAttacking = false;
        Debug.Log("Attacking player");
        yield return new WaitForSeconds(3);
        if (inPlayerCollider)
        {
            playerAttacked.takeDamage(this.damage);
            isAttacking = true;
        }
        else if (!inPlayerCollider)
        {
            Debug.Log("I'm breakiiiiiiiiing !");
            yield break;
        }
    }

}
