using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class LayerTransitioner : MonoBehaviour {

    public int forwardLayer = 0;
    public int backLayer = 1;

    private List<LayerMapController> controller;

    void Start() {
        controller = new List<LayerMapController>();
    }

    void Update() {
        foreach(LayerMapController obj in controller)
            if(obj != null)
                if(obj.tryToTravers) {
                    obj.currentLayer = ( obj.currentLayer == forwardLayer ) ? backLayer : forwardLayer;
                    obj.tryToTravers = false;
                }

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<LayerMapController>())
            controller.Add(other.GetComponent<LayerMapController>());
    }

    void OnTriggerExit2D(Collider2D other) {
        controller.Remove(other.gameObject.GetComponent<LayerMapController>());
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(0, 1, 0, .5f);
        Gizmos.DrawCube(gameObject.transform.position, gameObject.transform.localScale);
    }
}
