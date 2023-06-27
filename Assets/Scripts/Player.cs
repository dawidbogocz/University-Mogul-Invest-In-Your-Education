using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    private GameField currentField;
    private GameManager gameManager;
    public bool isInJail;
    public int jailTurns;

    void Start() {
        playerName = name;
        money = 1500; // Initial money for each player
        properties = new List<GameField>();
        jailTurns = 0;
        isInJail = false;
    }

    void Update() {
        
    }

    public void BuyPay()
    {
		if (currentField.isForSale && currentField.GetOwner() == null)
		{
			if (this.DeductMoney(currentField.cost))
			{
				currentField.SetOwner(this);
				this.AddProperty(currentField.id);
				Debug.Log(playerName + " bought " + currentField.GetFieldName());
			}
		}
		if (currentField.GetOwner() != null && !this.properties.Any(field => field.id == currentField.id))
		{
			{
				Player owner = currentField.GetOwner();

				int rent = currentField.GetRent();
				bool rentPaid = this.DeductMoney(rent);
				if (owner != null && owner != this)
				{
					if (rentPaid)
					{
						owner.AddMoney(rent);
					}

					Debug.Log(this.playerName + " paid rent to " + owner.playerName);
				}
			}
		}
	}
    public void Tax()
    {
		if (currentField.GetFieldType() == FieldType.Tax)
		{
			int tax = currentField.GetRent();
			bool rentPaid = this.DeductMoney(tax);
			Debug.Log(this.playerName + " paid Tax!");
		}
	}

    public void ReachedStart()
    {
		if (currentField.GetFieldType() == FieldType.Start)
		{
			this.AddMoney(400);
			Debug.Log(playerName + " START +400");
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