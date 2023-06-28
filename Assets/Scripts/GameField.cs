using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Enum for Field Types
public enum FieldType {
    None,
    Start,
    Field,
    Dorm,
    Faculty,
    Recreation,
    SpecialCard,
    Tax,
    Elevator,
    Prison
}

// Enum for the field subtypes
public enum FieldSubtype {
    None,
    Risk,
    Chance,
    Retake
}

//Enum for the field Color
public enum FieldColor {
    Brown,
    Cyan,
    Pink,
    Orange,
    Red,
    Yellow,
    Green,
    Blue,
    None
}

public class GameField : MonoBehaviour
{

    public int id;
    public int cost;
    public TextMeshPro fieldText;
    public bool isForSale;
    private int rent;
    private string fieldName;
    private Player owner;
    private FieldType type;
    private FieldSubtype subtype;
    private FieldColor color;

    // Start is called before the first frame update
    void Start() {
        fieldName = name;
        owner = null;
        SetColor(id);
        SetType(id);

        if (type == FieldType.Tax) {
            if (subtype == FieldSubtype.Retake) {
                rent = 70 * 6;
            } else {
                rent = 400;
            }
        } else {
            rent = cost / 2;
        }

        if(cost != 0) {
            isForSale = true;
        } else {
            isForSale = false;
        }

        if (type == FieldType.Dorm || type == FieldType.Faculty || type == FieldType.Recreation) {
            fieldText.text = name + "\n$" + cost;
        }

        GameManager.fields.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetRent() {
        return rent;
    }
    public FieldType GetFieldType() {
        return type;
    }
    public FieldSubtype GetFieldSubtype() {
        return subtype;
    }
    public FieldColor GetFieldColor() {
        return color;
    }
    public string GetFieldName() {
        return fieldName;
    }

    public Player GetOwner() {
        return owner;
    }

    public void SetOwner(Player newOwner) {
        owner = newOwner;
    }

    void SetColor(int id) {
        if (id == 1 || id == 3) {
            color = FieldColor.Brown;
        } else if (id == 6 || id == 8 || id == 9) {
            color = FieldColor.Cyan;
        } else if (id == 11 || id == 13 || id == 14) {
            color = FieldColor.Pink;
        } else if (id == 16 || id == 18 || id == 19) {
            color = FieldColor.Orange;
        } else if (id == 21 || id == 23 || id == 24) {
            color = FieldColor.Red;
        } else if (id == 26 || id == 27 || id == 29) {
            color = FieldColor.Yellow;
        } else if (id == 31 || id == 32 || id == 34) {
            color = FieldColor.Green;
        } else if (id == 37 || id == 39) { 
            color = FieldColor.Blue;
        } else {
            color = FieldColor.None;
        }
    }
    void SetType(int id) {
        subtype = FieldSubtype.None;

        if (id == 1 || id == 8 || id == 13 || id == 18 || id == 23 || id == 29 || id == 32 || id == 37) {
            type = FieldType.Dorm;
        } else if (id == 3 || id == 9 || id == 14 || id == 19 || id == 21 || id == 27 || id == 31 || id == 39) {
            type = FieldType.Faculty;
        } else if (id == 6 || id == 11 || id == 16 || id == 24 || id == 26 || id == 34) {
            type = FieldType.Recreation;
        } else if (id == 2 || id == 7 || id == 17 || id == 22 || id == 33 || id == 36) {
            type = FieldType.SpecialCard;
            isForSale = false;

            if (id == 2 || id == 17 || id == 33) {
                subtype = FieldSubtype.Risk;
            } else {
                subtype = FieldSubtype.Chance;
            }
        } else if (id == 10 || id == 12 || id == 20 || id == 28 || id == 30) {
            type = FieldType.Field;
        } else if (id == 5 || id == 15 || id == 25 || id == 35) {
            type = FieldType.Elevator;
        }
        else if (id == 4 || id == 38) {
            type = FieldType.Tax;

            if (id == 38) {
                subtype = FieldSubtype.Retake;
            }
        } else if (id == 40) {
            type = FieldType.Prison;
        } else if (id == 0) {
            type = FieldType.Start;
        } else { 
            type = FieldType.None; }
    }

}
