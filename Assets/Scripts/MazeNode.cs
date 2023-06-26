using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode : MonoBehaviour
{
	[SerializeField] GameObject topWall;
	[SerializeField] GameObject bottomWall;
	[SerializeField] GameObject rightWall;
	[SerializeField] GameObject leftWall;

	public List<GameObject> topWallPrefabs;
	public List<GameObject> bottomWallPrefabs;
	public List<GameObject> rightWallPrefabs;
	public List<GameObject> leftWallPrefabs;

	public void Init(bool top, bool bottom, bool right, bool left)
	{
		topWall.SetActive(top);
		bottomWall.SetActive(bottom);
		rightWall.SetActive(right);
		leftWall.SetActive(left);

		if (top) AssignRandomWallPrefab(topWall, topWallPrefabs);
		if (bottom) AssignRandomWallPrefab(bottomWall, bottomWallPrefabs);
		if (right) AssignRandomWallPrefab(rightWall, rightWallPrefabs);
		if (left) AssignRandomWallPrefab(leftWall, leftWallPrefabs);
	}

	void AssignRandomWallPrefab(GameObject wall, List<GameObject> prefabList)
	{
		if (prefabList.Count == 0)
		{
			Debug.LogWarning("No wall prefabs available!");
			return;
		}

		int randomIndex = Random.Range(0, prefabList.Count);
		GameObject selectedWallPrefab = prefabList[randomIndex];

		Instantiate(selectedWallPrefab, wall.transform.position, wall.transform.rotation);
		Destroy(wall);
	}
}