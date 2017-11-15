using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

    protected Rigidbody2D rb2d;
    protected Coroutine attackRoutine;
    [SerializeField]
    protected int maxHealth = 1;
    [SerializeField]
    protected int currentHealth;
    protected bool isAttacking = false;


    virtual protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }
		
    virtual protected void FixedUpdate()
    {
        
    }

	void setAnnimation(){
		//TODO
	}

    public void takeDamage(int dmg) {
        this.currentHealth -= dmg;
        this.rb2d.AddForce(new Vector2()));
        Debug.Log("Player lost " + dmg + " health point");
        if (this.currentHealth <= 0)
        {
            Debug.Log("Player died !");
        }
    }

    public void StopAttack(){
        if(attackRoutine != null){
            StopCoroutine(attackRoutine);
            isAttacking = false;
        }
    }
}
