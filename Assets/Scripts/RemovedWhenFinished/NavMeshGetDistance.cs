using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGetDistance : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public LineRenderer lineRenderer;
    public TextMeshProUGUI distanceLeft;

    private NavMeshPath path;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
    }

    // Calculate the length of the path
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