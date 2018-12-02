using System;
using System.Collections.Generic;
using UnityEngine;


public class CrewMemberAttribute
{
    private static Dictionary<string, Vector2> s_allowedAttributes;

    static CrewMemberAttribute()
    {
        s_allowedAttributes = new Dictionary<string, UnityEngine.Vector2>();
        s_allowedAttributes.Add("Rowing", new Vector2(1f, 10f));
        s_allowedAttributes.Add("Scouting", new Vector2(1f, 10f));
        s_allowedAttributes.Add("Canon", new Vector2(1f, 10f));
    }

    public string AttributeName { get; private set; }

    public float AttributeValue { get; private set; }

    public static bool IsAllowedName(string name)
    {
        return s_allowedAttributes.ContainsKey(name);
    }

    public static bool IsAllowedValue(string name, float value)
    {
        Vector2 minMax;
        if (!s_allowedAttributes.TryGetValue(name, out minMax))
        {
            throw new ArgumentException("Invalid value", "name");
        }
        return (value >= minMax.x || value <= minMax.y);
    }

    // Create a new instance of the attribute
    public static CrewMemberAttribute CreateAttribute(string name, float value)
    {
        if (!IsAllowedValue(name, value))
        {
            throw new ArgumentOutOfRangeException("value");
        }

        return new CrewMemberAttribute(name, value);
    }

    // Increase skill factor when scaleFactor > 1
    // Decrese skill when scaleFactor < 1
    public void ScaleSkill(float scaleFactor)
    {
        AttributeValue = Math.Max(AttributeValue * scaleFactor, s_allowedAttributes[AttributeName].y);
    }

    private CrewMemberAttribute(string name, float value)
    {
        AttributeName = name;
        AttributeValue = value;
    }
}

