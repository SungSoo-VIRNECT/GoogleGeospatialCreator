using CesiumForUnity;
using Mono.CSharp;
using QFSW.QC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGPSLocation : MonoBehaviour
{
    bool locationServiceStarted;
    private double latitude;
    private double longitude;
    private double altitude;


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


        void OnDestroy()
        {
            // Stop the location service if it was started
            if (locationServiceStarted)
            {
                Input.location.Stop();
            }
        }

        Test();
    }

    //optimized C# variant (distance in km, without variables and redundant calculations, very close to mathematical expression of Haversine Formular
    //구글맵 대비 오차가 거의 없음
    public static double CalulcateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double rad(double angle) => angle * 0.017453292519943295769236907684886127d; // = angle * Math.Pi / 180.0d
        double havf(double diff) => Math.Pow(Math.Sin(rad(diff) / 2d), 2); // = sin²(diff / 2)
        return 12745.6 * Math.Asin(Math.Sqrt(havf(lat2 - lat1) + Math.Cos(rad(lat1)) * Math.Cos(rad(lat2)) * havf(lon2 - lon1))); // earth radius 6.372,8‬km x 2 = 12745.6
    }

    //original code from .NET-Frameworks GeoCoordinate class, refactored into a standalone method:
    //구글맵 대비 오차가 조금 있음 실제값)28.99km 측정값)28.56km
    public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
    {
        var d1 = latitude * (Math.PI / 180.0);
        var num1 = longitude * (Math.PI / 180.0);
        var d2 = otherLatitude * (Math.PI / 180.0);
        var num2 = otherLongitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }

    [Command]
    public void Test()
    {
        var distance = CalulcateDistance(37.524244233827, 126.96283408707, 37.709744831603, 126.73157062738);
        Debug.Log(distance.ToString());

        var distance2 = GetDistance(37.524244233827, 126.96283408707, 37.709744831603, 126.73157062738);
        Debug.Log(distance2.ToString());
    }



}