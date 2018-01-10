using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject GetCurrentMap()
    {
        return this.GetPlayer().transform.parent.gameObject;
    }

    public GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player");
    }

    public void ChangeMap(Direction exitDirection)
    {
        LevelController levelController = this.currentLevel.GetComponent<LevelController>();

        Vector2 positionCurrentMap = levelController.levelMaps.FirstOrDefault(x => x.Value.Equals(this.GetCurrentMap())).Key;
        Vector2 newPosition = positionCurrentMap + exitDirection.ToVector();

        GameObject newMap = levelController.levelMaps[newPosition];
        GameController.instance.GetPlayer().transform.SetParent(newMap.transform);

        GameObject newExit = null;
        foreach (var exit in newMap.transform.FindObjectsWithTag("Exit"))
        {
            if (exit.GetComponent<HasDirection>().direction.Equals(exitDirection.Inverse()))
            {
                newExit = exit;
            }
        }

        Vector2 teleportLocation = (Vector2)newExit.transform.position + exitDirection.ToVector() * 1.4f;

        GameController.instance.GetPlayer().transform.position = teleportLocation;
    }
}
