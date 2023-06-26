using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Entity
{
	public string playerName;
	public Player player;
	public PawnScript[] myPawn;
	public bool hasTurn;
	public bool hasWon;
	public enum PlayerTypes
	{
		HUMAN,
		CPU,
		OTHER
	}
	public PlayerTypes playerType;
}
public enum State
{
	ROLL_DICE,
	WAITING,
	SWITCH_PLAYER,
	BUYING
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
	public TMP_Text amountOfMoney;
	
    
    public List<Entity> entities = new List<Entity>();
    public static List<GameField> fields = new List<GameField>();

    public State state;

    public int activePlayer;
    bool switchingPlayer = false;
    bool turnPossible = true;

	public GameObject skipButton;
	public GameObject buyButton;
    public GameObject rollButton;
	int rolledHumanDice;
    int diceNumber;

	public DiceScript diceScript;

	public CameraScript cameraScript;

	bool skipped;


	public GameObject Red;
	public GameObject Green;
	public GameObject Blue;
	public GameObject Yellow;

	private void Awake()
	{
		Instance = this;

		for (int i = 0; i < entities.Count; i++)
		{
			if (SaveSettings.players[i] == "HUMAN") {
				entities[i].playerType = Entity.PlayerTypes.HUMAN;
				entities[i].player = new Player(entities[i].playerName);
			} else if (SaveSettings.players[i] == "CPU") {
				entities[i].playerType = Entity.PlayerTypes.CPU;
				entities[i].player = new Player(entities[i].playerName);
			}
		}
	}

	void Start()
    {
		ActivateButton(false);
		ActivateBuyingStateButtons(false);
		skipped = false;
		int randomPlayer = Random.Range(0, entities.Count);
		activePlayer = randomPlayer;
		Info.Instance.ShowMessage(entities[activePlayer].playerName + " starts first!");
    }

    void Update()
    {
		if (entities[activePlayer].playerType == Entity.PlayerTypes.CPU)
		{
			switch (entities[activePlayer].playerName)
			{
				case "Red":
					if (Red.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam1);
					}
					else if (Red.transform.rotation == Quaternion.Euler(0f, 90f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam2);
					}
					else if (Red.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam3);
					}
					else if (Red.transform.rotation == Quaternion.Euler(0f, 270f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam4);
					}
					break;
				case "Green":
					if (Green.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam1);
					}
					else if (Green.transform.rotation == Quaternion.Euler(0f, 90f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam2);
					}
					else if (Green.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam3);
					}
					else if (Green.transform.rotation == Quaternion.Euler(0f, 270f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam4);
					}
					break;
				case "Blue":
					if (Blue.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam1);
					}
					else if (Blue.transform.rotation == Quaternion.Euler(0f, 90f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam2);
					}
					else if (Blue.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam3);
					}
					else if (Blue.transform.rotation == Quaternion.Euler(0f, 270f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam4);
					}
					break;
				case "Yellow":
					if (Yellow.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam1);
					}
					else if (Yellow.transform.rotation == Quaternion.Euler(0f, 90f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam2);
					}
					else if (Yellow.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam3);
					}
					else if (Yellow.transform.rotation == Quaternion.Euler(0f, 270f, 0f))
					{
						cameraScript.SwitchCamera(cameraScript.cam4);
					}
					break;
			}

			switch (state)
			{
				case State.ROLL_DICE:
					{
						if (turnPossible)
						{
							StartCoroutine(RollDiceDelay());
							state = State.WAITING;
						}
					}
					break;
				case State.BUYING:
					{
						amountOfMoney.text = entities[activePlayer].player.money.ToString() + " $";
						state = State.WAITING;
					}
					break;
				case State.WAITING:
					{
						amountOfMoney.text = entities[activePlayer].player.money.ToString() + " $";

					}
					break;
				case State.SWITCH_PLAYER:
					{
						if (turnPossible)
						{
							StartCoroutine(SwitchPlayer());
							state = State.WAITING;
							
						}
					}
					break;
			}
		}
		if (entities[activePlayer].playerType == Entity.PlayerTypes.HUMAN)
		{
			if (entities[activePlayer].playerName == "Red")
			{
				cameraScript.SwitchCamera(cameraScript.redCam);
			}
			else if (entities[activePlayer].playerName == "Green")
			{
				cameraScript.SwitchCamera(cameraScript.greenCam);
			}
			else if (entities[activePlayer].playerName == "Blue")
			{
				cameraScript.SwitchCamera(cameraScript.blueCam);
			}
			else if (entities[activePlayer].playerName == "Yellow")
			{
				cameraScript.SwitchCamera(cameraScript.yellowCam);
			}

			switch (state)
			{
				case State.ROLL_DICE:
					{
						
						if (turnPossible)
						{
							ActivateButton(true);
							state = State.WAITING;
						}
					}
					break;
				case State.BUYING:
					{
						ActivateBuyingStateButtons(true);
						amountOfMoney.text = entities[activePlayer].player.money.ToString() + " $";
						if(skipped){
							state = State.WAITING;
							skipped = false;
						}

					}
					break;
				case State.WAITING:
					{
						ActivateBuyingStateButtons(true);
						amountOfMoney.text = entities[activePlayer].player.money.ToString() + " $";
					}
					break;
				case State.SWITCH_PLAYER:
					{
						if (turnPossible)
						{
							ActivateBuyingStateButtons(false);
							ActivateButton(false);
							StartCoroutine(SwitchPlayer());						
							state = State.WAITING;
						}
					}
					break;
			}
		}
	}

	public void RollDice()
	{
		diceScript.RollDice();
	}

	IEnumerator RollDiceDelay()
	{
		yield return new WaitForSeconds(2);
		RollDice();
	}

	public void MovePlayer(int diceNumber)
	{
		Info.Instance.ShowMessage(entities[activePlayer].playerName + " has rolled " + diceNumber);
		List<PawnScript> player = new List<PawnScript>();

		for(int i = 0; i < entities[activePlayer].myPawn.Length; i++)
		{
	
			player.Add(entities[activePlayer].myPawn[i]);
		}

		if(player.Count > 0)
		{
			player[0].StartTheMove(diceNumber);
			state = State.WAITING;
			return;
		}

		
		state = State.BUYING;
	}


	IEnumerator SwitchPlayer()
	{
		if(switchingPlayer)
		{
			yield break;
		}
		switchingPlayer = true;

		yield return new WaitForSeconds(2);

		SetNextActivePlayer();

		switchingPlayer = false;
		state = State.ROLL_DICE;
	}

	void SetNextActivePlayer()
	{
		activePlayer++;
		activePlayer %= entities.Count;

		int available = 0;
		for(int i = 0; i < entities.Count; i++)
		{
			if (!entities[i].hasWon)
			{
				available++;
			}
		}

		if (entities[activePlayer].hasWon && available > 1)
		{
			SetNextActivePlayer();
			return;
		}
		else if (available < 2)
		{
			state = State.WAITING;
			return;
		}
		Info.Instance.ShowMessage(entities[activePlayer].playerName + "'s turn!");
		state = State.ROLL_DICE;
	}


	void ActivateButton(bool on)
	{
		rollButton.SetActive(on);
	}
	
	void ActivateBuyingStateButtons(bool on)
	{
		skipButton.SetActive(on);
		buyButton.SetActive(on);
	}

	public void HumanRollDice()
	{
		ActivateButton(false);

		RollDice();
	}
	public void SkipTurn()
	{
		ActivateBuyingStateButtons(false);
		skipped = true;
	}
	public void BuyProperty()
	{
		ActivateBuyingStateButtons(false);
	}
}
