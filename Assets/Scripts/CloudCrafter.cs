﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set In Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    private void Awake()
    {
        // Make array large enough to hold all cloud instances
        cloudInstances = new GameObject[numClouds];

        GameObject anchor = GameObject.Find("CloudAnchor");

        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab);

            // Position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            // Scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            // Smaller clouds should be nearer the ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

            // Smaller clouds should be further away
            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            // Make cloud a child of the anchor
            cloud.transform.SetParent(anchor.transform);

            cloudInstances[i] = cloud;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cloud in cloudInstances)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            // Move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            // If a cloud has moved too far left, move to the far right
            if (cPos.x <= cloudPosMin.x)
            {
                cPos.x = cloudPosMax.x;
            }

            cloud.transform.position = cPos;
        }
    }
}
