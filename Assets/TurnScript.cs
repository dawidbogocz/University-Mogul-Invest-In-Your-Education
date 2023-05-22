using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnScript : MonoBehaviour
{
    int turn = 0;
    int numberOfPlayers;
    bool isMoving = false;

    [SerializeField]
    PawnScript[] pawn;

    [SerializeField]
    CameraScript myCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            int steps = Random.Range(1, 7);
            Debug.Log("Dice Rolled: " + steps);
            StartCoroutine(pawn[turn % numberOfPlayers].move(steps));
            Vector3 cameraOffset;
            if(pawn[turn % numberOfPlayers].currentField < 11)
            {
                cameraOffset = new Vector3(4, 3, 2);
            }
            else if(pawn[turn % numberOfPlayers].currentField < 21)
            {
                cameraOffset = new Vector3(2, 3, -4);
            }
            else if (pawn[turn % numberOfPlayers].currentField < 31)
            {
                cameraOffset = new Vector3(-4, 3, -2);
            }
            else
            {
                cameraOffset = new Vector3(-2, 3, 4);
            }
            myCamera.setCamera(pawn[turn % numberOfPlayers].gameObject, cameraOffset);
            turn++;
        }
    }

    public void setNumberOfPlayers(int number)
    {
        numberOfPlayers = number;
    }
}
