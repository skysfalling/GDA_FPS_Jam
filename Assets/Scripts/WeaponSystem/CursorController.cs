using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : UnitySingleton<CursorController>
{
    public LayerMask cursorDetectionLayer;
    public Vector3 cursorWorldPosition;

    // Update is called once per frame
    void Update()
    {
        cursorWorldPosition = mouseClickWorldPosition();
    }

    Vector3 mouseClickWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cursorDetectionLayer))
        {
            return hit.point;

        }

        return Vector3.zero;
    }
}
