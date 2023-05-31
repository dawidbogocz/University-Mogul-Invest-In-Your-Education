using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;

   public TextMeshProUGUI timeCounter;
   //PuzzleGameController puzzle;




    private TimeSpan timePlaying;
    private bool timerGoing;

    private float elapsedTime;


    public void Awake()
    {
        instance = this;

    }

    public void Start()
    {
        timeCounter.text = "Time: 30.00";
        timerGoing = false;
        
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 10.0f;

        StartCoroutine(UpdateTimer());
    }



    private IEnumerator UpdateTimer()
    {
        while (timerGoing && elapsedTime > 00.00f)
        {
            
            //timer = timer.Subtract(deltaTimeSpan);
            //string t = "Time left: " + timer.ToString("mm\\:ss");
            //EditorGUILayout.LabelField("Next: ", t);


            elapsedTime -= Time.deltaTime;
             timePlaying = TimeSpan.FromSeconds(elapsedTime);

            string timePlayingStr = "Time: " + timePlaying.ToString("ss'.'ff");
            timeCounter.text = timePlayingStr;

            if(elapsedTime == 00.00f){

                //puzzle.isFailed = true;
                System.Threading.Thread.Sleep(2000);
                timerGoing = false;
                
            }

            yield return null;
        }

    }
}
