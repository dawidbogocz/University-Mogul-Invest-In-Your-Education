using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;

public class PawnScript : MonoBehaviour
{

    [SerializeField] PathScript currentPath;
    public int fieldID;
    bool isMoving;
    bool hasTurn;
    Player player;

    public void Start()
    {
        player = GetComponent<Player>();
    }

    private IEnumerator MoveCoroutine(float jumpHeight = 0.5f)
    {
        GameManager.Instance.skipButton.SetActive(false);
        Vector3 nextFieldPosition = currentPath.childNodeList[fieldID].position;
        Vector3 fieldSize = currentPath.childNodeList[fieldID].GetComponent<MeshRenderer>().bounds.size;
        float fieldHeight = currentPath.childNodeList[fieldID].GetComponent<MeshRenderer>().bounds.max.y;
        nextFieldPosition.y = fieldHeight + 0.01f;
        Vector3 nextPosition = nextFieldPosition;
        float range = 0.1f;
        nextPosition.x += Random.Range(-fieldSize.x * range, fieldSize.x * range);
        nextPosition.z += Random.Range(-fieldSize.z * range, fieldSize.z * range);
        Vector3 checkBoxSize = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size;
        checkBoxSize.y = 0;
        if (Physics.CheckBox(nextPosition, checkBoxSize))
        {
            bool readjustPosition = true;
            range = 0.25f;
            Stopwatch stopwatch = new Stopwatch(); ;
            stopwatch.Start();
            while (readjustPosition)
            {
                nextPosition = nextFieldPosition;
                nextPosition.x += Random.Range(-fieldSize.x * range, fieldSize.x * range);
                nextPosition.z += Random.Range(-fieldSize.z * range, fieldSize.z * range);
                if (!Physics.CheckBox(nextPosition, checkBoxSize))
                {
                    readjustPosition = false;
                }
                else if(stopwatch.ElapsedMilliseconds > 10)
                {
                    UnityEngine.Debug.Log("COULDN'T FIND VALID PAWN POSITION!!!!!!!!!!!!!!!!!!!!!!!!!");
                    readjustPosition = false;
                }
                else if (range < 0.4f)
                {
                    range += 0.001f;
                }
            }
        }

        Vector3 startPosition = transform.position;

        float duration;
        if (jumpHeight > 0.5f)
        {
            duration = 2f + Vector3.Distance(transform.position, nextPosition) / 10;
        }
        else
        {
            duration = 0.5f;
        }

        transform.DOMoveY(nextPosition.y + jumpHeight, duration * 0.25f).SetEase(Ease.OutQuad);

        transform.DOMoveX(nextPosition.x, duration * 0.25f).SetDelay(duration * 0.25f).SetEase(Ease.InOutQuad);
        transform.DOMoveZ(nextPosition.z, duration * 0.25f).SetDelay(duration * 0.25f).SetEase(Ease.InOutQuad);

        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, 90f, 0f);
        targetRotation = Quaternion.Euler(0f, (fieldID / 10) * 90, 0f);
        transform.DORotateQuaternion(targetRotation, duration * 0.5f).SetDelay(duration * 0.25f).SetEase(Ease.OutQuad);

        transform.DOMoveY(nextPosition.y, duration * 0.25f).SetDelay(duration * 0.5f).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(duration);
        //player.ReachedStart();

        /*if (diceNumber < 6)
		{
			GameManager.Instance.state = State.BUYING;
		}
		else if (diceNumber == 6)
		{
			GameManager.Instance.state = State.ROLL_DICE;
		}
		else
		{
			GameManager.Instance.state = State.SWITCH_PLAYER;
		}*/

        //player.Action();
    }

    public void MoveForward(int diceNumber)
    {
        StartCoroutine(MoveForwardCoroutine(diceNumber));
    }

    IEnumerator MoveForwardCoroutine(int diceNumber)
    {
        for (int i = 0; i < diceNumber; i++)
        {
            this.fieldID++;
            this.fieldID %= 40;
            yield return StartCoroutine(MoveCoroutine());
        }
        player.SetCurrentField(fieldID);
        GameManager.Instance.state = State.BUYING;
    }

    public void MoveToField(int fieldId)
    {
        StartCoroutine(MoveToFieldCoroutine(fieldId));
    }

    IEnumerator MoveToFieldCoroutine(int fieldId)
    {
        this.fieldID = fieldId;
        yield return StartCoroutine(MoveCoroutine(5));
        player.SetCurrentField(fieldID);
        GameManager.Instance.state = State.BUYING;
    }
}