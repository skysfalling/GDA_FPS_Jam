using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_1_Core : MonoBehaviour
{
    bool gameStarted = false;
    int  speed = 0;
    public int TargetsLeft = 20;
    public int TargetsHit = 0;
    Target_Air_Controller currentTarget;
    LineRenderer lineRenderer;
    float lineWidth = 0f;

    public Target_Air_Controller Target;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (gameStarted && lineWidth < 0.25f)
        {
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineWidth += 0.05f;
        }
    }

    private void spawnTarget()
    {
        Vector3 spawnPos = new Vector3(0f,2f,0f) + transform.position + Quaternion.Euler(0f, Random.Range(-40f, 45f), 0f) * Vector3.right * 15;
        Quaternion spawnRot = Quaternion.LookRotation((transform.position - spawnPos).normalized) * Quaternion.Euler(0,90,0);
        currentTarget = Instantiate(Target, spawnPos, spawnRot);
        currentTarget.RunOnHit.AddListener(targetHit);
    }

    public void startGame()
    {
        gameStarted = true;
        spawnTarget();
    }

    public void endGame()
    {
        gameStarted = false;
    }

    public void targetHit()
    {
        TargetsHit += 1;
        currentTarget.DestoryTarget();
        spawnTarget();
        TargetsLeft -= 1;
    }

}
