using UnityEngine;
using System.Collections;

public class CamAndStena : MonoBehaviour {

    private Camera mainCamera;
    private Vector3 spawnCam;

    public bool StenaLive = false;
    public float dist;

    public static float distance = 5.0f;

    void Start()
    {
        spawnCam = transform.localPosition;
        mainCamera.GetComponent<Camera>();
    }

    void Update()
    {
        transform.localPosition = spawnCam;
        checkWall();
        checkPlayer();
    }

    public void checkWall()
    {
        Vector3 point = transform.TransformDirection(Vector3.back);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, point, out hit))
        {
            if (hit.distance <= 2.0f && dist > 0.6)
            {
                StenaLive = true;
                transform.localPosition += new Vector3(0, 0, 1);
            }
        }
    }

    public void checkPlayer()
    {
        Vector3 point = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 2, point, out hit))
        {
            if (StenaLive == false)
            {
                spawnCam = transform.localPosition;
            }

            GameObject hitObject = hit.transform.gameObject;
            StatusPlayer player = hitObject.GetComponent<StatusPlayer>();
            if (player != null)
                dist = hit.distance;
        }
    }
}