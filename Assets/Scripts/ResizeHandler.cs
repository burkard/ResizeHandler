using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeHandler : MonoBehaviour
{
    public Vector3 parentCenter = Vector3.zero;
    private Camera mainCam;
    private float startDistance, currentScale, currentAngle;
    private Vector3 startPosition;

    private void Start()
    {
        mainCam = Camera.main;
        startDistance = (transform.position - parentCenter).magnitude;
        currentScale = (transform.position - parentCenter).magnitude / startDistance;
        startPosition = transform.position;
        
        startPosition = transform.position;
    }
    
    private void OnMouseDrag()
    {
        Ray castPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("UI");
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, mask))
        {
            transform.position = hit.point;
        }
        currentScale = (transform.position - parentCenter).magnitude / startDistance;
        currentAngle = CalculateAngle();
    }

    public float Scale()
    {
        return currentScale;
    }
    public float Angle()
    {
        return currentAngle;
    }

    private float CalculateAngle()
    {
        Vector3 startVector = startPosition - parentCenter;
        Vector3 endVector = transform.position - parentCenter;
        float angle = Vector3.Angle(startVector, endVector);
        
        float sign = Mathf.Sign(Vector3.Dot(mainCam.transform.forward, Vector3.Cross(startVector, endVector)));
        return angle * sign;
    }

   
}
