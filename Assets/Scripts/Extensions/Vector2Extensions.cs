using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static float SqrDistance(this Vector2 a, Vector2 b) {
        return Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2);
    }
}
