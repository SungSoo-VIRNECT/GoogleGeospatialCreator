﻿using Google.XR.ARCoreExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TextManager : MonoBehaviour
{
    /// <summary>
    /// The AREarthManager used in the sample.
    /// </summary>
    public AREarthManager EarthManager;
    /// <summary>
    /// Accuracy threshold for altitude and longitude that can be treated as localization
    /// completed.
    /// </summary>
    private const double _horizontalAccuracyThreshold = 20;
    /// <summary>
    /// Accuracy threshold for orientation yaw accuracy in degrees that can be treated as
    /// localization completed.
    /// </summary>
    private const double _orientationYawAccuracyThreshold = 25;
    private bool _isLocalizing = false;
    [SerializeField] private GameObject spinnerText;
    [SerializeField] private GameObject timeLeftText;
    [SerializeField] private GameObject navigationArrow;

    private void OnEnable()
    {
        _isLocalizing = true;
        timeLeftText.SetActive(false);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

    }

    void Update()
    {
        bool isSessionReady = ARSession.state == ARSessionState.SessionTracking &&
                Input.location.status == LocationServiceStatus.Running;
        var earthTrackingState = EarthManager.EarthTrackingState;
        var pose = earthTrackingState == TrackingState.Tracking ?
            EarthManager.CameraGeospatialPose : new GeospatialPose();
        if (!isSessionReady || earthTrackingState != TrackingState.Tracking ||
            pose.OrientationYawAccuracy > _orientationYawAccuracyThreshold ||
            pose.HorizontalAccuracy > _horizontalAccuracyThreshold)
        {
            //로컬라제이션 실패
            if (!_isLocalizing)
            {
                _isLocalizing = true;
                //스피너 + 경로탐색중 오브젝트 켜기
                //디폴트
                spinnerText.SetActive(true);
                timeLeftText.SetActive(false);
                navigationArrow.SetActive(false);
            }


            //"Point your camera at buildings, stores, and signs near you.";

        }
        else if (_isLocalizing)
        {
            // Finished localization.
            
            _isLocalizing = false;
            spinnerText.SetActive(false);
            timeLeftText.SetActive(true); //소요시간
            navigationArrow.SetActive(true); 


        }

        CalculateRemainingTime();
    }

    public void CalculateRemainingTime()
    {
        // Assume walking speed is 5 kilometers per hour
        double walkingSpeedKmph = 4.0;

        // Get the remaining distance (in meters)
        double remainingDistanceKilometers = DistanceMeasurement.distanceKilometers;

        // Calculate the time in hours
        double timeHours = remainingDistanceKilometers / (walkingSpeedKmph);

        // Convert hours to minutes and seconds
        int hours = (int)timeHours;
        int minutes = (int)((timeHours - hours) * 60);

        // Format the time as a string

        if (hours == 0 && minutes < 2)
        {
            timeLeftText.GetComponent<TextMeshProUGUI>().text = "곧 도착 예상";
        }
        else if(hours == 0)
        {
            timeLeftText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}분 소요예상", minutes);
        }
        else
        {
            timeLeftText.GetComponent<TextMeshProUGUI>().text = string.Format("{0}시간 {1}분 소요예상", hours, minutes);
        }
    }
}
