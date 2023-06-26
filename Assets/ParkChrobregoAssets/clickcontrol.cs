using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickcontrol : MonoBehaviour
{
    public static int remainingItems = 5;
    public static string nameOfObj;
    public GameObject objNameText;
        // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
            nameOfObj = gameObject.name;
            Debug.Log(nameOfObj);
            Destroy(gameObject);
            Destroy(objNameText);
            remainingItems--;
    }
}
