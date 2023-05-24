using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Name : MonoBehaviour
{
    public InputField playerName;

    void Update()
    {
        if (playerName.text.Length > 0) 
        {
            MainManager.Instance.PlayerName = playerName.text;
        }
    }
}
