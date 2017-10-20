using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
		
    virtual protected void FixedUpdate()
    {
        
    }

	void setAnnimation(){
		//TODO
	}
}
