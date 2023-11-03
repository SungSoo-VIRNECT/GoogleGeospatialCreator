using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using Unity.Splines.Examples;
using UnityEngine;
using UnityEngine.AI;

public class YongSanPlayer : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    private NavMeshPath path;
    public LineRenderer lineRenderer;

    public TextMeshProUGUI distanceLeft;
    void Update()
    {
        Debug.Log("Update");
        path = new NavMeshPath();
        if (NavMesh.CalculatePath(startPoint.position, endPoint.position, NavMesh.AllAreas, path))
        {
            float distance = GetPathLength(path);
            Debug.Log("Distance on NavMesh: " + distance);
            distanceLeft.text = distance + " 남음";
            DrawPath(path);
        }
        else
        {
            Debug.LogWarning("No valid path found.");
        }
    }

    private float GetPathLength(NavMeshPath path)
    {
        float pathLength = 0.0f;

        for (int i = 1; i < path.corners.Length; i++)
        {
            pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return pathLength;
    }

    // Draw the path using a LineRenderer
    private void DrawPath(NavMeshPath path)
    {
        lineRenderer.positionCount = path.corners.Length;
        lineRenderer.SetPositions(path.corners);
        lineRenderer.enabled = true;
    }
}
