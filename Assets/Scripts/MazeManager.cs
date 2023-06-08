using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeManager : MonoBehaviour
{
	public GameObject trophyPrefab;
	public GameObject lightBeamPrefab;
	private Transform playerTransform;
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI messageText;

	private GameObject trophyInstance;
	private GameObject lightBeamInstance;
	private bool isGameRunning = false;
	private float timerDuration = 30f;
	private float currentTime;

	private bool hasWon = false;
	void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		StartGame();
	}

	void StartGame()
	{
		isGameRunning = true;
		currentTime = timerDuration;
		StartCoroutine(StartTimer());
		GenerateTrophy();
		UpdateMessageText("");
	}

	IEnumerator StartTimer()
	{
		while (currentTime > 0f && !hasWon)
		{
			yield return new WaitForSeconds(1f);
			currentTime--;
			UpdateTimerUI();
		}
		if (!hasWon)
		{
			EndGame("Game Over");
		}
	}

	void EndGame(string message)
	{
		isGameRunning = false;
		Destroy(trophyInstance);
		Destroy(lightBeamInstance);
		UpdateMessageText(message);
		// Add any additional end game logic here
	}

	void GenerateTrophy()
	{
		Vector3 randomPos = new Vector3(Random.Range(5f, 15f), 0f, Random.Range(5f, 15f));
		trophyInstance = Instantiate(trophyPrefab, randomPos, Quaternion.identity);
		randomPos[1] = 2f;
		lightBeamInstance = Instantiate(lightBeamPrefab, randomPos, Quaternion.identity);
		// Add any additional trophy setup logic here
	}

	void UpdateTimerUI()
	{
		timerText.text = "Time: " + currentTime.ToString("F0") + "s";
	}

	void UpdateMessageText(string message)
	{
		messageText.text = message;
	}

	void Update()
	{
		if (isGameRunning && Vector3.Distance(playerTransform.position, trophyInstance.transform.position) < 0.5f)
		{
			hasWon = true;
			EndGame("You Won!");
			// Add any additional logic for when the player picks up the trophy here
		}
	}
}

