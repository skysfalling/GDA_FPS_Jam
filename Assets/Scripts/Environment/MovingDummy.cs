using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingDummy : TargetDamageDummy
{
    [Header("Move Targets")]
    public Vector3 targetA;
    public Vector3 targetB;

    Vector3 currentTarget;

    public override void Start()
    {
        base.Start();
       
        // Choose furthest target to goto first
        if ((transform.position - targetA).magnitude > (transform.position - targetB).magnitude)
        {
            currentTarget = targetA;
        }
        else
        {
            currentTarget = targetB;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, 5 * Time.deltaTime);

        // If in range swap target
        if ((transform.position - currentTarget).magnitude < 0.001f)
        {
            if(currentTarget == targetA)
            {
                currentTarget = targetB;
            }
            else
            {
                currentTarget = targetA;
            }
        }
    }
}
