using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public enum State
{
    ROLL_DICE,
    WAITING,
    SWITCH_PLAYER,
    BUYING,
    IN_JAIL
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text amountOfMoney;


    public List<Player> players = new List<Player>();
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
    public InventoryScript inventoryScript;
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

        players[0] = GameObject.Find("Red").GetComponent<Player>();
        players[1] = GameObject.Find("Green").GetComponent<Player>();
        players[2] = GameObject.Find("Blue").GetComponent<Player>();
        players[3] = GameObject.Find("Yellow").GetComponent<Player>();

        for (int i = 0; i < players.Count; i++)
        {
            if (SaveSettings.players[i] == "HUMAN")
            {
                players[i].playerType = PlayerTypes.HUMAN;
            }
            else if (SaveSettings.players[i] == "CPU")
            {
                players[i].playerType = PlayerTypes.CPU;
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
        int randomPlayer = Random.Range(0, players.Count);
        activePlayer = randomPlayer;
        inventoryScript.SwitchInventory(activePlayer);
        Info.Instance.ShowMessage(players[activePlayer].playerName + " starts first!");
    }

    void Update()
    {
        if (players[activePlayer].playerType == PlayerTypes.CPU)
        {

			if (Quaternion.Angle(players[activePlayer].transform.rotation, Quaternion.Euler(0f, 0f, 0f)) < 0.1f)
			{
				cameraScript.SwitchCamera(cameraScript.cam1);
			}
			else if (Quaternion.Angle(players[activePlayer].transform.rotation, Quaternion.Euler(0f, 90f, 0f)) < 0.1f)
			{
				cameraScript.SwitchCamera(cameraScript.cam2);
			}
			else if (Quaternion.Angle(players[activePlayer].transform.rotation, Quaternion.Euler(0f, 180f, 0f)) < 0.1f)
			{
				cameraScript.SwitchCamera(cameraScript.cam3);
			}
			else if (Quaternion.Angle(players[activePlayer].transform.rotation, Quaternion.Euler(0f, 270f, 0f)) < 0.1f)
			{
				cameraScript.SwitchCamera(cameraScript.cam4);
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
                        isBuying = true;
                        if (turnPossible)
                        {
                            int tmp = players[activePlayer].myPawn.fieldID;
                            Debug.Log(tmp);
                            FieldType fieldType = players[activePlayer].GetCurrentField().GetFieldType();
                            Debug.Log(fieldType);
                            FieldSubtype fieldSubtype = players[activePlayer].GetCurrentField().GetFieldSubtype();
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
                                        MovePlayerToField(38);
                                    }
                                    else if (RiskTasks[randomIndex].Contains("BREAK"))
                                    {
                                        MovePlayerToField(30);
                                    }
                                }
                                else if (RiskTasks[randomIndex].Contains("COLLECT"))
                                {
                                    if (RiskTasks[randomIndex].Contains("$200"))
                                    {
                                        players[activePlayer].AddMoney(200);
                                        state = State.SWITCH_PLAYER;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$500"))
                                    {
                                        players[activePlayer].AddMoney(500);
                                        state = State.SWITCH_PLAYER;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$1000"))
                                    {
                                        players[activePlayer].AddMoney(1000);
                                        state = State.SWITCH_PLAYER;
                                    }
                                }
                                else if (RiskTasks[randomIndex].Contains("PAY"))
                                {
                                    if (RiskTasks[randomIndex].Contains("$25"))
                                    {
                                        players[activePlayer].DeductMoney(25);
                                        state = State.SWITCH_PLAYER;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$50"))
                                    {
                                        players[activePlayer].DeductMoney(50);
                                        state = State.SWITCH_PLAYER;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$100"))
                                    {
                                        players[activePlayer].DeductMoney(100);
                                        state = State.SWITCH_PLAYER;
                                    }
                                }
                            }
                            else if (fieldSubtype == FieldSubtype.Chance)
                            {
                                var randomIndex = Random.Range(0, ChanceTasks.Count);
                                Debug.Log(ChanceTasks[randomIndex]);
                                Info.Instance.ShowCard(ChanceTasks[randomIndex]);

                                if (ChanceTasks[randomIndex].Contains("GO"))
                                {
                                    if (ChanceTasks[randomIndex].Contains("Solaris"))
                                    {
                                        MovePlayerToField(1);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Babilon"))
                                    {
                                        MovePlayerToField(8);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Strzecha"))
                                    {
                                        MovePlayerToField(13);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Piast"))
                                    {
                                        MovePlayerToField(18);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Ziemowit"))
                                    {
                                        MovePlayerToField(23);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Barbara"))
                                    {
                                        MovePlayerToField(29);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Elektron"))
                                    {
                                        MovePlayerToField(32);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Ondraszek"))
                                    {
                                        MovePlayerToField(37);
                                    }
                                }
                                else if (ChanceTasks[randomIndex].Contains("COLLECT"))
                                {
                                    if (ChanceTasks[randomIndex].Contains("$100"))
                                    {
                                        players[activePlayer].AddMoney(100);
                                        state = State.SWITCH_PLAYER;
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("$25"))
                                    {
                                        players[activePlayer].AddMoney(25);
                                        state = State.SWITCH_PLAYER;
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("$50"))
                                    {
                                        players[activePlayer].AddMoney(50);
                                        state = State.SWITCH_PLAYER;
                                    }
                                }
                            }
                            else if (fieldType == FieldType.Faculty || fieldType == FieldType.Dorm || fieldType == FieldType.Elevator || fieldType == FieldType.Recreation || fieldType == FieldType.Superpower)
                            {
                                Info.Instance.ShowMessage(players[activePlayer].PayRent());
                                if (players[activePlayer].GetCurrentField().GetOwner() == null)
                                {
                                    Info.Instance.ShowMessage(players[activePlayer].BuyPay());
                                }
                                state = State.SWITCH_PLAYER;
                            }
                            else if (fieldType == FieldType.Tax)
                            {
                                Info.Instance.ShowMessage(players[activePlayer].Tax());
                                state = State.SWITCH_PLAYER;
                            }
                            else if (fieldType == FieldType.StudyBreak)
                            {
                                Info.Instance.ShowMessage(players[activePlayer].GoToJail());
                                state = State.IN_JAIL;
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
                        //amountOfMoney.text = "$" + players[activePlayer].money.ToString();
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
                case State.IN_JAIL:
                    {
                        if (players[activePlayer].CanGetOutOfJail())
                        {
                            Info.Instance.ShowMessage(players[activePlayer].GetOutOfJail());
                            state = State.ROLL_DICE;
                        }
                        else
                        {
                            players[activePlayer].DecrementJailTurns();
                            state = State.SWITCH_PLAYER;
                        }
                    }
                    break;
            }
        }
        if (players[activePlayer].playerType == PlayerTypes.HUMAN)
        {
            if (players[activePlayer].playerName == "Red")
            {
                cameraScript.SwitchCamera(cameraScript.redCam);
            }
            else if (players[activePlayer].playerName == "Green")
            {
                cameraScript.SwitchCamera(cameraScript.greenCam);
            }
            else if (players[activePlayer].playerName == "Blue")
            {
                cameraScript.SwitchCamera(cameraScript.blueCam);
            }
            else if (players[activePlayer].playerName == "Yellow")
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
                        isBuying = true;
                        if (turnPossible)
                        {
                            ActivateObject(ref skipButton, true);
                            int tmp = players[activePlayer].myPawn.fieldID;
                            Debug.Log(tmp);
                            FieldType fieldType = players[activePlayer].GetCurrentField().GetFieldType();
                            Debug.Log(fieldType);
                            FieldSubtype fieldSubtype = players[activePlayer].GetCurrentField().GetFieldSubtype();
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
                                        MovePlayerToField(38);
                                    }
                                    else if (RiskTasks[randomIndex].Contains("BREAK"))
                                    {
                                        MovePlayerToField(30);
                                    }
                                }
                                else if (RiskTasks[randomIndex].Contains("COLLECT"))
                                {
                                    if (RiskTasks[randomIndex].Contains("$200"))
                                    {
                                        players[activePlayer].AddMoney(200);
                                        state = State.WAITING;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$500"))
                                    {
                                        players[activePlayer].AddMoney(500);
                                        state = State.WAITING;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$1000"))
                                    {
                                        players[activePlayer].AddMoney(1000);
                                        state = State.WAITING;
                                    }
                                }
                                else if (RiskTasks[randomIndex].Contains("PAY"))
                                {
                                    if (RiskTasks[randomIndex].Contains("$25"))
                                    {
                                        players[activePlayer].DeductMoney(25);
                                        state = State.WAITING;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$50"))
                                    {
                                        players[activePlayer].DeductMoney(50);
                                        state = State.WAITING;
                                    }
                                    else if (RiskTasks[randomIndex].Contains("$100"))
                                    {
                                        players[activePlayer].DeductMoney(100);
                                        state = State.WAITING;
                                    }
                                }
                            }
                            else if (fieldSubtype == FieldSubtype.Chance)
                            {
                                ActivateObject(ref cardInfo, true);
                                var randomIndex = Random.Range(0, ChanceTasks.Count);
                                Debug.Log(ChanceTasks[randomIndex]);
                                Info.Instance.ShowCard(ChanceTasks[randomIndex]);

                                if (ChanceTasks[randomIndex].Contains("GO"))
                                {
                                    if (ChanceTasks[randomIndex].Contains("Solaris"))
                                    {
                                        MovePlayerToField(1);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Babilon"))
                                    {
                                        MovePlayerToField(8);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Strzecha"))
                                    {
                                        MovePlayerToField(13);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Piast"))
                                    {
                                        MovePlayerToField(18);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Ziemowit"))
                                    {
                                        MovePlayerToField(23);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Barbara"))
                                    {
                                        MovePlayerToField(29);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Elektron"))
                                    {
                                        MovePlayerToField(32);
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("Ondraszek"))
                                    {
                                        MovePlayerToField(37);
                                    }
                                }
                                else if (ChanceTasks[randomIndex].Contains("COLLECT"))
                                {
                                    if (ChanceTasks[randomIndex].Contains("$100"))
                                    {
                                        players[activePlayer].AddMoney(100);
                                        state = State.WAITING;
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("$25"))
                                    {
                                        players[activePlayer].AddMoney(25);
                                        state = State.WAITING;
                                    }
                                    else if (ChanceTasks[randomIndex].Contains("$50"))
                                    {
                                        players[activePlayer].AddMoney(50);
                                        state = State.WAITING;
                                    }
                                }
                            }
                            else if (fieldType == FieldType.Faculty || fieldType == FieldType.Dorm || fieldType == FieldType.Elevator || fieldType == FieldType.Recreation || fieldType == FieldType.Superpower)
                            {
                                if (players[activePlayer].GetCurrentField().GetOwner() == null)
                                {
                                    ActivateObject(ref buyButton, true);
                                }
                                state = State.WAITING;
                            }
                            else if (fieldType == FieldType.Tax)
                            {
                                Info.Instance.ShowMessage(players[activePlayer].Tax());
                                state = State.WAITING;
                            }
                            else if (fieldType == FieldType.StudyBreak)
                            {
                                Info.Instance.ShowMessage(players[activePlayer].GoToJail());
                                state = State.IN_JAIL;
                            }
                            else
                            {
                                state = State.WAITING;
                            }
                        }
                        if (skipped)
                        {
                            state = State.WAITING;
                            skipped = false;
                        }
                    }
                    break;
                case State.WAITING:
                    {
                        //amountOfMoney.text = "$" + players[activePlayer].money.ToString();
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
                case State.IN_JAIL:
                    {
                        if (players[activePlayer].CanGetOutOfJail())
                        {
                            Info.Instance.ShowMessage(players[activePlayer].GetOutOfJail());
                            state = State.ROLL_DICE;
                        }
                        else
                        {
                            players[activePlayer].DecrementJailTurns();
                            state = State.SWITCH_PLAYER;
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

    public void MovePlayerForward(int diceNumber)
    {
        Info.Instance.ShowMessage(players[activePlayer].playerName + " has rolled " + diceNumber);
        PawnScript pawn = players[activePlayer].myPawn;

        if (pawn != null)
        {
            pawn.MoveToField(30);
            state = State.WAITING;
            return;
        }

        state = State.BUYING;
    }

    public void MovePlayerToField(int fieldID)
    {
        PawnScript pawn = players[activePlayer].myPawn;

        if (pawn != null)
        {
            pawn.MoveToField(fieldID);
            state = State.WAITING;
            return;
        }

        state = State.BUYING;
    }

    IEnumerator SwitchPlayer()
    {
        if (switchingPlayer)
        {
            yield break;
        }
        switchingPlayer = true;

        yield return new WaitForSeconds(2);

        SetNextActivePlayer();

        switchingPlayer = false;
        inventoryScript.SwitchInventory(activePlayer);

        if (players[activePlayer].IsInJail())
        {
            state = State.IN_JAIL;
        }
        else
        {
            state = State.ROLL_DICE;
        }
    }

    void SetNextActivePlayer()
    {
        activePlayer++;
        activePlayer %= players.Count;

        int available = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].hasWon)
            {
                available++;
            }
        }

        if (players[activePlayer].hasWon && available > 1)
        {
            SetNextActivePlayer();
            return;
        }
        else if (available < 2)
        {
            state = State.WAITING;
            return;
        }
        Info.Instance.ShowMessage(players[activePlayer].playerName + "'s turn!");
        state = State.ROLL_DICE;
    }


    void ActivateObject(ref GameObject obj, bool on)
    {
        obj.SetActive(on);
    }

    void ActivateBuyingStateButtons(bool on)
    {
        skipButton.SetActive(on);
        buyButton.SetActive(on);
    }

    public void HumanRollDice()
    {
        ActivateObject(ref rollButton, false);

        RollDice();
    }
    public void SkipTurn()
    {
        ActivateObject(ref skipButton, false);
        Info.Instance.ShowMessage(players[activePlayer].PayRent());
        state = State.SWITCH_PLAYER;
    }
    public void BuyProperty()
    {
        ActivateObject(ref buyButton, false);

        Info.Instance.ShowMessage(players[activePlayer].BuyPay());
    }
}
