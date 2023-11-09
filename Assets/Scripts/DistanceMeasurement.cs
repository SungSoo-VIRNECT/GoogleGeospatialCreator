using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DistanceMeasurement : MonoBehaviour
{
    public AREarthManager EarthManager;
    public ARSessionOrigin arSessionOrigin;
    public Transform targetObject; // The object you want to measure the distance to.
    public TextMeshProUGUI distanceText;
    public ARGeospatialCreatorAnchor arGeospatialCreatorAnchor;
    public GameObject infoImage;
    public static double distanceKilometers;
    private double lat;
    private double lon;
    private double deviceLat;
    private double deviceLon;

    private void Start()
    {
        InvokeRepeating("GetLocationData", 3, 5);
    }


    public void GetLocationData()
    {
        lat = arGeospatialCreatorAnchor.Latitude;
        lon = arGeospatialCreatorAnchor.Longitude;
        Debug.Log("구조 위치" + lat + " " + lon);
        deviceLat = EarthManager.CameraGeospatialPose.Latitude;
        deviceLon = EarthManager.CameraGeospatialPose.Longitude;
        Debug.Log($"나의 위치 {deviceLat} {deviceLon}");

        distanceKilometers = CalculateDistance(lat, lon, deviceLat, deviceLon);
        Debug.Log("distance " +distanceKilometers);

        float trimmedValue = (float)Math.Round(distanceKilometers, 1); 
        if (trimmedValue >= 1)
        {
            // Display distance in kilometers
            double distanceInKilometers = trimmedValue;
            distanceText.text = distanceInKilometers.ToString("0.0") + "KM 남음";
        }
        else
        {
            // Display distance in meters
            trimmedValue = (int)(distanceKilometers * 1000);
            distanceText.text = (trimmedValue).ToString() + "M 남음";
        }
        Debug.Log("Distance to target object: " + distanceKilometers);

        if(distanceKilometers * 1000 <= 10)
        {
            infoImage.SetActive(true);
        }
    }


    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double rad(double angle) => angle * 0.017453292519943295769236907684886127d; // = angle * Math.Pi / 180.0d
        double havf(double diff) => Math.Pow(Math.Sin(rad(diff) / 2d), 2); // = sin²(diff / 2)       
        double distanceInKilometers = 12745.6 * Math.Asin(Math.Sqrt(havf(lat2 - lat1) + Math.Cos(rad(lat1)) * Math.Cos(rad(lat2)) * havf(lon2 - lon1)));
        int distanceInMeters = (int)(distanceInKilometers * 1000);
        Debug.Log(distanceInKilometers + " " + distanceInMeters);
        return distanceInKilometers;
    }
}
