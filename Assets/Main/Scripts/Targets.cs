using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Targets
{
    // The list of targets
    public static List<GameObject> list = new List<GameObject>();

    
    // Add a new target to the list
    public static void AddTarget(GameObject target)
    {
        list.Add(target);
    }

    // Remove a target from the list
    public static void RemoveTarget(GameObject target)
    {
        list.Remove(target);
    }
}

