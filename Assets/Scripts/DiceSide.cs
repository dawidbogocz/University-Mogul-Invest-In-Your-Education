using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    bool onGround;
    public int sideValue;

	private void OnTriggerStay(Collider collider)
	{
		if (collider.CompareTag("DiceGround"))
		{
			onGround = true;
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("DiceGround"))
		{
			onGround = false;
		}
	}

	public bool OnGround()
    {
        return onGround;
    }
}
