using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public List<GameObject> levels;
    public GameObject currentLevel = null;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        this.ChangeLevel();

        DontDestroyOnLoad(gameObject);
    }

    protected void ChangeLevel()
    {
        if (this.currentLevel != null && this.levels.Count > 0)
        {
            Destroy(this.currentLevel);
            int nextLevel = this.levels.FindIndex(level => level == this.currentLevel) + 1;
            this.currentLevel = this.levels[nextLevel];
        }
        else
        {
            this.currentLevel = this.levels[0];
        }

        this.currentLevel = Instantiate(this.currentLevel);
    }

    public GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player");
    }

    public void ChangeMap(Direction exitDirection)
    {
        this.currentLevel.GetComponent<LevelController>().MovePlayerToMap(exitDirection);
    }
}
