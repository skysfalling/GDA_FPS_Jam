using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game_1_Core : MonoBehaviour
{
    // Game Stats
    public int totalTargets;
    public float timeBetweenTargets;

    // Trackers
    int TargetsLeft;
    int TargetsHit = 0;
    int BestTargetsHit;

    // Dependencies
    public TextMeshPro mainScoreTracker; // Prints Targets Hit / 20
    public TextMeshPro secondaryScoreTracker; // Prints Targets Left
    public Target_Air_Controller Target; // Air Target Prefab to create

    public GameObject StartButton;

    ShootableButton StartButtonCore;
    Target_Air_Controller currentTarget;
    LineRenderer lineRenderer;

    // Misc
    bool gameStarted = false;
    float lineWidth = 0f;
    string mainScoreText;
    string secondaryScoreText;
    Coroutine coroutine;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        StartButtonCore = StartButton.GetComponent<ShootableButton>();

        TargetsLeft = totalTargets;
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

        // Start Targets Timer
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(missTimer());
        
    }

    public void startGame()
    {
        gameStarted = true;

        // Reset trackers
        TargetsLeft = totalTargets;
        TargetsHit = 0;

        // Set our text panels up
        mainScoreText = mainScoreTracker.text;
        secondaryScoreText = secondaryScoreTracker.text;

        // Update our text
        mainScoreTracker.text = TargetsHit.ToString() + "/" + totalTargets.ToString();
        mainScoreTracker.fontSize = 14.56f;
        secondaryScoreTracker.text = TargetsLeft.ToString() + " Left";

        // Disable Start Button
        StartButton.SetActive(false);

        // Spawn first target
        spawnTarget();
    }

    public void endGame()
    {
        gameStarted = false;

        if(BestTargetsHit < TargetsHit)
        {
            BestTargetsHit = TargetsHit;
        }

        // Update our text
        mainScoreTracker.text = "Last " + TargetsHit.ToString() + "/" + totalTargets.ToString() + "\n" +
                                "Best " + BestTargetsHit.ToString() + "/" + totalTargets.ToString();
        mainScoreTracker.fontSize = 7.38f;
        secondaryScoreTracker.text = secondaryScoreText;

        // Stop Targets Timer
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        StartButtonCore.OnHitEffect();

        // Enable Start Button
        StartButton.SetActive(true);
    }

    public void targetHit()
    {
        // Process hit
        TargetsLeft -= 1;
        TargetsHit += 1;
        currentTarget.DestoryTarget();

        // Check for end condition
        if (TargetsLeft <= 0)
        {
            endGame();
            return;
        }

        spawnTarget();
        
        // Update Text
        mainScoreTracker.text = TargetsHit.ToString() + "/" + totalTargets.ToString();
        secondaryScoreTracker.text = TargetsLeft.ToString() + " Left";
    }

    private IEnumerator missTimer()
    {
        yield return new WaitForSeconds(timeBetweenTargets);

        // Process hit
        currentTarget.DestoryTarget();
        TargetsLeft -= 1;

        // Check for end condition
        if (TargetsLeft <= 0)
        {
            endGame();
        }
        else
        { 
            // Otherwise, continue and spawn target
            spawnTarget();

            // Update Text
            mainScoreTracker.text = TargetsHit.ToString() + "/" + totalTargets.ToString();
            secondaryScoreTracker.text = TargetsLeft.ToString() + " Left";
        }
    }
}
