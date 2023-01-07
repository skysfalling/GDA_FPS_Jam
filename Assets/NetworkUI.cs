using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class NetworkUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playerCountText;

    private NetworkVariable<int> playerCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            //GameController.Instance.ownedPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerController>();
            //GameController.Instance.ownedFormController = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<FormController>();
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //GameController.Instance.ownedPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerController>();
            //GameController.Instance.ownedFormController = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<FormController>();
        });
    }

    private void Update()
    {
        playerCountText.text = "Players: " + playerCount.Value;

        if (!IsServer)
        {
            return;
        }

        playerCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
        
    }
}
