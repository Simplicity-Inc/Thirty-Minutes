using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlatformController))]
public class PlatformerControllerGizmo : MonoBehaviour {

    PlatformController m_PlatformController;

    Vector3[] waypoints;
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    void OnDrawGizmos() {
        SetupReferences();
        DrawGizmo();
    }

    void DrawGizmo() {
        Vector3 position = CalculateCurrentGizmoPosition();
        Debug.Log(position.ToString());
        Bounds bounds = GetComponent<Collider2D>().bounds;

        Gizmos.color = new Color(1, 0, 0, 1);
        Gizmos.DrawWireCube(position, bounds.size);
        Gizmos.color = new Color(1, 0, 0, .3f);
        Gizmos.DrawCube(position, bounds.size);
    }

    void SetupReferences() {
        if(m_PlatformController == null)
            m_PlatformController = GetComponent<PlatformController>();

        waypoints = new Vector3[m_PlatformController.localWaypoints.Length];
        for(int i = 0; i < m_PlatformController.localWaypoints.Length; i++) {
            waypoints[i] = m_PlatformController.localWaypoints[i] + transform.position;
        }
    }

    float Ease(float x) {
        float a = m_PlatformController.easeAmount + 1;
        return Mathf.Pow(x, a) / ( Mathf.Pow(x, a) + Mathf.Pow(1 - x, a) );
    }

    Vector3 CalculateCurrentGizmoPosition() {
        if(PreviewTime.Time < nextMoveTime) { return Vector3.zero; }

        fromWaypointIndex %= waypoints.Length;
        int toWaypointIndex = ( fromWaypointIndex + 1 ) % waypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(waypoints[fromWaypointIndex], waypoints[toWaypointIndex]);
        percentBetweenWaypoints += PreviewTime.Time * m_PlatformController.speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(waypoints[fromWaypointIndex], waypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if(percentBetweenWaypoints >= 1) {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if(!m_PlatformController.cyclic) {
                if(fromWaypointIndex >= waypoints.Length - 1) {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(waypoints);
                }
            }
            nextMoveTime = PreviewTime.Time + m_PlatformController.waitTime;
        }

        return newPos - transform.position;
    }
}
