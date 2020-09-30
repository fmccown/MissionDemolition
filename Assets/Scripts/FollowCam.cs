using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set In Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;


    private void Awake()
    {
        camZ = transform.position.z;
    }

    // FixedUpdate() is in sync with physics engine. Called 50 times/second
    private void FixedUpdate()
    {
        if (POI == null) return;

        Vector3 dest = POI.transform.position;

        // Limit x and y to minimum values
        dest.x = Mathf.Max(minXY.x, dest.x);
        dest.y = Mathf.Max(minXY.y, dest.y);

        // Interpolate from the current Camera position toward destination
        dest = Vector3.Lerp(transform.position, dest, easing);
        
        // Force destination.z to keep camera far enough away
        dest.z = camZ;

        // Move camera to destination
        transform.position = dest;

        // Keep ground in view
        Camera.main.orthographicSize = dest.y + 10;

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
