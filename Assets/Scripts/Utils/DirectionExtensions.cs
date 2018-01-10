using UnityEngine;

static class DirectionExtensions
{
    public static Direction Inverse(this Direction self)
    {
        switch (self)
        {
            case Direction.TOP:
                return Direction.BOTTOM;
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.BOTTOM:
                return Direction.TOP;
            case Direction.LEFT:
                return Direction.RIGHT;
            default:
                return Direction.UNDEFINED;
        }
    }

    public static Direction ToDirection(this Vector2 self)
    {
        float angle = self.Angle();

        if (angle == 0)
            return Direction.TOP;
        if (angle == 90)
            return Direction.RIGHT;
        if (angle == 180)
            return Direction.BOTTOM;
        return Direction.LEFT;
    }

    public static Vector2 ToVector(this Direction self)
    {
        switch (self) {
            case Direction.TOP:
                return new Vector2(0, 1);

            case Direction.BOTTOM:
                return new Vector2(0, -1);

            case Direction.LEFT:
                return new Vector2(-1, 0);

            default:
                return new Vector2(1, 0);
        }
    }
}
