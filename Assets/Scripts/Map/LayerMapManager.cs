using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LayerMapManager : MonoBehaviour {
    
    public LayeredMap[] maps;
    public List<LayerMapController> entities;
    public int currentMap;

    void Start() {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach(GameObject go in allObjects) 
            if(go.GetComponent<LayerMapController>())
                entities.Add(go.GetComponent<LayerMapController>());

        for(int i = 0; i < maps.Length; ++i) maps[i].Setup();

    }

    void Update() {
        foreach(LayerMapController l in entities)
            if(l.isPlayer)
                currentMap = l.currentLayer;

        // Scan through maps to find if their layer is less than current map
        // If so, toggle both to false
        // If their layer is greater toggle colliders to false and renderers to true
        // Finally if it is the current layer turn both true

        for(int i = 0; i < maps.Length; ++i) {
            if(i < currentMap) { 
                maps[i].ToggleColliders(false);
                maps[i].ToggleMesh(false);
            } else if(i > currentMap) {
                maps[i].ToggleColliders(false);
                maps[i].ToggleMesh(true);
            } else if(i == currentMap) {
                maps[i].ToggleColliders(true);
                maps[i].ToggleMesh(true);
            }
        }
    }

    [System.Serializable]
    public class LayeredMap {
        public GameObject parent;

        public List<MeshRenderer> meshes;
        public List<BoxCollider2D> colliders;
        public int layer;

        public void Setup() {
            meshes = new List<MeshRenderer>();
            colliders = new List<BoxCollider2D>();

            List<GameObject> gs = new List<GameObject>();
            Transform[] ts = parent.GetComponentsInChildren<Transform>();
            foreach(Transform t in ts) 
                if(t != null && t.gameObject != null) gs.Add(t.gameObject);

            foreach(GameObject child in gs) {
                if(child.GetComponent<MeshRenderer>()) meshes.Add(child.GetComponent<MeshRenderer>());
                if(child.GetComponent<BoxCollider2D>()) colliders.Add(child.GetComponent<BoxCollider2D>());
            }
        }

        public void ToggleMesh(bool state) {
            foreach(MeshRenderer m in meshes)
                m.enabled = state;
        }
        public void ToggleColliders(bool state) {
            foreach(BoxCollider2D b in colliders)
                b.enabled = state;
        }
    }
}
