using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public enum PlayerTypes {
    HUMAN,
    CPU,
    OTHER
}

public class Player : MonoBehaviour {
    public PawnScript[] myPawn;
    public bool hasTurn;
    public bool hasWon;
    public PlayerTypes playerType;
    public string playerName;
    public int money;
    public List<GameField> properties;
    private int currentFieldId;
    private GameField currentField;
    private GameManager gameManager;
    public bool isInJail;
    public int jailTurns;
    public InventoryScript inventory;

    void Start() {
        Debug.Log("Player is data loaded: " + GameManager.dataLoaded);
        if (!GameManager.dataLoaded) {
            playerName = name;
            money = 1500; // Initial money for each player
            properties = new List<GameField>();
            jailTurns = 0;
            isInJail = false;
        }
    }

    void Update() {

    }

    public string BuyPay() {
        if (currentField.isForSale && currentField.GetOwner() == null) {
            if (this.DeductMoney(currentField.cost)) {
                currentField.SetOwner(this);
                this.AddProperty(currentField.id, false);
                inventory.addCard(this, currentField.id);
                return playerName + " bought " + currentField.GetFieldName();
            }
        }

        return "";
    }

    public string PayRent() {
        if (currentField.GetOwner() != null && !this.properties.Any(field => field.id == currentField.id)) {
            {
                Player owner = currentField.GetOwner();

                int rent = currentField.GetRent();
                bool rentPaid = this.DeductMoney(rent);
                if (owner != null && owner != this && owner != isInJail) {
                    if (rentPaid) {
                        owner.AddMoney(rent);
                    }

                    return this.playerName + " paid rent to " + owner.playerName;
                }
            }
        }

        return "";
    }

    public string Tax() {
        if (currentField.GetFieldType() == FieldType.Tax) {
            int multiplicator = 1;

            if (currentField.GetFieldSubtype() == FieldSubtype.Retake) {
                multiplicator = Random.Range(0, 6);
                return this.playerName + " paid for " + multiplicator + " ECTS";
            } else {
                int tax = currentField.GetRent();
                bool rentPaid = this.DeductMoney(multiplicator * tax);
                return this.playerName + " paid Tax!";
            }

        }

        return "";
    }

    public string ReachedStart() {
        if (currentField.GetFieldType() == FieldType.Start) {
            this.AddMoney(400);
            return playerName + " START +400";
        }

        return "";
    }
    // Money management
    public void AddMoney(int amount) {
        money += amount;
    }

    public bool DeductMoney(int amount) {
        if (money >= amount) {
            money -= amount;
            return true;
        } else {
            Debug.Log("Insufficient funds!");
            return false;
        }
    }

    // Property management
    public void AddProperty(int fieldId, bool isLoaded) {
        GameField property = null;

        foreach (GameField gameField in GameManager.fields) {
            if (gameField.id == fieldId) {
                property = gameField;
                gameField.SetOwner(this);
            }
        }

        if (!isLoaded) {
            currentField.SetRent();
        }

        properties.Add(property);
    }

    public void RemoveProperty(int fieldId) {
        GameField property = null;

        foreach (GameField gameField in GameManager.fields) {
            if (gameField.id == fieldId) {
                property = gameField;
                gameField.SetOwner(null);
            }
        }


        properties.Remove(property);
    }

    public bool IsInJail() {
        return isInJail;
    }

    public string GoToJail() {
        isInJail = true;
        jailTurns = 3; // Reset the jail turns counter
        Debug.Log(playerName + " took a leave!");
        return playerName + " took a leave!";
    }

    public string GetOutOfJail() {
        isInJail = false;
        jailTurns = 0; // Reset the jail turns counter
        Debug.Log(playerName + " got back!");
        return playerName + " got back!";
    }

    public void DecrementJailTurns() {
        jailTurns--;
        Debug.Log("Jail Turns: " + jailTurns);
    }

    public bool CanGetOutOfJail() {
        return jailTurns <= 0; // Player can get out of jail after three turns
    }

    public void SetCurrentField(int fieldId) {
        currentField = GameManager.fields.Find(field => field.id == fieldId);
    }

    public GameField GetCurrentField() {
        return currentField;
    }


    public void SetIdOfCurrentField(int fieldId) {
        currentFieldId = fieldId;
    }

    public int GetIdOfCurrentField() {

        return currentFieldId;
    }

    public List<int> getListOfFieldsId() {
        List<int> list = new List<int>();
        foreach (GameField property in properties) {
            list.Add(property.id);

        }
        return list;
    }

    public void BuyFieldsAfterLoading(List<int> gameFields) {
        foreach (int i in gameFields) {
            AddProperty(i, true);
        }
    }
}