using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
	[SerializeField] MazeNode nodePrefab;
	[SerializeField] Vector2Int mazeSize;
	[SerializeField] float nodeSize;
	[SerializeField] float playerSpeed;

	public Transform player;
	public Transform goal;
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI congratulationsText;
	private float timer = 30f;
	public float arrivalThreshold = 0.5f;
	private bool isGameRunning = true;
	private bool hasWon = false;

	private void Start()
	{
		GenerateMazeInstant(mazeSize);
		SetPlayerAndGoalPosition();
	}

	private void Update()
	{
		if (isGameRunning)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				isGameRunning = false;
				Debug.Log("Time's up! Game over.");
			}

			UpdateTimerText();
			MovePlayer();

			if (Input.GetKeyDown(KeyCode.R))
			{
				ResetGame();
			}
			if (!hasWon)
			{
				Vector3 playerPosition = player.position;
				Vector3 goalPosition = goal.position;

				float distance = Vector3.Distance(playerPosition, goalPosition);

				if (distance <= arrivalThreshold)
				{
					hasWon = true;
					Debug.Log("Congratulations! You reached the goal!");

					isGameRunning = false;
					// Show the congratulatory message or perform other win-related actions here
					StartCoroutine(ShowCongratulationsMessageAndEndGame("Congratulations! You reached the goal. You don't have to pay.", 2f));
				}
			}
		}
	}

	private void UpdateTimerText()
	{
		timerText.text = "Time: " + timer.ToString("0.00");
	}
	void GenerateMazeInstant(Vector2Int size)
	{
		List<MazeNode> nodes = new List<MazeNode>();

		// Create nodes
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				Vector3 nodePos = new Vector3(x - (size.x / 2f), 0, y - (size.y / 2f));
				MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
				nodes.Add(newNode);
			}
		}

		List<MazeNode> currentPath = new List<MazeNode>();
		List<MazeNode> completedNodes = new List<MazeNode>();

		// Choose starting node
		currentPath.Add(nodes[Random.Range(0, nodes.Count)]);

		while (completedNodes.Count < nodes.Count)
		{
			// Check nodes next to the current node
			List<int> possibleNextNodes = new List<int>();
			List<int> possibleDirections = new List<int>();

			int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
			int currentNodeX = currentNodeIndex / size.y;
			int currentNodeY = currentNodeIndex % size.y;

			if (currentNodeX < size.x - 1)
			{
				// Check node to the right of the current node
				if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
					!currentPath.Contains(nodes[currentNodeIndex + size.y]))
				{
					possibleDirections.Add(1);
					possibleNextNodes.Add(currentNodeIndex + size.y);
				}
			}
			if (currentNodeX > 0)
			{
				// Check node to the left of the current node
				if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
					!currentPath.Contains(nodes[currentNodeIndex - size.y]))
				{
					possibleDirections.Add(2);
					possibleNextNodes.Add(currentNodeIndex - size.y);
				}
			}
			if (currentNodeY < size.y - 1)
			{
				// Check node above the current node
				if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
					!currentPath.Contains(nodes[currentNodeIndex + 1]))
				{
					possibleDirections.Add(3);
					possibleNextNodes.Add(currentNodeIndex + 1);
				}
			}
			if (currentNodeY > 0)
			{
				// Check node below the current node
				if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
					!currentPath.Contains(nodes[currentNodeIndex - 1]))
				{
					possibleDirections.Add(4);
					possibleNextNodes.Add(currentNodeIndex - 1);
				}
			}

			// Choose next node
			if (possibleDirections.Count > 0)
			{
				int chosenDirection = Random.Range(0, possibleDirections.Count);
				MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

				switch (possibleDirections[chosenDirection])
				{
					case 1:
						chosenNode.RemoveWall(1);
						currentPath[currentPath.Count - 1].RemoveWall(0);
						break;
					case 2:
						chosenNode.RemoveWall(0);
						currentPath[currentPath.Count - 1].RemoveWall(1);
						break;
					case 3:
						chosenNode.RemoveWall(3);
						currentPath[currentPath.Count - 1].RemoveWall(2);
						break;
					case 4:
						chosenNode.RemoveWall(2);
						currentPath[currentPath.Count - 1].RemoveWall(3);
						break;
				}

				currentPath.Add(chosenNode);
			}
			else
			{
				completedNodes.Add(currentPath[currentPath.Count - 1]);

				currentPath.RemoveAt(currentPath.Count - 1);
			}
		}
	}

	void SetPlayerAndGoalPosition()
	{
		List<MazeNode> nodes = new List<MazeNode>(GetComponentsInChildren<MazeNode>());

		MazeNode randomNode = nodes[Random.Range(0, nodes.Count)];
		player.position = randomNode.transform.position;

		MazeNode goalNode = randomNode;
		while (goalNode == randomNode)
		{
			goalNode = nodes[Random.Range(0, nodes.Count)];
		}
		goal.position = goalNode.transform.position;
	}


	IEnumerator ShowCongratulationsMessageAndEndGame(string message, float duration)
	{
		congratulationsText.text = message;

		yield return new WaitForSeconds(duration);

		//EndGame();
		//ResetGame();
	}

	void MovePlayer()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
		movement.Normalize(); 
		movement *= playerSpeed * Time.deltaTime;
		movement.y = 0f; 

		player.Translate(movement);
	}

	void ResetGame()
	{
		isGameRunning = true;
		timer = 30f;
		UpdateTimerText();

		SetPlayerAndGoalPosition();
	}
}