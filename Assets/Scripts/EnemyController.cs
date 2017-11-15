using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController {
    
    private Animator animator;
    private GameObject hitBox;
    private float timeToHit;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float coolDown = 10f;



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
        base.FixedUpdate();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player"){
            Debug.Log("Colliding with a player");
            attackRoutine = StartCoroutine(Attack(coll));
        }
    }

    private IEnumerator Attack(Collision2D coll){
        isAttacking = true;
        Debug.Log("Attacking player");
        coll.gameObject.GetComponentInParent<PlayerController>().takeDamage(this.damage);
        StopAttack();
        yield return new WaitForSeconds(1);
    }

}
