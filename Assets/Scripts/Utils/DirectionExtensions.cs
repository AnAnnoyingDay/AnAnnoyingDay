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
        if (self.y > 0) return Direction.TOP;
        if (self.y < 0) return Direction.BOTTOM;
        if (self.x > 0) return Direction.RIGHT;

        return Direction.LEFT;
    }
}
