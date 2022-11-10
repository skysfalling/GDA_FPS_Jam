using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitscan : MonoBehaviour
{
    public float damage = 1.0f;

    [SerializeField] private float hipSpread;
    [SerializeField] private float ADSspread;

    public Vector3 targetPosition;
    public bool hasTargetPosition;
    public LineRenderer lineRenderer;

    public virtual void Start()
    {
        Destroy(gameObject, 0.1f);
    }


    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        hasTargetPosition = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, position);
    }

    public virtual void OnImpact(Collision collision) {}




}
