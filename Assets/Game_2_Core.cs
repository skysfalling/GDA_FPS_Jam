using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_2_Core : MonoBehaviour
{
    // Dependencies
    public GameObject MainTarget;

    // Trackers
    ShootableButton currentButton;

    public void SetTargetDistance(float distance)
    {
        // Deselect past button
        if (currentButton != null)
        {
            currentButton.DisableButton();
        }

        // Get old local position and update y
        Vector3 newPosition = MainTarget.transform.localPosition;
        newPosition.x = -distance - 15;

        // Set Position
        MainTarget.transform.localPosition = newPosition;
    }

    public void SetCurrentButton(ShootableButton button)
    {
        currentButton = button;
    }
}
