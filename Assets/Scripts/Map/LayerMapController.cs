using UnityEngine;
using System.Collections;

public class LayerMapController : MonoBehaviour {

    public int currentLayer = 0;
    public bool isPlayer = false;
    [ReadOnly] public Player host;

    void Start() {
        
    }
}
