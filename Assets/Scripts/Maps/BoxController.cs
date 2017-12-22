using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController {

    public int X { get; set; }
    public int Y { get; set; }
    public int ActionOrder { get; set; }
    public bool IsBoss { get; set; }
    public bool IsKey { get; set; }
    public int DistFromBoss { get; set; }
    public string State { get; set; } 
    public Vector2Int[] Exits { get; set; }

    public BoxController()
    {
        ActionOrder = -1;
        IsBoss = false;
        IsKey = false;
        State = "Empty";
        DistFromBoss = -1;
    }
	
}
