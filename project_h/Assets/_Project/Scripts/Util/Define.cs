using System;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{
    public static class MoveDir
    {
        public static Vector2 BOTTOM = new Vector2(0f, -1f);
        public static Vector2 BOTTOM_LEFT = new Vector2(-0.894f, -0.447f);
        public static Vector2 BOTTOM_RIGHT = new Vector2(0.894f, -0.447f);
        public static Vector2 TOP = new Vector2(0f, 1f);
        public static Vector2 TOP_LEFT = new Vector2(-0.894f, 0.447f);
        public static Vector2 TOP_RIGHT = new Vector2(0.894f, 0.447f);
        public static Vector2 LEFT = new Vector2(-1f, 0f);
        public static Vector2 RIGHT = new Vector2(1f, 0f);
    }
}