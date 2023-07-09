using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    Rigidbody rb;

    bool hasLanded;
    bool thrown;

    Vector3 initPos;

    public DiceSide[] diceSides;
    public int diceValue;

    void Start()
    {
        initPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

	public void RollDice()
	{
		Reset();
		if (!thrown && !hasLanded) 
        { 
            thrown = true;
            rb.useGravity = true;
            rb.AddTorque(Random.Range(1,25), Random.Range(1, 25), Random.Range(1, 25));
            rb.AddForce(Random.Range(-5, 5), 80, Random.Range(-5, 5));
        }
        else if(thrown && hasLanded) 
        {
            Reset();
        }
	}

	private void Reset()
	{
		transform.position = initPos;
        rb.isKinematic = false;
        thrown = false;
        hasLanded = false;
        rb.useGravity = false;
	}

	private void Update()
	{
		if(rb.IsSleeping() && !hasLanded && thrown)
        {
            hasLanded = true;
            rb.useGravity = false;
            rb.isKinematic = true;

            SideValueCheck();
        }
        else if (rb.IsSleeping() && hasLanded && diceValue == 0)
        {
            RollAgain();
        }
	}

    void RollAgain()
    {
        Reset();
        thrown = true;
        rb.useGravity = true;
        rb.AddTorque(Random.Range(1, 25), Random.Range(1, 25), Random.Range(1, 25));
        rb.AddForce(Random.Range(-5, 5), 80, Random.Range(-5, 5));
    }

    void SideValueCheck()
    {
        diceValue = 0;
        foreach(DiceSide side in diceSides)
        {
            if (side.OnGround())
            {
                diceValue = side.sideValue;
                GameManager.Instance.MovePlayerForward(diceValue);
            }
        }
    }
}
