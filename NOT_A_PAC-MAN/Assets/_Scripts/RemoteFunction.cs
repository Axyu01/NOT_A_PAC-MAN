using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class RemoteFunction
{
    public static string GetFunctionName(string message)
    {
        try
        {
            if (message.Split(')').Length == 2 && message.Split('(').Length == 2)
                return message.Split('(')[0];
            else
                return null;
        }
        catch
        {
            return null;
        }
    }
    public static string[] GetFunctionArguments(string message)
    {
        try { return message.Split('(')[1].Split(')')[0].Split(','); } catch { return null; }
    }
}
