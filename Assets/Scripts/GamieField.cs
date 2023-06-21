using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for Field Types
public enum FieldType {
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

public class GamieField : MonoBehaviour
{

    public int id;
    public static string name;
    public static int cost;
    private int rent = cost / 2;
    private static FieldType type;         
    private static FieldSubtype subtype = FieldSubtype.None;
    // Start is called before the first frame update
    void Start()
    {
        setType(id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setType(int id) {
        if (id == 1 || id == 8 || id == 13 || id == 18 || id == 23 || id == 29 || id == 32 || id == 37) {
            type = FieldType.Dorm;
        } else if (id == 3 || id == 9 || id == 14 || id == 19 || id == 21 || id == 27 || id == 31 || id == 39) {
            type = FieldType.Faculty;
        } else if (id == 6 || id == 11 || id == 16 || id == 24 || id == 26 || id == 34) {
            type = FieldType.Recreation;
        } else if (id == 2 || id == 7 || id == 17 || id == 22 || id == 33 || id == 36) {
            type = FieldType.SpecialCard;

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
        }
    }
}
