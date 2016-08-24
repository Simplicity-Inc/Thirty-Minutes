using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

    //List<HasTime> npcTimes = new List<HasTime>();
    public Camera cameraInUse;

    public LayerMask layer;
    RaycastHit2D hit;
    Ray playerToMouse;
    float distance;

    HasTime objInteraction;

    public Image clock;
    public int playerSlices = 6;

    void Start() {
        //    GameObject[] allObjects = FindObjectsOfType<GameObject>();
        //    foreach(GameObject go in allObjects)
        //        if(go.GetComponent<HasTime>()) { npcTimes.Add(go.GetComponent<HasTime>()); }
    }

    void Update() {
        HandleRayCast();
        hit = Physics2D.Raycast(playerToMouse.origin, playerToMouse.direction, distance, layer);
        if(hit) objInteraction = hit.transform.GetComponent<HasTime>();

        clock.fillAmount = Mathf.Lerp(clock.fillAmount, playerSlices / 12.0f, .2f);

        Debug.DrawLine(playerToMouse.origin, hit.point);
    }

    public void TakeTime() {
        if(objInteraction != null && hit.distance < 1) {
            
        }
    }

    public void GiveTime() {
        if(objInteraction != null && hit.distance < 1) {
            
        }
    }

    void HandleRayCast() {
        Ray mseRay = cameraInUse.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.back, transform.position);
        Vector3 hitPt;

        if(playerPlane.Raycast(mseRay, out distance)) {
            hitPt = mseRay.GetPoint(distance);
            playerToMouse = new Ray(transform.position, hitPt - transform.position);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(playerToMouse);
        Gizmos.DrawRay(playerToMouse.origin, playerToMouse.direction);
    }
}
