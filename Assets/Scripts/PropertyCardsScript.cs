using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class PropertyCardsScript : MonoBehaviour
{
    [SerializeField] private PathScript fields;
    [SerializeField] private GameObject propertyCard;
    [SerializeField] private Sprite[] sprites = new Sprite[8];
    [SerializeField] private GameObject testPlayer;
    private int yPosition = 0;
    private Dictionary<string, bool> showingCard = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        addCard(testPlayer, "DS SOLARIS");
        addCard(testPlayer, "MINING");
        addCard(testPlayer, "SPIRALA");
        addCard(testPlayer, "DS BABILON");
        addCard(testPlayer, "TRANSPORT");
        addCard(testPlayer, "PARK");
        addCard(testPlayer, "DS STRZECHA");
        addCard(testPlayer, "MT");
        addCard(testPlayer, "ZAJADALNIA");
        addCard(testPlayer, "DS PIAST");
        addCard(testPlayer, "CIVIL ENGINEERING");
        addCard(testPlayer, "ARCHITECTURE");
        addCard(testPlayer, "DS ZIEMOWIT");
        addCard(testPlayer, "OSIR");
        addCard(testPlayer, "MROWISKO");
        addCard(testPlayer, "CHEMISTRY");
        addCard(testPlayer, "DS BARBARA");
        addCard(testPlayer, "ELECTRICAL ENGINEERING");
        addCard(testPlayer, "DS ELEKTRON");
        addCard(testPlayer, "STOLOWKA");
        addCard(testPlayer, "DS ONDRASZEK");
        addCard(testPlayer, "AEII");
    }

    public void addCard(GameObject player, string cardName)
    {
        GameObject card = Instantiate(propertyCard);

        GameObject field = fields.transform.Find(cardName).gameObject;

        string materialName = field.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterials[0].name;
        int materialID = materialName[materialName.Length - 1] - '0';
        card.GetComponent<Image>().sprite = sprites[materialID - 1];

        card.transform.SetParent(player.transform, false);

        card.name = cardName + " CARD";

        card.transform.Find("PropertyName").GetComponent<TextMeshProUGUI>().text = cardName;
        card.transform.Find("Rent").GetComponent<TextMeshProUGUI>().text = "Rent: " + field.GetComponent<GamieField>().rent;
        card.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost: " + field.GetComponent<GamieField>().cost;

        Vector3 pos = card.transform.position;
        pos.y += yPosition;
        card.transform.position = pos;
        yPosition -= 28;

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

        while (showingCard[card.name] == true)
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
