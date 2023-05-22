using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayersScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] pawn;
    [SerializeField]
    TurnScript turns;
    [SerializeField]
    CameraScript myCamera;
    int numberOfplayers;

    public void setNumberOfPlayers(int number)
    {
        numberOfplayers = number;
    }

    public void startGame()
    {
        for(int i = 0; i < numberOfplayers; i++)
        {
            pawn[i].SetActive(true);
        }
        turns.setNumberOfPlayers(numberOfplayers);
        Vector3 cameraOffset = new Vector3(2, 3, -4);
        myCamera.setCamera(pawn[0].gameObject, cameraOffset);
    }
}
