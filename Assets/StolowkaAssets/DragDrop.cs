using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private float dragSpeed;

    public int foodValue;
    public GameObject previousIngredient;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnPointerDown(PointerEventData eventData) {

    }

    public void OnBeginDrag(PointerEventData eventData) {

    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / 45;
    }

    public void OnEndDrag(PointerEventData eventData) {
        bool isCorrect = false;

        if(rectTransform.transform.position.x <= 1013 && rectTransform.transform.position.x >= 1000 
            && rectTransform.transform.position.y <= 2 && rectTransform.transform.position.y >= 0) {
            if (gameObject.name == "bunDown") {
                isCorrect = true;
            }
            else if (rectTransform.transform.position.y >= previousIngredient.transform.position.y / 2) {
                isCorrect = true;
            }
            else {
                Debug.Log("Current Y: " + rectTransform.transform.position.y + "\n");
                Debug.Log("Previous Y: " + previousIngredient.transform.position.y);
            }
        }

        if (isCorrect) {
            BurgerMaster.plateValue += foodValue;
        }
    }
}
