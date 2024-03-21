using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererAnimation : MonoBehaviour
{
    /*
    public float speed = 10f;
    public bool isThrowing;

    private LineRenderer lineRenderer;
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private Vector3 controlPoint;
    private float startTime;
    private float journeyLength;

    private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            startPoint = transform.position;
            isThrowing = false;
        }

        private void Update()
        {
        if (isThrowing)
        {
            AnimateWaterHook();
        }
        else
        {
            if (lineRenderer.positionCount != 0)
            {
                lineRenderer.positionCount = 0;
            }
        }
    }

        public void ThrowWaterHook(Vector3 target)
        {
            targetPoint = target;
            startPoint = transform.position;
            controlPoint = (startPoint + targetPoint) * 0.5f + Vector3.up * Vector3.Distance(startPoint, targetPoint) * 0.5f;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPoint, targetPoint);
            isThrowing = true;
    }

    private void AnimateWaterHook()
    {
        startPoint = transform.position;
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        Vector3 currentPosition = CalculateBezierPoint(fracJourney, startPoint, controlPoint, targetPoint);

        int segments = 20;
        lineRenderer.positionCount = segments + 1;
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / (float)segments;
            Vector3 point = CalculateBezierPoint(t, startPoint, controlPoint, targetPoint);
            lineRenderer.SetPosition(i, point);
        }

        if (fracJourney >= 1)
        {
            isThrowing = false;
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 point = uu * p0 + 2 * u * t * p1 + tt * p2;
        return point;
    }
}*/ 
    // Above is 1 Hook variation and the First Hook System.

    public float speed = 10f;
    public bool isThrowing;

    public int numberOfLines = 3;
    public float lineWidth = 0.2f;

    private LineRenderer[] lineRenderers;
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private Vector3[] controlPoints;
    private float startTime;
    private float journeyLength;
    public Material waterShader;


    [SerializeField][Range(0f, 5f)] private float sideCurveFactor = 1f;
    [SerializeField][Range(0f, 5f)] private float verticalCurveFactor = 1f;

    private void Start()
    {
        lineRenderers = new LineRenderer[numberOfLines];
        controlPoints = new Vector3[numberOfLines];

        for (int i = 0; i < numberOfLines; i++)
        {
            GameObject lineObject = new GameObject("LineRenderer_" + i);
            lineObject.transform.parent = transform;

            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.material = waterShader;
            //lineRenderer.material.color = Color.blue;

            lineRenderers[i] = lineRenderer;
        }

        startPoint = transform.position;
        isThrowing = false;
    }

    private void Update()
    {
        if (isThrowing)
        {
            AnimateWaterHook();
        }
        else
        {
            for (int i = 0; i < numberOfLines; i++)
            {
                if (lineRenderers[i].positionCount != 0)
                {
                    lineRenderers[i].positionCount = 0;
                }
            }
        }
    }

    public void ThrowWaterHook(Vector3 target)
    {

        targetPoint = target;
        startPoint = transform.position;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPoint, targetPoint);
        isThrowing = true;

        // Set the numberOfLines to 1
        numberOfLines = 1;
        controlPoints = new Vector3[numberOfLines];

        // Calculate the control point position
        float controlPointPositionFactor = Random.Range(0.3f, 0.7f);
        Vector3 controlPointPosition = Vector3.Lerp(startPoint, targetPoint, controlPointPositionFactor);

        // Calculate the curve influence based on the controlPointPositionFactor
        float curveInfluence = Mathf.Pow(1 - controlPointPositionFactor, 3);
        float height = Vector3.Distance(startPoint, targetPoint) * (controlPointPositionFactor + curveInfluence);

        // Generate a random sideways direction
        Vector3 randomDirection = Vector3.Cross(target - startPoint, Vector3.up).normalized;
        float randomSidewaysOffset = Random.Range(-2f, 2f) * sideCurveFactor;

        // Set the control point with the calculated position, height, and sideways offset
        Vector3 midPoint = (startPoint + targetPoint) / 2;
        Vector3 upwardOffset = Vector3.up * height * verticalCurveFactor;
        Vector3 sidewaysOffset = randomDirection * randomSidewaysOffset;
        controlPoints[0] = Vector3.Slerp(midPoint + upwardOffset, midPoint + sidewaysOffset, controlPointPositionFactor);
        
    }



    private void AnimateWaterHook()
    {
        startPoint = transform.position;
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;

        int segments = 20;
        for (int i = 0; i < numberOfLines; i++)
        {
            lineRenderers[i].positionCount = segments + 1;
            for (int j = 0; j <= segments; j++)
            {
                float t = (float)j / (float)segments;
                Vector3 point = CalculateBezierPoint(t, startPoint, controlPoints[i], targetPoint);
                lineRenderers[i].SetPosition(j, point);
            }
        }

        if (fracJourney >= 1)
        {
            isThrowing = false;
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 point = uu * p0 + 2 * u * t * p1 + tt * p2;
        return point;
    }
}