using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class LayerTransitioner : MonoBehaviour {

    public int forwardLayer = 0;
    public int backLayer = 1;

    private LayerMapController controller;

    void Update() {
        if(controller != null)
            if(controller.isPlayer && controller.tryToTravers) {
                controller.currentLayer = ( controller.currentLayer == forwardLayer )
                                                ? backLayer : forwardLayer;
                controller.tryToTravers = false;
            }

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<LayerMapController>())
            controller = other.gameObject.GetComponent<LayerMapController>();
    }

    void OnTriggerExit2D(Collider2D other) {
        controller = null;
    }
}
