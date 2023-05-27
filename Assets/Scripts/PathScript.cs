using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    Transform[] childObjects;
    public List<Transform> childNodeList = new List<Transform>();

	private void Start()
	{
		FillNodes();
	}

	void FillNodes()
    {
        childNodeList.Clear();

        foreach(Transform child in transform)
        {
                childNodeList.Add(child);
            
        }
    }
}
