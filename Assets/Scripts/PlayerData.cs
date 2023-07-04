using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public string playerName;
    public int money;
    public List<GameField> properties;
    //public GameField currentField;
    public int currentFieldId;
    public bool isInJail;
    public int jailTurns;
    public PlayerTypes playerType;
    public List<int> gameFields;

}

