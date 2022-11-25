using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Timer : MonoBehaviourPun
{
    float currentTime;
    public float startingTime = 30f;

    [SerializeField] TextMeshProUGUI countdownTimer;
     void Start()
    {
        countdownTimer.text = startingTime.ToString();
        currentTime = startingTime;
     }

     void Update()
    {
        startingTime -= Time.deltaTime;
        countdownTimer.text = Mathf.Round(startingTime).ToString();

        if(startingTime < 0)
        {
            startingTime = 0;
        }
    }

}
