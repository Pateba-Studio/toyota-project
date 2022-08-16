using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PlayerData
{
    public string email;
    public string ticket;
}

public class JavascriptHook : MonoBehaviour
{
    public TextMeshProUGUI detailText;
    public PlayerData playerData;

    void Update()
    {
        detailText.text = $"{playerData.email}\n{playerData.ticket}";
    }

    public void PlayerEmailHandler(string email) {
        playerData.email = email;
    }

    public void PlayerTicketHandler(string ticket)
    {
        playerData.ticket = ticket;
    }
}
