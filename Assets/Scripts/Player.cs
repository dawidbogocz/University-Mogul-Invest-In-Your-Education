using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {
    public string playerName;
    public int money;
    public List<GameField> properties;
    private GameField currentField;
    public bool isInJail;
    public int jailTurns;

    public Player(string name) {
        playerName = name;
        money = 1500; // Initial money for each player
        properties = new List<GameField>();
        jailTurns = 0;
        isInJail = false;
    }

    void Start() {
        playerName = name;
        money = 1500; // Initial money for each player
        properties = new List<GameField>();
        jailTurns = 0;
        isInJail = false;
    }

    void Update() {
        
    }

    public void Action() {
        //Debug.Log("Action: " + currentField.GetFieldName() + " " + currentField.GetFieldType());
        if (currentField.GetFieldType() == FieldType.Start) {
            this.AddMoney(400);
            Debug.Log(playerName + " START +400 (" + currentField.id + ")");
        }

        if (currentField.isForSale && currentField.GetOwner() == null) {
            //TEST BUY
            if (this.DeductMoney(currentField.cost)) {
                currentField.SetOwner(this);
                this.AddProperty(currentField.id);
                Debug.Log(playerName + " bought " + currentField.GetFieldName());
            }
        }

        if ((currentField.GetOwner() != null && !this.properties.Any(field => field.id == currentField.id))
                || currentField.GetFieldType() == FieldType.Tax) {
            {
                Player owner = currentField.GetOwner();

                int rent = currentField.GetRent();
                bool rentPaid = this.DeductMoney(rent);
                if (owner != null && owner != this) {
                    if (rentPaid) {
                        owner.AddMoney(rent);
                    }

                    Debug.Log(this.playerName + " paid rent to " + owner.playerName);
                } else {
                    Debug.Log(this.playerName + " paid Tax!");
                }
            }
        }
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
    public void AddProperty(int fieldId) {
        GameField property = null;

        foreach (GameField gameField in GameManager.fields) {
            if (gameField.id == fieldId) {
                property = gameField;
                gameField.SetOwner(this);
            }
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

    public void GoToJail() {
        isInJail = true;
        jailTurns = 0; // Reset the jail turns counter
    }

    public void GetOutOfJail() {
        isInJail = false;
        jailTurns = 0; // Reset the jail turns counter
    }

    public void IncrementJailTurns() {
        jailTurns++;
    }

    public bool CanGetOutOfJail() {
        return jailTurns >= 3; // Player can get out of jail after three turns
    }

    public void SetCurrentField(int fieldId) {
        currentField = GameManager.fields.Find(field => field.id == fieldId);
    }

    public GameField GetCurrentField() {
        return currentField;
    }

    public int GetIdOfCurrentField(){

        return currentField.id;
    }
}