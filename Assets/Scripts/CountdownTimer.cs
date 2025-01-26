using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 90; //Set the total time for the countdown
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (totalTime > 0)
        {
            // Subtract elapsed time every frame
            totalTime -= Time.deltaTime;

            // Divide the time by 60
            float minutes = Mathf.FloorToInt(totalTime / 60);

            // Returns the remainder
            float seconds = Mathf.FloorToInt(totalTime % 60);

            // Set the text string
            if (minutes > 0)
            {
                timerText.text = $"Get ready for battle: {minutes}m{seconds}s";
            }
            else
            {
                if (seconds > 5)
                {
                    timerText.text = $"Get ready for battle: {seconds}s";
                } else
                {
                    timerText.text = $"WARNING: {seconds}s";
                }
                
            }
            

        }
        else
        {
            timerText.text = "Time's up";
            timerText.enabled = false;
            totalTime = 0;
        }
    }
}
