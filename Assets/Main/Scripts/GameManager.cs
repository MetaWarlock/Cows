using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<GameObject> _cows;
    // Start is called before the first frame update
    void Start()
    {
        Targets.AddTarget(GameObject.FindGameObjectWithTag("Player"));
        // Find all of the cows in the scene and add them to the list
        _cows = new List<GameObject>(GameObject.FindGameObjectsWithTag("Cow"));
        // Add all of the cows to the Targets list
        foreach (GameObject cow in _cows)
        {
            Targets.AddTarget(cow);
        }
    }

}
