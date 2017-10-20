using System;
using Random = UnityEngine.Random;

[Serializable]
public class Count
{
    public int minimum;
    public int maximum;

    public Count(int minimum, int maximum)
    {
        this.minimum = minimum;
        this.maximum = maximum;
    }

    public int GetRandomValue()
    {
        return Random.Range(this.minimum, this.maximum + 1);
    }
}
