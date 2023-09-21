using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Sensors' (是否遺漏了組件參考?)
using Unity.MLAgents.Sensors;
#pragma warning restore CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Sensors' (是否遺漏了組件參考?)

public class LidarSensor : MonoBehaviour
{
    [SerializeField] [Range(5f,30f)] private float MaxLength = 18.0f;
    [SerializeField] [Range(0.01f,5f)] private float resolution = 1f;
    [SerializeField] [Range(30, 360)] private int scanArea = 360;

    private List<float> LidarDistanceArray;

    private void Awake()
    {
        // LidarDistanceArray = new List<float>();
        // float scanArea = Convert.ToSingle(this.scanArea);
        // float resolution = Convert.ToSingle(this.resolution);
    }
    
    private void FixedUpdate()
    {
        LidarScan();
        Debug.Log("Count of LidarArray : " + LidarDistanceArray.Count);
    }

    private void LidarScan()
    {
        List<float> lidarArray = new List<float>();

        for (float i = 0.0f; i < 360; i+=resolution)
        {
            transform.localEulerAngles = new Vector3(0f, i, 0f);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit raycastHit;
            if (scanArea == 360)
            {
                if (Physics.Raycast(ray, out raycastHit, MaxLength)){
                    Debug.DrawLine(transform.position, raycastHit.point, Color.yellow);
                    lidarArray.Add(raycastHit.distance);
                }
                else
                {
                    Vector3 endPosition = transform.position + transform.forward * MaxLength;
                    Debug.DrawLine(transform.position, endPosition, Color.white);
                    lidarArray.Add(MaxLength);
                }
            }
            else
            {
                int halfDeg = scanArea / 2;
                int negativeAreaStartDeg = 360 - halfDeg;

                if (i < halfDeg || i >= negativeAreaStartDeg)
                {
                    if (Physics.Raycast(ray, out raycastHit, MaxLength)){
                        Debug.DrawLine(transform.position, raycastHit.point, Color.yellow);
                        lidarArray.Add(raycastHit.distance);
                    }
                    else
                    {
                        Vector3 endPosition = transform.position + transform.forward * MaxLength;
                        Debug.DrawLine(transform.position, endPosition, Color.white);
                        lidarArray.Add(MaxLength);
                    }
                }
            }


        }

        LidarDistanceArray = lidarArray;

    }

    public List<float> getLidar()
    {
        return LidarDistanceArray;
    }
}
