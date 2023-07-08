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
    private GameObject[] deck = new GameObject[4];
    private Dictionary<string, bool> showingCard = new Dictionary<string, bool>();
    private Dictionary<int, bool> showingDeck = new Dictionary<int, bool>();
    [SerializeField] private Player[] players = new Player[4];

    // Start is called before the first frame update
    void Start() 
    {
        for (int i = 0; i < 4; i++) 
        {
            deck[i] = transform.Find("Decks").GetChild(i).gameObject;
        }
        //StartCoroutine(test());
    }

    private IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        addCard(players[0], 1);
        addCard(players[0], 3);
        addCard(players[0], 6);
        addCard(players[0], 8);
        addCard(players[0], 9);
        addCard(players[0], 11);
        addCard(players[0], 13);
        addCard(players[0], 14);
        addCard(players[0], 16);
        addCard(players[0], 18);
        addCard(players[0], 19);
        addCard(players[0], 21);
        addCard(players[0], 23);
        addCard(players[0], 24);
        addCard(players[0], 26);
        addCard(players[0], 27);
        addCard(players[0], 29);
        addCard(players[0], 31);
        addCard(players[0], 32);
        addCard(players[0], 34);
        addCard(players[0], 37);
        addCard(players[0], 39);
        addCard(players[0], 5);
        addCard(players[0], 12);
        addCard(players[0], 15);
        addCard(players[0], 25);
        addCard(players[0], 28);
        addCard(players[0], 35);
    }

    void Update()
    {
        if (showingDeck.Count > 0) {
            for (int i = 0; i < 4; i++) {
                if (showingDeck[i]) {
                    transform.Find("MoneyAmount").GetComponent<TMP_Text>().text = "$" + players[i].money.ToString();
                }
            }
        }
    }

    public void addCard(Player player, int cardID)
    {
        int playerID = 0;
        for(int i = 0; i < 4; i++)
        {
            if(player == players[i])
            {
                playerID = i;
            }
        }
        GameObject card = Instantiate(propertyCard);
        GameObject cardSprite = card.transform.Find("Card").gameObject;
        GameObject field = fields.transform.GetChild(cardID).gameObject;

        cardSprite.GetComponent<Image>().sprite = sprites[((int)field.GetComponent<GameField>().GetFieldColor())];

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
        entry1.callback.AddListener( (eventData) => { showCard(card); } );
        trigger.triggers.Add(entry1);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerExit;
        entry2.callback.AddListener((eventData) => { hideCard(card); });
        trigger.triggers.Add(entry2);

        showingCard[card.name] = false;
        StartCoroutine(moveCardLeft(card, 0));
    }

    public void switchPlayersInventory(int playerID)
    {
        for (int i = 0; i < 4; i++)
		{
            ColorBlock colors = transform.Find("InventoryButtons").GetChild(i).GetComponent<Button>().colors;
            if (i == playerID)
			{

				colors.normalColor = new Color(1, 1, 1);
                showingDeck[i] = true;
                StartCoroutine(moveDeckLeft(i));
            }
			else
			{
				colors.normalColor = new Color(0.25f, 0.25f, 0.25f);
                showingDeck[i] = false;
                StartCoroutine(moveDeckRight(i));
            }
            transform.Find("InventoryButtons").GetChild(i).GetComponent<Button>().colors = colors;
		}
    }

    private IEnumerator moveDeckLeft(int deckID)
    {
        if(deck[deckID] == null)
        {
            yield return new WaitForSeconds(1);
        }
        Vector3 currentPosition = deck[deckID].transform.localPosition;
        Vector3 targetPosition = deck[deckID].transform.localPosition;
        targetPosition.x = 0;
        float i = 0;
        showingDeck[deckID] = true;

        while (showingDeck[deckID] == true && deck[deckID].transform.localPosition.x > targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            deck[deckID].transform.localPosition = Vector3.Lerp(deck[deckID].transform.localPosition, targetPosition, t);
            i += 0.01f;
            yield return null;
        }
    }

    private IEnumerator moveDeckRight(int deckID)
    {
        if (deck[deckID] == null)
        {
            yield return new WaitForSeconds(1);
        }
        Vector3 currentPosition = deck[deckID].transform.localPosition;
        Vector3 targetPosition = deck[deckID].transform.localPosition;
        targetPosition.x = 300;
        float i = Mathf.PI / 2;
        showingDeck[deckID] = false;

        while (showingDeck[deckID] == false && deck[deckID].transform.localPosition.x < targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            deck[deckID].transform.localPosition = Vector3.Lerp(targetPosition, deck[deckID].transform.localPosition, t);
            i -= 0.01f;
            yield return null;
        }
    }

    public void showCard(GameObject card)
    {
        StartCoroutine(moveCardLeft(card, -130));
    }

    private IEnumerator moveCardLeft(GameObject cardObject, int targetX)
    {
        Transform card = cardObject.transform.Find("Card");
        Vector3 currentPosition = card.transform.localPosition;
        Vector3 targetPosition = card.transform.localPosition;
        targetPosition.x = targetX;
        float i = 0;
        showingCard[cardObject.name] = true;

        while (showingCard[cardObject.name] == true && card.transform.localPosition.x > targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI/2)));
            card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, targetPosition, t);
            i += 0.01f;
            yield return null;
        }
    }

    public void hideCard(GameObject card)
    {
        StartCoroutine(moveCardRight(card, 0));
    }

    private IEnumerator moveCardRight(GameObject cardObject, int targetX)
    {
        Transform card = cardObject.transform.Find("Card");
        Vector3 currentPosition = card.transform.localPosition;
        Vector3 targetPosition = card.transform.localPosition;
        targetPosition.x = targetX;
        float i = Mathf.PI / 2;
        showingCard[cardObject.name] = false;

        while (showingCard[cardObject.name] == false && card.transform.localPosition.x < targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            card.transform.localPosition = Vector3.Lerp(targetPosition, card.transform.localPosition, t);
            i -= 0.01f;
            yield return null;
        }
    }
}
