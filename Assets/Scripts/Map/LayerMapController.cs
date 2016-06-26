using UnityEngine;
using System.Collections;

public class LayerMapController : MonoBehaviour {

    public int currentLayer = 0;
    public bool isPlayer = false;
    [ReadOnly] public Player host;
    public bool tryToTravers = false;

    void Update() {
        if(Input.GetKeyDown(KeyCode.W)) tryToTravers = true;
        else tryToTravers = false;
    }
}
