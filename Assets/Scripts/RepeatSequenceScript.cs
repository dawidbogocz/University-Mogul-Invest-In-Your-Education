using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RepeatSequenceScript : MonoBehaviour
{
    [SerializeField] private GameObject[] button = new GameObject[4];    // 0=red; 1=blue; 2=yellow; 3=green
    [SerializeField] private TextMeshProUGUI messegeDisplay, timerDisplay;
    private int[] sequence = new int[8];
    private int currentlyOn = 0;
    private int level = 1;
    private int elementsToShow = 4;
    private bool acceptingInput = false;
    private float timeLeft = 10;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            sequence[i] = Random.Range(0, 4);
        }
        StartCoroutine(startLevelMessege());
        StartCoroutine(startTimer());
    }

    private IEnumerator startTimer()
    {
        while (true)
        {
            if (acceptingInput)
            {
                timeLeft -= Time.deltaTime;

                if (timeLeft <= 0)
                {
                    timeLeft = 0.0f;
                    timerDisplay.text = "Time: " + timeLeft.ToString("0.00");
                    playerLost();
                }
                timerDisplay.text = "Time: " + timeLeft.ToString("0.00");
            }
            yield return null;
        }
    }

    private IEnumerator showSequence()
    {
        for (int i = 0; i < elementsToShow; i++)
        {
            yield return new WaitForSeconds(1.0f / elementsToShow);
            lightOn(sequence[i]);
            yield return new WaitForSeconds(2.0f / elementsToShow);
            lightOff(sequence[i]);
        }
        acceptingInput = true;
    }

    private void lightOn(int buttonID)
    {
        SpriteRenderer sprite = button[buttonID].GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1);
    }

    private void lightOff(int buttonID)
    {
        SpriteRenderer sprite = button[buttonID].GetComponent<SpriteRenderer>();
        sprite.color = new Color(0.25f, 0.25f, 0.25f); ;
    }

    public void clickButton(int buttonID)
    {
        if (acceptingInput)
        {
            lightOn(buttonID);
        }
    }

    public void checkPlayerSequence(int buttonID)
    {
        if (acceptingInput)
        {
            lightOff(buttonID);

            if (buttonID == sequence[currentlyOn])
            {
                currentlyOn++;
                if (currentlyOn == elementsToShow)
                {
                    acceptingInput = false;
                    if (level == 3)
                    {
                        playerWon();
                    }
                    else
                    {
                        level++;
                        currentlyOn = 0;
                        elementsToShow = 2 + (level * 2);
                        StartCoroutine(startLevelMessege());
                    }
                }
            }
            else
            {
                playerLost();
            }
        }
    }

    private void playerLost()
    {
        acceptingInput = false;
        messegeDisplay.text = "You lost, you have to pay";
        messegeDisplay.gameObject.SetActive(true);
    }

    private void playerWon()
    {
        acceptingInput = false;
        messegeDisplay.text = "Congratualtion, you don't need to pay";
        messegeDisplay.gameObject.SetActive(true);
    }

    private IEnumerator startLevelMessege()
    {
        messegeDisplay.text = "level " + level + "/3";
        messegeDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        messegeDisplay.gameObject.SetActive(false);
        StartCoroutine(showSequence());
    }
}
