using CesiumForUnity;
using UnityEngine;
using UnityEngine.Android;
using Unity.Mathematics;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using Unity.VisualScripting;

public class GPSManager : MonoBehaviour
{
    private bool hasLocationPermission = false;
    private double latitude, longitude, height;
    [SerializeField]
    private CesiumGeoreference cesiumGeoreference;
    [SerializeField]
    private CesiumGlobeAnchor cesiumDynmaicCamera;
    [SerializeField]
    private GameObject AROriginGameObject;
    [SerializeField]
    private GameObject testCube;


    void Start()
    {
        // Request location permission
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        else
        {
            hasLocationPermission = true;
            StartLocationService();
            Input.location.Start();
        }

        latitude = (double)37.52422922;
        longitude = (double)126.962961;
        height = (double)37.9192;

    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            StopLocationService();
        }
        else
        {
            StartLocationService();
        }
    }

    private void StartLocationService()
    {
        if (hasLocationPermission)
        {
            Input.location.Start();
            Input.compass.enabled = true;
        }
    }

    private void StopLocationService()
    {
        if (hasLocationPermission)
        {
            Input.location.Stop();
            Input.compass.enabled = false;
        }
    }

    public void InstantiateTestCube()
    {
        cesiumGeoreference.latitude = latitude;
        cesiumGeoreference.longitude = longitude;
        cesiumGeoreference.height = height;

        cesiumDynmaicCamera.latitude = latitude;
        cesiumDynmaicCamera.longitude = longitude;
        cesiumDynmaicCamera.height = height;

        Instantiate(testCube);


        Debug.Log(cesiumGeoreference.latitude + " Latitdue");
        Debug.Log(cesiumGeoreference.longitude + " longitude");
        Debug.Log(cesiumGeoreference.height + " height");
    }

    void Update()
    {
        if (hasLocationPermission && Input.location.isEnabledByUser)
        {
            // Get GPS coordinates
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            height = Input.location.lastData.altitude;

            // Do something with the coordinates
            Debug.Log("Latitude: " + latitude + ", Longitude: " + longitude + ", Altitude: " + height);
        }
    }
}