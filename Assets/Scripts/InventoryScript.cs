using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] private PathScript fields;
    [SerializeField] private GameObject propertyCard;
    [SerializeField] private Sprite[] sprites = new Sprite[9];
    private GameObject[] inventory = new GameObject[4];
    private int[] money = new int[4];
    private int[] previousMoney = new int[4];
    private GameObject[] moneyDisplay = new GameObject[4];
    private GameObject[] deck = new GameObject[4];
    private int selectedInventory;
    private Dictionary<string, bool> showingCard = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        inventory[0] = transform.Find("RedInventory").gameObject;
        inventory[1] = transform.Find("GreenInventory").gameObject;
        inventory[2] = transform.Find("BlueInventory").gameObject;
        inventory[3] = transform.Find("YellowInventory").gameObject;
        for (int i = 0; i < 4; i++)
        {
            moneyDisplay[i] = inventory[i].transform.Find("Money").gameObject;
            deck[i] = inventory[i].transform.Find("Deck").gameObject;
            money[i] = 1500;
            previousMoney[i] = 1500;
        }
        //StartCoroutine(test());
    }
    private IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        AddCard(0, 1);
        AddCard(0, 3);
        AddCard(0, 6);
        AddCard(0, 8);
        AddCard(0, 9);
        AddCard(0, 11);
        AddCard(0, 13);
        AddCard(0, 14);
        AddCard(0, 16);
        AddCard(0, 18);
        AddCard(0, 19);
        AddCard(0, 21);
        AddCard(0, 23);
        AddCard(0, 24);
        AddCard(0, 26);
        AddCard(0, 27);
        AddCard(0, 29);
        AddCard(0, 31);
        AddCard(0, 32);
        AddCard(0, 34);
        AddCard(0, 37);
        AddCard(0, 39);
        AddCard(0, 5);
        AddCard(0, 12);
        AddCard(0, 15);
        AddCard(0, 25);
        AddCard(0, 28);
        AddCard(0, 35);
    }

    void Update()
    {
        /*for (int i = 0; i < 4; i++)
        {
            if (players[i].money != money[i])
            {
                UpdateMoney(i, players[i].money);
            }
        }*/
        /*if (showingDeck.Count > 0) {
            for (int i = 0; i < 4; i++) {
                if (showingDeck[i]) {
                    transform.Find("Money").GetComponent<TMP_Text>().text = "$" + players[i].money.ToString();
                }
            }
        }*/
    }

    public void AddCard(int playerID, int fieldID)
    {
        GameObject card = Instantiate(propertyCard);
        GameObject cardSprite = card.transform.Find("Card").gameObject;
        GameObject field = fields.transform.GetChild(fieldID).gameObject;

        cardSprite.GetComponent<Image>().sprite = sprites[(int)field.GetComponent<GameField>().GetFieldColor()];

        card.transform.SetParent(deck[playerID].transform, false);

        card.name = field.name + " CARD";

        cardSprite.transform.Find("PropertyName").GetComponent<TextMeshProUGUI>().text = field.name;
        cardSprite.transform.Find("Rent").GetComponent<TextMeshProUGUI>().text = "Rent: $" + field.GetComponent<GameField>().GetRent();
        cardSprite.transform.Find("ColorRent").GetComponent<TextMeshProUGUI>().text = "Rent with all fields in color: $" + field.GetComponent<GameField>().cost * 2;
        cardSprite.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: $" + field.GetComponent<GameField>().cost;

        Vector3 pos = card.transform.localPosition;
        pos.y -= deck[playerID].transform.childCount * 11;
        card.transform.localPosition = pos;
        Vector3 spritePos = cardSprite.transform.localPosition;
        spritePos.x += 300;
        cardSprite.transform.localPosition = spritePos;

        EventTrigger trigger = card.GetComponentInParent<EventTrigger>();
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerEnter;
        entry1.callback.AddListener((eventData) => { ShowCard(card); });
        trigger.triggers.Add(entry1);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((eventData) => { HideCard(card); });
        trigger.triggers.Add(entry2);

        showingCard[card.name] = false;
        StartCoroutine(MoveCardLeft(card, 0));
    }

    public void SwitchInventory(int playerID)
    {
        selectedInventory = playerID;
        for (int i = 0; i < 4; i++)
        {
            ColorBlock colors = transform.Find("InventoryButtons").GetChild(i).GetComponent<Button>().colors;
            if (i == playerID)
            {

                colors.normalColor = new Color(1, 1, 1);
                StartCoroutine(MoveInventoryLeft(i));
            }
            else
            {
                colors.normalColor = new Color(0.25f, 0.25f, 0.25f);
                StartCoroutine(MoveInventoryRight(i));
            }
            transform.Find("InventoryButtons").GetChild(i).GetComponent<Button>().colors = colors;
        }
    }

    private IEnumerator MoveInventoryLeft(int inventoryID)
    {
        if (inventory[inventoryID] == null)
        {
            yield return new WaitForSeconds(1);
        }
        Vector3 currentPosition = inventory[inventoryID].transform.localPosition;
        Vector3 targetPosition = currentPosition;
        targetPosition.x = 0;
        float i = 0;

        while (selectedInventory == inventoryID && inventory[inventoryID].transform.localPosition.x > targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            inventory[inventoryID].transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, t);
            i += Time.deltaTime * 2f;
            yield return null;
        }
    }

    private IEnumerator MoveInventoryRight(int inventoryID)
    {
        if (inventory[inventoryID] == null)
        {
            yield return new WaitForSeconds(1);
        }
        Vector3 currentPosition = inventory[inventoryID].transform.localPosition;
        Vector3 targetPosition = currentPosition;
        targetPosition.x = 300;
        float i = 0;

        while (selectedInventory != inventoryID && inventory[inventoryID].transform.localPosition.x < targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            inventory[inventoryID].transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, t);
            i += Time.deltaTime * 2f;
            yield return null;
        }
    }

    public void ShowCard(GameObject card)
    {
        StartCoroutine(MoveCardLeft(card, -130));
    }

    private IEnumerator MoveCardLeft(GameObject cardObject, int targetX)
    {
        Transform card = cardObject.transform.Find("Card");
        Vector3 currentPosition = card.transform.localPosition;
        Vector3 targetPosition = currentPosition;
        targetPosition.x = targetX;
        float i = 0;
        showingCard[cardObject.name] = true;

        while (showingCard[cardObject.name] == true && card.transform.localPosition.x > targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            card.transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, t);
            i += Time.deltaTime * 2f;
            yield return null;
        }
    }

    public void HideCard(GameObject card)
    {
        StartCoroutine(MoveCardRight(card, 0));
    }

    private IEnumerator MoveCardRight(GameObject cardObject, int targetX)
    {
        Transform card = cardObject.transform.Find("Card");
        Vector3 currentPosition = card.transform.localPosition;
        Vector3 targetPosition = currentPosition;
        targetPosition.x = targetX;
        float i = 0;
        showingCard[cardObject.name] = false;

        while (showingCard[cardObject.name] == false && card.transform.localPosition.x < targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            card.transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, t);
            i += Time.deltaTime * 2f;
            yield return null;
        }
    }

    public void UpdateMoney(int playerID, int newAmount)
    {
        money[playerID] = newAmount;
        if (playerID == selectedInventory)
        {
            StartCoroutine(UpdateMoneyCoroutine(playerID));
        }
        else
        {
            previousMoney[playerID] = newAmount;
        }
    }

    private IEnumerator UpdateMoneyCoroutine(int playerID)
    {
        if (previousMoney[playerID] < money[playerID])
        {
            while (previousMoney[playerID] < money[playerID])
            {
                previousMoney[playerID]++;
                moneyDisplay[playerID].GetComponent<TMP_Text>().text = "$" + previousMoney[playerID].ToString();
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (previousMoney[playerID] > money[playerID])
            {
                previousMoney[playerID]--;
                moneyDisplay[playerID].GetComponent<TMP_Text>().text = "$" + previousMoney[playerID].ToString();
                yield return new WaitForSeconds(0.01f);
            }
        }
        }
    }
