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
    [SerializeField] private Sprite[] sprites = new Sprite[8];
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
        switchPlayersInventory(0);
    }

    void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            if(showingDeck[i])
            {
                transform.Find("MoneyAmount").GetComponent<TMP_Text>().text = "$" + players[i].money.ToString();
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

        GameObject field = fields.transform.GetChild(cardID).gameObject;

        card.GetComponent<Image>().sprite = sprites[((int)field.GetComponent<GameField>().GetFieldColor())];

        card.transform.SetParent(deck[playerID].transform, false);

        card.name = field.name + " CARD";

        card.transform.Find("PropertyName").GetComponent<TextMeshProUGUI>().text = field.name;
        card.transform.Find("Rent").GetComponent<TextMeshProUGUI>().text = "Rent: $" + field.GetComponent<GameField>().GetRent();
        card.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: $" + field.GetComponent<GameField>().cost;

        Vector3 pos = card.transform.localPosition;
        pos.y -= deck[playerID].transform.childCount * 12;
        card.transform.localPosition = pos;

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
                StartCoroutine(showDeckCoroutine(i));
            }
			else
			{
				colors.normalColor = new Color(0.25f, 0.25f, 0.25f);
                showingDeck[i] = false;
                StartCoroutine(hideDeckCoroutine(i));
            }
            transform.Find("InventoryButtons").GetChild(i).GetComponent<Button>().colors = colors;
		}
    }

    private IEnumerator showDeckCoroutine(int deckID)
    {
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

    private IEnumerator hideDeckCoroutine(int deckID)
    {
        Vector3 currentPosition = deck[deckID].transform.localPosition;
        Vector3 targetPosition = deck[deckID].transform.localPosition;
        targetPosition.x = 250;
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
        StartCoroutine(showCardCoroutine(card));
    }

    private IEnumerator showCardCoroutine(GameObject card)
    {
        Vector3 currentPosition = card.transform.localPosition;
        Vector3 targetPosition = card.transform.localPosition;
        targetPosition.x = -100;
        float i = 0;
        showingCard[card.name] = true;

        while (showingCard[card.name] == true && card.transform.localPosition.x > targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI/2)));
            card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, targetPosition, t);
            i += 0.01f;
            yield return null;
        }
    }

    public void hideCard(GameObject card)
    {
        StartCoroutine(hideCardCoroutine(card));
    }

    private IEnumerator hideCardCoroutine(GameObject card)
    {
        Vector3 currentPosition = card.transform.localPosition;
        Vector3 targetPosition = card.transform.localPosition;
        targetPosition.x = 0;
        float i = Mathf.PI / 2;
        showingCard[card.name] = false;

        while (showingCard[card.name] == false && card.transform.localPosition.x < targetPosition.x)
        {
            float t = (Mathf.Sin(Mathf.Clamp(i, 0, Mathf.PI / 2)));
            card.transform.localPosition = Vector3.Lerp(targetPosition, card.transform.localPosition, t);
            i -= 0.01f;
            yield return null;
        }
    }
}
