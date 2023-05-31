using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownController : MonoBehaviour
{

    public int countdownTime;
    public TextMeshProUGUI countdownDisplay;

    private void Start(){
        StartCoroutine(CountdownToStart());
    }

   IEnumerator CountdownToStart(){

    while(countdownTime > 0){
        countdownDisplay.text = countdownTime.ToString();
        yield return new WaitForSeconds(1f);

        countdownTime--;
    }

    countdownDisplay.text = "Go!";

    PuzzleGameController.instance.BeginTimer();

    yield return new WaitForSeconds(1f);
    countdownDisplay.text = "";
    //countdownDisplay.gameObject.SetActive(false);
   }

}
