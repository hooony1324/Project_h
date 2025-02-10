using System;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{
    public static Vector2 CanvasResolution = new Vector2(2280, 1080);

    public static float DISTANCE_MAX = 100;

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

    public static class IsoDir
    {
        public static Vector2 TOP_RIGHT = new Vector2(0.984f, 0.447f);
        public static Vector2 TOP_LEFT = new Vector2(-0.894f, 0.447f);
        public static Vector2 BOTTOM_RIGHT = new Vector2(0.894f, -0.447f);
        public static Vector2 BOTTOM_LEFT = new Vector2(-0.894f, -0.447f);
    }

    public const char MAP_TOOL_WALL = '0';
    public const char MAP_TOOL_CHARACTER_WALKABLE = '1'; 
    public const char MAP_TOOL_CAMERA_WALKABLE = '2';	
    public const int HERO_DEFAULT_MOVE_DEPTH = 8;

    public static class SortingLayers
    {
        public const int SPELL_INDICATOR = 200;
        public const int ENTITY = 300;
        public const int GATHERING_RESOURCES = 300;
        public const int PROJECTILE = 310;
        public const int DROP_ITEM = 310;
        public const int SKILL_EFFECT = 315;
        public const int WORLD_FONT = 410;

        public const int JOYSTICK = 500;
        public const int NPC_INTERACTION = 800;
    }

    public enum EDefaultSkillSlot
    {
        DefaultAttack,
        Dodge,
    }

    public enum EDungeonRoomType
    {
        None,
        BossMonster,
    }

    public enum EFloatingTextType
    {
        Damage,
        Heal,
        Buff,
        Debuff,
        CC,
    }
}