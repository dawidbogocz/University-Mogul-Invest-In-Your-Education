using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public bool runningTimer;
    public float currentTime;
    public float maxTime = 30; //max time - start at 30s, every next game -5s
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;

    private 
    // Start is called before the first frame update
    void Start()
    {
        currentTime = maxTime;
        runningTimer = true;
        messageText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (runningTimer) {
            currentTime -= Time.deltaTime;
            if (currentTime >= -0.01) {
                timerText.text = currentTime.ToString("F");
            }

            if(clickcontrol.remainingItems == 0) {
                runningTimer = false;
                messageText.text = "Congratulations!\nYou don't have to pay.";
            }

            if (currentTime <= 0) {
                runningTimer = false;
                messageText.text = "You lost!\nYou have to pay.";
            }
        }
    }
}
