using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {

    void Dash()
    {
        int dashLength = 1;
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float newPosX = move.x;
        float newPosY = move.y;

        if (newPosX != 0)
            newPosX = move.x > 0 ? move.x + dashLength : move.x - dashLength;
        if (newPosY != 0)
            newPosY = move.y > 0 ? move.y + dashLength : move.y - dashLength;

        transform.Translate(new Vector2(newPosX, newPosY));
    }

    void Moving()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector3(moveHorizontal, moveVertical);
        Vector3 rotation = new Vector3(0, 0, 0);
        transform.Translate(movement * speed);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Moving();
        if (Input.GetButtonDown("Dash"))
            Dash();
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
