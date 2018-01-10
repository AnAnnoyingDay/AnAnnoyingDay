using UnityEngine;

static class VectorExtensions
{
    public static float Angle(this Vector2 self)
    {
        if (self.x < 0)
        {
            return 360 - (Mathf.Atan2(self.x, self.y) * Mathf.Rad2Deg * -1);
        }

        return Mathf.Atan2(self.x, self.y) * Mathf.Rad2Deg;
    }
}
