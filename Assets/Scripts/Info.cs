using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public static Info Instance;

    public TextMeshProUGUI infoText;

	private void Awake()
	{
		Instance = this;
		infoText.text = "";
	}

	public void ShowMessage(string text)
	{
		infoText.text = text;	
	}
}
