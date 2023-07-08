using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReachedStart : MonoBehaviour
{
	private Player player;
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other);
		player = other.GetComponent<Player>();
		Debug.Log(player);
		player.AddMoney(400);
		Debug.Log(player.money);
	}
}
