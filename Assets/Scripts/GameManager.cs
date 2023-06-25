using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

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
	
    
    public List<Entity> entities = new List<Entity>();
    public static List<GameField> fields = new List<GameField>();

    public State state;

    public int activePlayer;
    bool switchingPlayer = false;
    bool turnPossible = true;
	bool isBuying = false;

    public GameObject rollButton;
	public GameObject endTurn;
	public GameObject cardInfo;
	int rolledHumanDice;
    int diceNumber;

	public DiceScript diceScript;

	public CameraScript cameraScript;

	public GameObject Red;
	public GameObject Green;
	public GameObject Blue;
	public GameObject Yellow;

	public TextAsset fileRisk;
	public TextAsset fileChance;
	private List<string> RiskTasks;
	private List<string> ChanceTasks;

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
		ActivateObject(ref rollButton, false);
		ActivateObject(ref endTurn, false);
		ActivateObject(ref cardInfo, false);

		var lines1 = fileRisk.text.Split('\n');
		RiskTasks = new List<string>(lines1);

		var lines2 = fileChance.text.Split('\n');
		ChanceTasks = new List<string>(lines2);

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
				case State.WAITING:
					{

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
				case State.BUYING:
					{
						isBuying = true;
						if (turnPossible)
						{
							ActivateObject(ref endTurn, true);
							int tmp = entities[activePlayer].myPawn[0].currentField;
							Debug.Log(tmp);
							if (entities[activePlayer].myPawn[0].currentField == 2 || entities[activePlayer].myPawn[0].currentField == 17 || entities[activePlayer].myPawn[0].currentField == 33)
							{
								ActivateObject(ref cardInfo, true);
								var randomIndex = Random.Range(0, RiskTasks.Count);
								Debug.Log(RiskTasks[randomIndex]);
								Info.Instance.ShowCard(RiskTasks[randomIndex]);
							}

							if (entities[activePlayer].myPawn[0].currentField == 7 || entities[activePlayer].myPawn[0].currentField == 22 || entities[activePlayer].myPawn[0].currentField == 36)
							{
								ActivateObject(ref cardInfo, true);
								var randomIndex = Random.Range(0, ChanceTasks.Count);
								Debug.Log(ChanceTasks[randomIndex]);
								Info.Instance.ShowCard(ChanceTasks[randomIndex]);
							}
						}
						state = State.WAITING;
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
							ActivateObject(ref rollButton, true);
							state = State.WAITING;
						}
					}
					break;
				case State.WAITING:
					{

					}
					break;
				case State.SWITCH_PLAYER:
					{
						if (turnPossible)
						{
							ActivateObject(ref cardInfo, false);
							ActivateObject(ref endTurn, false);
							isBuying = false;
							StartCoroutine(SwitchPlayer());
							state = State.WAITING;
						}
					}
					break;
				case State.BUYING:
					{
						isBuying = true;
						if (turnPossible)
						{
							ActivateObject(ref endTurn, true);
							int tmp = entities[activePlayer].myPawn[0].currentField;
							Debug.Log(tmp);
							if (entities[activePlayer].myPawn[0].currentField == 2 || entities[activePlayer].myPawn[0].currentField == 17 || entities[activePlayer].myPawn[0].currentField == 33)
							{
								ActivateObject(ref cardInfo, true);
								var randomIndex = Random.Range(0, RiskTasks.Count);
								Debug.Log(RiskTasks[randomIndex]);
								Info.Instance.ShowCard(RiskTasks[randomIndex]);

								if (RiskTasks[randomIndex].Contains("GO"))
								{
									new WaitForSeconds(5);
									if (RiskTasks[randomIndex].Contains("Tax"))
									{
										MovePlayer(38 - entities[activePlayer].myPawn[0].currentField);
									}
									else if (RiskTasks[randomIndex].Contains("BREAK"))
									{
										MovePlayer(30 - entities[activePlayer].myPawn[0].currentField);
									}
								}
								else if (RiskTasks[randomIndex].Contains("COLLECT"))
								{
									if (RiskTasks[randomIndex].Contains("$200"))
									{
										entities[activePlayer].player.AddMoney(200);
									}
									else if (RiskTasks[randomIndex].Contains("$500"))
									{
										entities[activePlayer].player.AddMoney(500);
									}
									else if(RiskTasks[randomIndex].Contains("$1000"))
									{
										entities[activePlayer].player.AddMoney(1000);
									}
								}
								else if (RiskTasks[randomIndex].Contains("PAY"))
								{
									if (RiskTasks[randomIndex].Contains("$25"))
									{
										entities[activePlayer].player.DeductMoney(25);
									}
									else if (RiskTasks[randomIndex].Contains("$50"))
									{
										entities[activePlayer].player.DeductMoney(50);
									}
									else if (RiskTasks[randomIndex].Contains("$100"))
									{
										entities[activePlayer].player.DeductMoney(100);
									}
								}
							}

							if (entities[activePlayer].myPawn[0].currentField == 7 || entities[activePlayer].myPawn[0].currentField == 22 || entities[activePlayer].myPawn[0].currentField == 36)
							{
								ActivateObject(ref cardInfo, true);
								var randomIndex = Random.Range(0, ChanceTasks.Count);
								Debug.Log(ChanceTasks[randomIndex]);
								Info.Instance.ShowCard(ChanceTasks[randomIndex]);

								if (ChanceTasks[randomIndex].Contains("GO"))
								{
									if (ChanceTasks[randomIndex].Contains("Solaris"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 1)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 1;
											MovePlayer(temp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Babilon"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 8)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 8;
											MovePlayer(temp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Strzecha"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 13)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 13;
											MovePlayer(temp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Piast"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 18)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 18;
											MovePlayer(temp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Ziemowit"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 23)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 23;
											MovePlayer(temp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Barbara"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 29)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 29;
											MovePlayer(temp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Elektron"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 32)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 32;
											MovePlayer(temp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Ondraszek"))
									{
										if (entities[activePlayer].myPawn[0].currentField > 37)
										{
											int temp = 39 - entities[activePlayer].myPawn[0].currentField + 1 + 37;
											MovePlayer(temp);
										}
									}
								}
								else if (ChanceTasks[randomIndex].Contains("COLLECT"))
								{
									if (ChanceTasks[randomIndex].Contains("$100"))
									{
										entities[activePlayer].player.AddMoney(100);
									}
									else if (ChanceTasks[randomIndex].Contains("$25"))
									{
										entities[activePlayer].player.AddMoney(25);
									}
									else if (ChanceTasks[randomIndex].Contains("$50"))
									{
										entities[activePlayer].player.AddMoney(50);
									}
								}
							}
						}
						state = State.WAITING;
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
		if (diceNumber < 7)
		{
			Info.Instance.ShowMessage(entities[activePlayer].playerName + " has rolled " + diceNumber);
		}

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

		yield return new WaitForSeconds(0.1f);

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

	void ActivateObject(ref GameObject obj, bool on)
	{
		obj.SetActive(on);
	}

	public void HumanRollDice()
	{
		ActivateObject(ref rollButton, false);

		RollDice();
	}
	public void EndTurn()
	{
		ActivateObject(ref cardInfo, false);
		isBuying = false;
		state = State.SWITCH_PLAYER;
	}
}
