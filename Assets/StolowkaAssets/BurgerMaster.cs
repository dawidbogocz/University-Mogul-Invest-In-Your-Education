using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BurgerMaster : MonoBehaviour
{
    public bool runningTimer;
    public float currentTime;
    public float maxTime = 30; //max time - start at 30s, every next game -5s
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI timerText;

    public static int orderValue = 1111111;
    public static int plateValue = 0000000;

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

            if (plateValue == orderValue) {
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
