using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string playerName;
    public int money;
    public List<GameField> properties;
    public bool isInJail;
    public int jailTurns;

    public Player(string name)
    {
        playerName = name;
        money = 1500; // Initial money for each player
        properties = new List<GameField>();
        jailTurns=0;
        isInJail = false;
    }

    // Money management
    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool DeductMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        else
        {
            Debug.Log("Insufficient funds!");
            return false;
        }
    }

    // Property management
    public void AddProperty(int fieldId)
    {
        GameField property = null;

        GameObject fields = GameObject.Find("Fields");

        GameField[] gameFields = fields.GetComponentsInChildren<GameField>();

        foreach (GameField gameField in gameFields) {
            if (gameField.id == fieldId) {
                property = gameField;
            }
        }

        properties.Add(property);
    }

    public void RemoveProperty(int fieldId) {
       GameField property = null;

       GameObject fields = GameObject.Find("Fields");

       GameField[] gameFields = fields.GetComponentsInChildren<GameField>();

       foreach (GameField gameField in gameFields) {
            if (gameField.id == fieldId) {
                property = gameField;
            }
        }

       
        properties.Remove(property);
    }

    public bool IsInJail()
    {
        return isInJail;
    }

    public void GoToJail()
    {
        isInJail = true;
        jailTurns = 0; // Reset the jail turns counter
    }

    public void GetOutOfJail()
    {
        isInJail = false;
        jailTurns = 0; // Reset the jail turns counter
    }

    public void IncrementJailTurns()
    {
        jailTurns++;
    }

    public bool CanGetOutOfJail()
    {
        return jailTurns >= 3; // Player can get out of jail after three turns
    }

}