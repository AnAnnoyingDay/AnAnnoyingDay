using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameController;

    public void Awake()
    {
        if (GameController.instance == null)
        {
            Instantiate(this.gameController);
        }
    }
}
