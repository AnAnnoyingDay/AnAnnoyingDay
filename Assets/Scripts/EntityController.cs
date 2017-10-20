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
		
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector3(moveHorizontal, moveVertical);
        Vector3 rotation = new Vector3(0, 0, 0);

        transform.Translate(movement * speed);

        //Fixe la rotation
        transform.rotation = Quaternion.Euler(rotation);
    }

	void setAnnimation(){
		//TODO
	}
}
