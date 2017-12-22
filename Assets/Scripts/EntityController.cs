using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

    protected Rigidbody2D rb2d;
    protected Transform trs;
    protected Coroutine attackRoutine;
    protected Animator myAnimator;

    public float speed = 0.1f;

    protected int maxHealth = 10;
    protected int currentHealth;
    public bool isAttacking = false;    


    virtual protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        currentHealth = maxHealth;
        //Debug.Log("Player health point : " + this.currentHealth);
    }
		
    
    virtual protected void FixedUpdate()
    {
    }


    protected void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0);
        }
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void takeDamage(int dmg) {
        this.currentHealth -= dmg;
        trs = gameObject.transform;

        var opposite = -rb2d.velocity;
        var brakePower = 10;
        var brakeForce = opposite.normalized * brakePower;
        rb2d.AddForce(brakeForce * Time.deltaTime);

        Debug.Log("Player HP : " + this.currentHealth);
        if (this.currentHealth <= 0)
        {
            Debug.Log("Player died !");
        }
    }
}
