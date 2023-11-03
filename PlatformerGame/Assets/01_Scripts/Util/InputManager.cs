using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    Horizontal,
    Vertical
}

public enum Key
{
    Left,
    Right,
    Up,
    Down,
    Run,
    Interaction,
    DefaultAttack,
    Dash,
    Skill3,
    Skill4,
    Escape,
    OpenInventory
}

public static class InputManager
{
    static Dictionary<Key, KeyCode> keys = new Dictionary<Key, KeyCode>();

    static InputManager()
    {
        keys.Add(Key.Left, KeyCode.LeftArrow);
        keys.Add(Key.Right, KeyCode.RightArrow);
        keys.Add(Key.Up, KeyCode.UpArrow);
        keys.Add(Key.Down, KeyCode.DownArrow);
        keys.Add(Key.Run, KeyCode.LeftShift);
        keys.Add(Key.Interaction, KeyCode.F);
        //Init Skill keys
        keys.Add(Key.DefaultAttack, KeyCode.Space);
        keys.Add(Key.Dash, KeyCode.Q);
        keys.Add(Key.Skill3, KeyCode.W);
        keys.Add(Key.Skill4, KeyCode.E);
        //Init Util Keys
        keys.Add(Key.Escape, KeyCode.Escape);
        keys.Add(Key.OpenInventory, KeyCode.Tab);
    }

    public static float GetAxisRaw(Axis axis)
    {
        float value = 0f;

        if (axis == Axis.Horizontal)
        {
            value = GetHorizontalAxis();
        }

        else if (axis == Axis.Vertical)
        {
            value = GetVerticalAxis();
        }

        return value;
    }

    public static bool GetKey(Key key)
    {
        if (!keys.ContainsKey(key)) return false;

        return Input.GetKey(keys[key]);
    }

    public static bool GetKeyDown(Key key)
    {
        if (!keys.ContainsKey(key)) return false;

        return Input.GetKeyDown(keys[key]);
    }

    public static bool GetKeyUp(Key key)
    {
        if (!keys.ContainsKey(key)) return false;

        return Input.GetKeyUp(keys[key]);
    }

    static float GetHorizontalAxis()
    {
        float value = 0f;
        
        if (GetKey(Key.Left))
        {
            value = -1f;
        }

        else if (GetKey(Key.Right))
        {
            value = 1f;
        }

        if (GetKey(Key.Right) && GetKey(Key.Left))
        {
            value = 0f;
        }

        return value;
    }
    
    static float GetVerticalAxis()
    {
        float value = 0f;
        
        if (GetKey(Key.Down))
        {
            value = -1f;
        }

        else if (GetKey(Key.Up))
        {
            value = 1f;
        }

        if (GetKey(Key.Up) && GetKey(Key.Down))
        {
            value = 0f;
        }

        return value;
    }
}
