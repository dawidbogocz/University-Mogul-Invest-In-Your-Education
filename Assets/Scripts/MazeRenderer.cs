using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject MazeCellPrefab;

    public float CellSize = 1f;

	private void Start()
	{
		MazeCell[,] maze = mazeGenerator.GetMaze();

		for (int i = 0; i < mazeGenerator.mazeWidth; i++) 
		{ 
			for (int j = 0; j < mazeGenerator.mazeHeight; j++)
			{
				GameObject newCell = Instantiate(MazeCellPrefab, new Vector3((float)i * CellSize, 0f, (float)j * CellSize), Quaternion.identity, transform);

				MazeNode mazeCell = newCell.GetComponent<MazeNode>();

				bool top = maze[i, j].topWall;
				bool left = maze[i, j].leftWall;

				bool right = false;
				bool bottom = false;

				if( i == mazeGenerator.mazeWidth-1) { right = true;}
				if ( j == 0) { bottom = true;}

				mazeCell.Init(top, bottom, right, left);
			}
		}

	}
}
