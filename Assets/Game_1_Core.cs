using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game_1_Core : MonoBehaviour
{
    // Game Stats
    int speed = 0;
    int TargetsLeft = 20;
    int TargetsHit = 0;

    // Dependencies
    public TextMeshPro mainScoreTracker; // Prints Targets Hit / 20
    public TextMeshPro secondaryScoreTracker; // Prints Targets Left
    public Target_Air_Controller Target; // Air Target Prefab to create

    public GameObject StartButton;

    Target_Air_Controller currentTarget;
    LineRenderer lineRenderer;

    // Misc
    bool gameStarted = false;
    float lineWidth = 0f;
    string mainScoreText;
    string secondaryScoreText;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Slowly grow laser grid
        if (gameStarted && lineWidth < 0.25f)
        {
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineWidth += 0.05f;
        }
    }

    private void spawnTarget()
    {
        // Spawn a target in a cone -40 to 40 degrees away from panel, facing edge.
        Vector3 spawnPos = new Vector3(0f,2f,0f) + transform.position + Quaternion.Euler(0f, Random.Range(-40f, 40f), 0f) * Vector3.right * 15;

        // Face Target towards this object
        Quaternion spawnRot = Quaternion.LookRotation((transform.position - spawnPos).normalized) * Quaternion.Euler(0,90,0);

        currentTarget = Instantiate(Target, spawnPos, spawnRot);

        // Add Unity Event Listener to Recieve Callback from target hit
        // This allows us to increment targets hit and create our next target, once the last is hits
        currentTarget.RunOnHit.AddListener(targetHit);
    }

    public void startGame()
    {
        gameStarted = true;

        // Set our text panels up
        mainScoreText = mainScoreTracker.text;
        secondaryScoreText = secondaryScoreTracker.text;

        mainScoreTracker.text = TargetsHit.ToString() + "/20";
        secondaryScoreTracker.text = TargetsLeft.ToString() + " Left";

        // Disable Start Button
        StartButton.SetActive(false);

        // Spawn first target
        spawnTarget();
    }

    public void endGame()
    {
        gameStarted = false;
        mainScoreTracker.text = mainScoreText;
        secondaryScoreTracker.text = secondaryScoreText;

        // Enable Start Button
        StartButton.SetActive(true);
    }

    public void targetHit()
    {
        TargetsHit += 1;
        currentTarget.DestoryTarget();
        spawnTarget();
        TargetsLeft -= 1;

        // Update Text
        mainScoreTracker.text = TargetsHit.ToString() + "/20";
        secondaryScoreTracker.text = TargetsLeft.ToString() + " Left";
    }

}
