using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeightManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] planeList;
    [SerializeField]
    private int heightNumber = 2;


    private void Start()
    {
    
    }

    public void RaiseHeight()
    {
        foreach(var plane in planeList)
        {
            plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y + heightNumber, plane.transform.position.z);

        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("test");
    }


    public void LowerHeight()
    {
        foreach (var plane in planeList)
        {
            plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y - heightNumber, plane.transform.position.z);

        }
    }
}
