using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LayerMapManager : MonoBehaviour {

    public GameObject[] mapLayers;
    public List<LayerMapController> entities;
    public int currentMap;

    void Start() {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject go in allObjects) {
            if(go.GetComponent<LayerMapController>()) {
                entities.Add(go.GetComponent<LayerMapController>());
            }
        }
    }

    void Update() {
        HandleMaps();
        DrawFocusMap();
    }

    public void AddToMap(LayerMapController controller) {
        entities.Add(controller);
    }

    void ChangeLayer(LayerMapController controller, int layer) {
        if(entities.Contains(controller) && controller.currentLayer != layer) {
            controller.currentLayer = layer;
        }
    }

    void DrawFocusMap() { FullActiveMap(currentMap); }

    void HandleMaps() {
        for(int i = 0; i < mapLayers.Length; ++i) {
            if(i < currentMap) { FullDeactiveMap(i);
            } else if(i > currentMap) { HalfDeactiveMap(i);
            }
        }
    }

    void FullActiveMap(int i) {
        mapLayers[i].SetActive(true);
        BoxCollider2D[] colliders = mapLayers[i].GetComponentsInChildren<BoxCollider2D>();
        for(int c = 0; c < colliders.Length; ++c) colliders[c].enabled = true;
    }

    void HalfActiveMap(int i) {

    }

    void FullDeactiveMap(int i) {
        mapLayers[i].SetActive(false);
    }
    void HalfDeactiveMap(int i) {
        BoxCollider2D[] colliders = mapLayers[i].GetComponentsInChildren<BoxCollider2D>();
        for(int c = 0; c < colliders.Length; ++c) colliders[c].enabled = false;
    }
}
