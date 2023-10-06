using CesiumForUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLocation : MonoBehaviour
{
    private bool locationServiceStarted = false;
    private float latitude;
    private float longitude;
    private float altitude;
    public CesiumGlobeAnchor cesiumGlobeAnchor;

    IEnumerator Start()
    {
        // Check if GPS is enabled on the device
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS is not enabled on the device.");
            yield break;
        }

        // Start service and retrieve location data
        Input.location.Start();
        locationServiceStarted = true;

        // Wait until location service initializes
        int maxWaitTime = 20; // Maximum time to wait for initialization in seconds
        while (Input.location.status == LocationServiceStatus.Initializing && maxWaitTime > 0)
        {
            yield return new WaitForSeconds(1);
            maxWaitTime--;
        }

        // If the location service didn't initialize within the maximum wait time
        if (maxWaitTime <= 0)
        {
            Debug.Log("GPS initialization timed out.");
            yield break;
        }

        // If the location service failed to initialize
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("GPS initialization failed.");
            yield break;
        }

        // Retrieve the latitude and longitude
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        altitude = Input.location.lastData.altitude;
        // Print the location information
        Debug.Log("Latitude: " + latitude);
        Debug.Log("Longitude: " + longitude);
        Debug.Log("Longitude: " + altitude);
    }

    private void Update()
    {
        cesiumGlobeAnchor.SetPositionEarthCenteredEarthFixed(latitude, longitude, altitude);
    }

    void OnDestroy()
    {
        // Stop the location service if it was started
        if (locationServiceStarted)
        {
            Input.location.Stop();
        }
    }
}   
