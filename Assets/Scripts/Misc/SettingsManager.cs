using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    bool settingsOpen = false;
    float oldTimeScale;

    public PlayerController player;
    public GameObject settingsUICanvas;

    private void Start()
    {
        settingsUICanvas.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            settingsOpen = !settingsOpen;

            if (settingsOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                oldTimeScale = Time.timeScale;
                Time.timeScale = 0;
                player.canControlMovement = false;
                Cursor.visible = true;

                settingsUICanvas.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = oldTimeScale;
                player.canControlMovement = true;
                Cursor.visible = false;

                settingsUICanvas.SetActive(false);
            }
        }
    }
}
