using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public List<GameObject> levels;
    public GameObject currentLevel = null;
    protected AstarPath pathfinding;

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

        this.pathfinding = GameObject.FindWithTag("Pathfinding").GetComponent<AstarPath>(); 
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

        this.GetPlayer().GetComponent<PlayerController>().hasKey = false;
        this.ReloadPathFinding(this.GetCurrentMap());
    }

    public GameObject GetCurrentMap()
    {
        Debug.Log(this.GetPlayer().transform.parent.name);
        return this.GetPlayer().transform.parent.gameObject;
    }

    public GameObject GetPlayer()
    {
        return GameObject.FindWithTag("Player").transform.gameObject;
    }

    public void ChangeMap(Direction exitDirection)
    {
        LevelController levelController = this.currentLevel.GetComponent<LevelController>();

        Vector2 positionCurrentMap = levelController.levelMaps.FirstOrDefault(x => x.Value.Equals(this.GetCurrentMap())).Key;
        Vector2 newPosition = positionCurrentMap + exitDirection.ToVector();

        GameObject newMap = levelController.levelMaps[newPosition];
        GameObject newExit = null;
        foreach (var exit in newMap.transform.FindObjectsWithTag("Exit"))
        {
            if (exit.GetComponent<HasDirection>().direction.Equals(exitDirection.Inverse()))
            {
                newExit = exit;
            }
        }

        if (!this.CanOpenDoor(newExit))
        {
            Debug.Log("Unable to open the door. Key is missing.");
            return;
        }

        this.GetPlayer().transform.SetParent(newMap.transform);
        Vector2 teleportLocation = (Vector2)newExit.transform.position + exitDirection.ToVector() * 1.4f;

        this.GetPlayer().transform.position = teleportLocation;
        this.ReloadPathFinding(newMap);
    }

    // TODO fix
    private void ReloadPathFinding(GameObject newMap)
    {
        var enemiesToDisable = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemiesToDisable)
        {
            enemy.GetComponent<AIDestinationSetter>().enabled = false;
            enemy.GetComponent<AILerp>().canMove = false;
            enemy.GetComponent<AILerp>().canSearch = false;
        }

        Vector2 mapSize = newMap.GetComponent<MapController>().mapSize;

        // Pas top mais on fait avec ça le temps de voir s'il y a mieux :o
        this.pathfinding.data.gridGraph.center = newMap.transform.position;
        this.pathfinding.data.gridGraph.SetDimensions((int) mapSize.x * 3, (int) mapSize.y * 3, 1f);
        this.pathfinding.Scan();

        var enemiesToEnable = newMap.transform.FindObjectsWithTag("Enemy");

        foreach(var enemy in enemiesToEnable) {
            if (this.GetPlayer().transform != null) {
                enemy.GetComponent<AIDestinationSetter>().target = this.GetPlayer().transform;
                enemy.GetComponent<AILerp>().canMove = true;
                enemy.GetComponent<AILerp>().canSearch = true;
                enemy.GetComponent<AIDestinationSetter>().enabled = true;
            }
        }
    }

    private bool CanOpenDoor(GameObject door)
    {
        return !(door.gameObject.GetComponentInParent<MapController>().isBoss
            && !this.GetPlayer().GetComponent<PlayerController>().hasKey);
    }

    public void PlayerPickedKey(GameObject key)
    {
        Destroy(key);
        this.GetPlayer().GetComponent<PlayerController>().hasKey = true;
    }
}
