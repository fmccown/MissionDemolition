using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Singleton

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    public GameObject poi
    {
        get
        {
            return _poi;
        }

        set
        {
            _poi = value;
            if (_poi != null)
            {
                // When set to something new, reset everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }

    private void Awake()
    {
        S = this;

        // Disable line until it's needed
        line = GetComponent<LineRenderer>();
        line.enabled = false;

        points = new List<Vector3>();
    }
    
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        // Add a point to a line
        Vector3 pt = _poi.transform.position;

        // Make sure new point is far enough away from last point
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }

        if (points.Count == 0)
        {
            // Add a little more line to aid in aiming
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;          
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
        }
        else
        {
            // Away from slingshot
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
        }

        line.enabled = true;
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            if (FollowCam.POI != null && FollowCam.POI.tag == "Projectile")
            {
                poi = FollowCam.POI;
            }
            else
            {
                return;
            }
        }

        // Attempt to add this new location
        AddPoint();

        if (FollowCam.POI == null)
        {
            // Make local poi null too
            poi = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
