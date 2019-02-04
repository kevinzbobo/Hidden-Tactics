using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{

    public static Constants.Elements GetElement(String element)
    {
        if (element.Equals(Constants.ElementNameFire)) return Constants.Elements.Fire;
        if (element.Equals(Constants.ElementNameWood)) return Constants.Elements.Wood;
        if (element.Equals(Constants.ElementNameMetal)) return Constants.Elements.Metal;
        if (element.Equals(Constants.ElementNameWater)) return Constants.Elements.Water;
        if (element.Equals(Constants.ElementNameEarth)) return Constants.Elements.Earth;
        return Constants.Elements.Fire;
    }

    public static bool IsOvercominInteraction(Constants.Elements target, Constants.Elements source)
    {

        Constants.Elements[] element = new Constants.Elements[] { Constants.Elements.Wood, Constants.Elements.Fire, Constants.Elements.Earth, Constants.Elements.Metal, Constants.Elements.Water };
        int targetIndex = Array.FindIndex(element, row => row == target);
        int sourceIndex = Array.FindIndex(element, row => row == source);

        Debug.Log("overcominga: " + sourceIndex + ":" + targetIndex);
        Debug.Log("overcomingb: " + (sourceIndex + 2) % 5 + ":" + targetIndex);

        return (sourceIndex + 2) % 5 == targetIndex;
    }

    public static bool IsGeneratingInteraction(Constants.Elements target, Constants.Elements source)
    {
        Constants.Elements[] element = new Constants.Elements[] { Constants.Elements.Wood, Constants.Elements.Fire, Constants.Elements.Earth, Constants.Elements.Metal, Constants.Elements.Water };
        int targetIndex = Array.FindIndex(element, row => row == target);
        int sourceIndex = Array.FindIndex(element, row => row == source);

        return (sourceIndex + 1) % 5 == targetIndex;
    }

    public static bool IsCommonInteraction(Constants.Elements target, Constants.Elements source)
    {
        return target == source;
    }

    public static Color32 IntToColor(uint hex)
    {
        byte alpha = (byte)(hex >> 24);
        byte red = (byte)(hex >> 16);
        byte green = (byte)(hex >> 8);
        byte blue = (byte)(hex >> 0);

        return new Color32(red, green, blue, alpha);
    }
}
