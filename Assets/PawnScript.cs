using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            while (moveToNextNode(nextPosition))
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.25f);
            steps--;
        }

        isMoving = false;
    }

    bool moveToNextNode(Vector3 goal)
    {
        this.transform.position = Vector3.MoveTowards(transform.position, goal, 50 * Time.deltaTime);
        return goal != (this.transform.position);
    }
}
