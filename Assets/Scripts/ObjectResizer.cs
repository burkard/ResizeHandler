using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectResizer : MonoBehaviour
{
    public bool isBeingResized;
    private Bounds bounds;
    public float lineWidth = .1f;
    public float handlerSize = 0.2f;

    private int segments = 360;
    private int pointCount;
    private Vector3[] points;
    private float zPosition = 0.0f;
    private float radius;
    private LineRenderer line;

    private GameObject handler, raycastPlane;
    private float initialScale;
    private Quaternion initialRotation;

    void Start()
    {
        bounds = this.gameObject.GetComponent<Renderer>().bounds;
        isBeingResized = false;
        pointCount = segments + 1;
        initialScale = transform.localScale.x;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (isBeingResized)
        {
            DrawCircle();
            ResizeObject();
            RotateObject();
        }
    }

    private void ResizeObject()
    {
        if (handler.GetComponent<ResizeHandler>().Scale() > 0)
            transform.localScale = Vector3.one * initialScale * handler.GetComponent<ResizeHandler>().Scale();
    }

    private void RotateObject()
    {
        transform.rotation = initialRotation;
        float rotAngle = handler.GetComponent<ResizeHandler>().Angle();
        transform.Rotate(Camera.main.transform.forward, rotAngle, Space.World);
    }


    private void OnMouseUp()
    {
        isBeingResized = !isBeingResized;
        if (isBeingResized) AddLineRenderer();
        else RemoveLineRenderer();
    }

    private void AddLineRenderer()
    {
        if (line == null)
        {
            points = new Vector3[pointCount];
            this.gameObject.AddComponent<LineRenderer>();
            line = this.gameObject.GetComponent<LineRenderer>();
            line.startWidth = lineWidth; line.endWidth = lineWidth;
            line.positionCount = pointCount;
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = Color.blue; line.endColor = Color.blue;
            
            handler = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            handler.transform.localScale = new Vector3(handlerSize, handlerSize, handlerSize);
            handler.gameObject.AddComponent<ResizeHandler>();
            handler.GetComponent<ResizeHandler>().parentCenter = transform.position;

            Vector3 cameraDirection = transform.position - Camera.main.transform.position;
            radius = bounds.extents.magnitude * this.transform.localScale.x * 1.2f;
            float rad = Mathf.Deg2Rad * (360f / segments);
            Vector3 handlerPos = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, zPosition) + this.transform.position;
            handlerPos = RotatePointAroundPivot(handlerPos, transform.position, cameraDirection);
            handler.transform.position = handlerPos;

            raycastPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Destroy(raycastPlane.GetComponent<MeshRenderer>());
            raycastPlane.transform.LookAt(Camera.main.transform);
            raycastPlane.transform.Rotate(new Vector3(90, 0, 0));
            raycastPlane.transform.localScale = new Vector3(1000f, 1000f, 1000f);
            raycastPlane.layer = 5;
        }
    }

    private void RemoveLineRenderer()
    {
        if (this.gameObject.GetComponent<LineRenderer>() != null)
        {
            Destroy(this.gameObject.GetComponent<LineRenderer>());
            points = null;
            Destroy(handler.gameObject);
            Destroy(raycastPlane.gameObject);
            initialScale = transform.localScale.x;
            initialRotation = transform.rotation;
        }
    }

    void DrawCircle()
    {
        Vector3 cameraDirection = transform.position - Camera.main.transform.position;
            
        radius = bounds.extents.magnitude * this.transform.localScale.x * 1.2f;
        for (int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, zPosition) + this.transform.position;
            points[i] = RotatePointAroundPivot(points[i], transform.position, cameraDirection);
        }

        line.SetPositions(points);
        line.alignment = LineAlignment.View;
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles){
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.LookRotation(angles, Vector3.up) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    public void StopResizing()
    {
        isBeingResized = false;
        RemoveLineRenderer();
    }
}
