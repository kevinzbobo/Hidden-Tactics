using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolLoader
{
    public Tool GetTool(int id)
    {
        Tool tool = new Tool();
        return tool;
    }
}

[Serializable]
public struct ToolData
{
    public int ID;
}
