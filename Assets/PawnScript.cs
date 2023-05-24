using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PawnScript : MonoBehaviour
{

    public PathScript currentPath;
    public int currentField;
    int steps;
    bool isMoving;

    public IEnumerator move(int steps)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {
            currentField++;
            currentField %= currentPath.childNodeList.Count;
            Vector3 nextFieldPosition = currentPath.childNodeList[currentField].position;
            Vector3 fieldSize = currentPath.childNodeList[currentField].GetComponent<MeshRenderer>().bounds.size;
            float fieldHeight = currentPath.childNodeList[currentField].GetComponent<MeshRenderer>().bounds.max.y;
            nextFieldPosition.y += fieldHeight + 0.0001f;
            Vector3 nextPosition = nextFieldPosition;
            nextPosition.x += Random.Range(-fieldSize.x * 0.1f, fieldSize.x * 0.1f);
            nextPosition.z += Random.Range(-fieldSize.z * 0.1f, fieldSize.z * 0.1f);
            Vector3 checkBoxSize = new Vector3(0.2f, 0, 0.2f);
            if (Physics.CheckBox(nextPosition, checkBoxSize))
            {
                bool readjustPosition = true;
                while (readjustPosition)
                {
                    nextPosition = nextFieldPosition;
                    nextPosition.x += Random.Range(-fieldSize.x * 0.4f, fieldSize.x * 0.4f);
                    nextPosition.z += Random.Range(-fieldSize.z * 0.4f, fieldSize.z * 0.4f);
                    if (!Physics.CheckBox(nextPosition, checkBoxSize))
                    {
                        readjustPosition = false;
                    }
                }
            }
            yield return MoveToNextNode(nextPosition);
            yield return new WaitForSeconds(0.25f);
            steps--;
        }

        isMoving = false;
    }

	IEnumerator MoveToNextNode(Vector3 goal)
	{
		Vector3 startPosition = transform.position;
		float duration = 0.5f;


		transform.DOMoveY(goal.y + 0.25f, duration * 0.25f).SetEase(Ease.OutQuad);


		transform.DOMove(goal, duration * 0.25f).SetDelay(duration * 0.25f).SetEase(Ease.InOutQuad);

		if ((currentField % 10) == 0)
		{
			Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, 90f);
			transform.DORotateQuaternion(targetRotation, duration * 0.5f).SetDelay(duration * 0.25f).SetEase(Ease.OutQuad);
		}

		yield return new WaitForSeconds(duration * 0.4f);
		transform.DOMoveY(goal.y, duration * 0.25f).SetEase(Ease.InQuad);

		

		yield return new WaitForSeconds(duration * 0.25f);
	}
}
