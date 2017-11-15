using System;
using Random = UnityEngine.Random;

[Serializable]
public class Count
{
    public int minimum;
    public int maximum;
    public int? fixedRandomValue;

    public Count(int minimum, int maximum)
    {
        this.minimum = minimum;
        this.maximum = maximum;
    }

    public int GetFixedRandomValue() {
        if (this.fixedRandomValue == null) {
            this.fixedRandomValue = this.GetRandomValue();
        }

        return (int) this.fixedRandomValue;
    }

    public int GetRandomValue()
    {
        return Random.Range(this.minimum, this.maximum + 1);
    }
}
