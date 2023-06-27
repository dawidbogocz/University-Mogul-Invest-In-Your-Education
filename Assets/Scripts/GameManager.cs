using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using System.Linq;

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
	bool isBuying = false;

	public GameObject skipButton;
	public GameObject buyButton;
    public GameObject rollButton;
	public GameObject cardInfo;
	int rolledHumanDice;
    int diceNumber;

	public DiceScript diceScript;

	public CameraScript cameraScript;

	bool skipped;


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
		ActivateObject(ref cardInfo, false);
		ActivateObject(ref skipButton, false);
		ActivateObject(ref buyButton, false);

		var lines1 = fileRisk.text.Split('\n');
		RiskTasks = new List<string>(lines1);

		var lines2 = fileChance.text.Split('\n');
		ChanceTasks = new List<string>(lines2);

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
						isBuying = true;
						if (turnPossible)
						{
							int tmp = entities[activePlayer].myPawn[0].fieldId;
							Debug.Log(tmp);
							FieldType fieldType = entities[activePlayer].player.GetCurrentField().GetFieldType();
							Debug.Log(fieldType);
							FieldSubtype fieldSubtype = entities[activePlayer].player.GetCurrentField().GetFieldSubtype();
							Debug.Log(fieldSubtype);
							if (fieldSubtype == FieldSubtype.Risk)
							{
								var randomIndex = Random.Range(0, RiskTasks.Count);
								Debug.Log(RiskTasks[randomIndex]);
								Info.Instance.ShowCard(RiskTasks[randomIndex]);

								if (RiskTasks[randomIndex].Contains("GO"))
								{
									new WaitForSeconds(5);
									if (RiskTasks[randomIndex].Contains("Tax"))
									{
										MovePlayer(38 - tmp);
									}
									else if (RiskTasks[randomIndex].Contains("BREAK"))
									{
										if (tmp > 30)
										{
											int temp = 39 - tmp + 1 + 30;
											MovePlayer(temp);
											state = State.SWITCH_PLAYER;
										}
										else
										{
											MovePlayer(30 - tmp);
											state = State.SWITCH_PLAYER;
										}
									}
								}
								else if (RiskTasks[randomIndex].Contains("COLLECT"))
								{
									if (RiskTasks[randomIndex].Contains("$200"))
									{
										entities[activePlayer].player.AddMoney(200);
										state = State.SWITCH_PLAYER;
									}
									else if (RiskTasks[randomIndex].Contains("$500"))
									{
										entities[activePlayer].player.AddMoney(500);
										state = State.SWITCH_PLAYER;
									}
									else if (RiskTasks[randomIndex].Contains("$1000"))
									{
										entities[activePlayer].player.AddMoney(1000);
										state = State.SWITCH_PLAYER;
									}
								}
								else if (RiskTasks[randomIndex].Contains("PAY"))
								{
									if (RiskTasks[randomIndex].Contains("$25"))
									{
										entities[activePlayer].player.DeductMoney(25);
										state = State.SWITCH_PLAYER;
									}
									else if (RiskTasks[randomIndex].Contains("$50"))
									{
										entities[activePlayer].player.DeductMoney(50);
										state = State.SWITCH_PLAYER;
									}
									else if (RiskTasks[randomIndex].Contains("$100"))
									{
										entities[activePlayer].player.DeductMoney(100);
										state = State.SWITCH_PLAYER;
									}
								}
							}

							if (fieldSubtype == FieldSubtype.Chance)
							{
								var randomIndex = Random.Range(0, ChanceTasks.Count);
								Debug.Log(ChanceTasks[randomIndex]);
								Info.Instance.ShowCard(ChanceTasks[randomIndex]);

								if (ChanceTasks[randomIndex].Contains("GO"))
								{
									if (ChanceTasks[randomIndex].Contains("Solaris"))
									{
										if (tmp > 1)
										{
											int temp = 39 - tmp + 1 + 1;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(1 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Babilon"))
									{
										if (tmp > 8)
										{
											int temp = 39 - tmp + 8;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(8 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Strzecha"))
									{
										if (tmp > 13)
										{
											int temp = 39 - tmp + 1 + 13;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(13 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Piast"))
									{
										if (tmp > 18)
										{
											int temp = 39 - tmp + 1 + 18;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(18 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Ziemowit"))
									{
										if (tmp > 23)
										{
											int temp = 39 - tmp + 1 + 23;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(23 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Barbara"))
									{
										if (tmp > 29)
										{
											int temp = 39 - tmp + 1 + 29;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(29 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Elektron"))
									{
										if (tmp > 32)
										{
											int temp = 39 - tmp + 1 + 32;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(32 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Ondraszek"))
									{
										if (tmp > 37)
										{
											int temp = 39 - tmp + 1 + 37;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(37 - tmp);
										}
									}
								}
								else if (ChanceTasks[randomIndex].Contains("COLLECT"))
								{
									if (ChanceTasks[randomIndex].Contains("$100"))
									{
										entities[activePlayer].player.AddMoney(100);
										state = State.SWITCH_PLAYER;
									}
									else if (ChanceTasks[randomIndex].Contains("$25"))
									{
										entities[activePlayer].player.AddMoney(25);
										state = State.SWITCH_PLAYER;
									}
									else if (ChanceTasks[randomIndex].Contains("$50"))
									{
										entities[activePlayer].player.AddMoney(50);
										state = State.SWITCH_PLAYER;
									}
								}
							}
							else if (fieldType == FieldType.Faculty || fieldType == FieldType.Dorm || fieldType == FieldType.Elevator || fieldType == FieldType.Recreation)
							{
								entities[activePlayer].player.BuyPay();
								state = State.SWITCH_PLAYER;
							}
							else if (fieldType == FieldType.Tax)
							{
								entities[activePlayer].player.Tax();
								state = State.SWITCH_PLAYER;
							}
							else
							{
								state = State.SWITCH_PLAYER;
							}
						}
						if (skipped)
						{
							state = State.SWITCH_PLAYER;
							skipped = false;
						}
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
							ActivateObject(ref rollButton, true);
							state = State.WAITING;
						}
					}
					break;
				case State.BUYING:
					{
						amountOfMoney.text = entities[activePlayer].player.money.ToString() + " $";
						isBuying = true;
						if (turnPossible)
						{
							ActivateObject(ref skipButton, true);
							int tmp = entities[activePlayer].myPawn[0].fieldId;
							Debug.Log(tmp);
							FieldType fieldType = entities[activePlayer].player.GetCurrentField().GetFieldType();
							Debug.Log(fieldType);
							FieldSubtype fieldSubtype = entities[activePlayer].player.GetCurrentField().GetFieldSubtype();
							Debug.Log(fieldSubtype);
							if (fieldSubtype == FieldSubtype.Risk)
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
										MovePlayer(38 - tmp);
									}
									else if (RiskTasks[randomIndex].Contains("BREAK"))
									{
										if (tmp > 30)
										{
											int temp = 39 - tmp + 1 + 30;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(30 - tmp);
										}
									}
								}
								else if (RiskTasks[randomIndex].Contains("COLLECT"))
								{
									if (RiskTasks[randomIndex].Contains("$200"))
									{
										entities[activePlayer].player.AddMoney(200);
										state = State.WAITING;
									}
									else if (RiskTasks[randomIndex].Contains("$500"))
									{
										entities[activePlayer].player.AddMoney(500);
										state = State.WAITING;
									}
									else if (RiskTasks[randomIndex].Contains("$1000"))
									{
										entities[activePlayer].player.AddMoney(1000);
										state = State.WAITING;
									}
								}
								else if (RiskTasks[randomIndex].Contains("PAY"))
								{
									if (RiskTasks[randomIndex].Contains("$25"))
									{
										entities[activePlayer].player.DeductMoney(25);
										state = State.WAITING;
									}
									else if (RiskTasks[randomIndex].Contains("$50"))
									{
										entities[activePlayer].player.DeductMoney(50);
										state = State.WAITING;
									}
									else if (RiskTasks[randomIndex].Contains("$100"))
									{
										entities[activePlayer].player.DeductMoney(100);
										state = State.WAITING;
									}
								}
							}

							if (fieldSubtype == FieldSubtype.Chance)
							{
								ActivateObject(ref cardInfo, true);
								var randomIndex = Random.Range(0, ChanceTasks.Count);
								Debug.Log(ChanceTasks[randomIndex]);
								Info.Instance.ShowCard(ChanceTasks[randomIndex]);

								if (ChanceTasks[randomIndex].Contains("GO"))
								{
									if (ChanceTasks[randomIndex].Contains("Solaris"))
									{
										if (tmp > 1)
										{
											int temp = 39 - tmp + 1 + 1;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(1 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Babilon"))
									{
										if (tmp > 8)
										{
											int temp = 39 - tmp + 8;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(8 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Strzecha"))
									{
										if (tmp > 13)
										{
											int temp = 39 - tmp + 1 + 13;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(13 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Piast"))
									{
										if (tmp > 18)
										{
											int temp = 39 - tmp + 1 + 18;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(18 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Ziemowit"))
									{
										if (tmp > 23)
										{
											int temp = 39 - tmp + 1 + 23;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(23 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Barbara"))
									{
										if (tmp > 29)
										{
											int temp = 39 - tmp + 1 + 29;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(29 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Elektron"))
									{
										if (tmp > 32)
										{
											int temp = 39 - tmp + 1 + 32;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(32 - tmp);
										}
									}
									else if (ChanceTasks[randomIndex].Contains("Ondraszek"))
									{
										if (tmp > 37)
										{
											int temp = 39 - tmp + 1 + 37;
											MovePlayer(temp);
										}
										else
										{
											MovePlayer(37 - tmp);
										}
									}
								}
								else if (ChanceTasks[randomIndex].Contains("COLLECT"))
								{
									if (ChanceTasks[randomIndex].Contains("$100"))
									{
										entities[activePlayer].player.AddMoney(100);
										state = State.WAITING;
									}
									else if (ChanceTasks[randomIndex].Contains("$25"))
									{
										entities[activePlayer].player.AddMoney(25);
										state = State.WAITING;
									}
									else if (ChanceTasks[randomIndex].Contains("$50"))
									{
										entities[activePlayer].player.AddMoney(50);
										state = State.WAITING;
									}
								}
							}
							else if (fieldType == FieldType.Faculty || fieldType == FieldType.Dorm || fieldType == FieldType.Elevator || fieldType == FieldType.Recreation)
							{
								ActivateObject(ref buyButton, true);
								state = State.WAITING;
							}
							else if (fieldType == FieldType.Tax)
							{
								entities[activePlayer].player.Tax();
								state = State.WAITING;
							}
							else
							{
								state = State.WAITING;
							}
						}
					if (skipped){
							state = State.WAITING;
							skipped = false;
						}
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
							ActivateObject(ref skipButton, false);
							ActivateObject(ref buyButton, false);
							ActivateObject(ref cardInfo, false);
							isBuying = false;
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


	void ActivateObject(ref GameObject obj, bool on)
	{
		obj.SetActive(on);
	}

	public void HumanRollDice()
	{
		ActivateObject(ref rollButton, false);

		RollDice();
	}
	public void SkipTurn()
	{
		ActivateObject(ref skipButton, false);
		skipped = true;
		state = State.SWITCH_PLAYER;
	}
	public void BuyProperty()
	{
		ActivateObject(ref buyButton, false);

		entities[activePlayer].player.BuyPay();
	}
}
