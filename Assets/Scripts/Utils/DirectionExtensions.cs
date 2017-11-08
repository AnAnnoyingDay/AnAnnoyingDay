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
}
