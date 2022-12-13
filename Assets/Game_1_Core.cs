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

    public Target_Air_Controller Target;


    void Update()
    {
        if (gameStarted)
        {

        }
    }

    private void spawnTarget()
    {
        currentTarget = Instantiate(Target, new Vector3(Random.Range(0,10), 2, Random.Range(0, 10)), Quaternion.identity);
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
