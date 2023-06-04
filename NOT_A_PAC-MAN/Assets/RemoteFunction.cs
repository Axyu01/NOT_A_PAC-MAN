using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class RemoteFunction
{
    public static string GetFunctionName(string message)
    {
        try { return message.Split('(')[0]; } catch { return null; }
    }
    public static string[] GetFunctionArguments(string message)
    {
        try { return message.Split('(')[1].Split(')')[0].Split(','); } catch { return null; }
    }
}
