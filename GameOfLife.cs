using UnityEngine;

public class GameOfLife : ProcessingLite.GP21
{
	GameCell[,] cells; //Our game grid matrix
	float cellSize = 0.25f; //Size of our cells
	int numberOfColums;
	int numberOfRows;
	int spawnChancePercentage = 15;

	void Start()
	{
		//Lower framerate makes it easier to test and see whats happening.
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 8;
		//Calculate our grid depending on size and cellSize
		numberOfColums = (int)Mathf.Floor(Width / cellSize);
		numberOfRows = (int)Mathf.Floor(Height / cellSize);

		//Initiate our matrix array
		cells = new GameCell[numberOfColums, numberOfRows];

		//Create all objects

		//For each row
		for (int y = 0; y < numberOfRows; ++y)
		{
			//for each column in each row
			for (int x = 0; x < numberOfColums; ++x)
			{
				//Create our game cell objects, multiply by cellSize for correct world placement
				cells[x, y] = new GameCell(x * cellSize, y * cellSize, cellSize);

				//Random check to see if it should be alive
				if (Random.Range(0, 100) < spawnChancePercentage)
				{
					cells[x, y].alive = true;
				}
			}
		}
	}

	void Update()
	{
		//Clear screen
		Background(0);

        //TODO: Calculate next generation
			for (int y = 0; y < numberOfRows; ++y)
			{
				for (int x = 0; x < numberOfColums; ++x)
				{
					int neighbours = 0;

				for (int e = -1; e < 2; e++)
				{
					if (x + e > 0 && x + e < numberOfColums && y + 1 < numberOfRows)
					{
						if (cells[x + e, y + 1].alive)
						{
							neighbours++;
						}
					}
				}
				for (int e = -1; e < 2; e++)
				{
					if (x + e > 0 && x + e < numberOfColums && y - 1 > 0)
					{
						if (cells[x + e, y - 1].alive)
						{
							neighbours++;
						}
					}
				}

				if (x > 0)
				{
					if (cells[x - 1, y].alive)
						neighbours++;
				}

				if (x < numberOfRows)
				{
					if (cells[x + 1, y].alive)
						neighbours++;
				}





				//Any live cell with fewer than two live neighbors dies as if caused by underpopulation.
				//Any live cell with two or three live neighbors lives on to the next generation.
				//Any live cell with more than three live neighbors dies, as if by overpopulation.
				//Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

				//Rule 1 & Rule 3
				if (neighbours == 3)
                    {
						cells[x, y].aliveNextGen = true;
                    }
					else if (neighbours == 2 && cells[x, y].alive)
					{
						cells[x, y].aliveNextGen = true;
					}
                    else
                    {
						cells[x, y].aliveNextGen = false;
					}
				}
			}

		//TODO: update buffer

		//Draw all cells.
		for (int y = 0; y < numberOfRows; ++y)
		{
			for (int x = 0; x < numberOfColums; ++x)
			{
				cells[x, y].alive = cells[x, y].aliveNextGen;
				cells[x, y].Draw();

			}
		}

	}
}

//You will probebly need to keep track of more things in this class
public class GameCell : ProcessingLite.GP21
{
	float x, y; //Keep track of our position
	float size; //our size

	//Keep track if we are alive
	public bool alive = false;
	public bool aliveNextGen = false;
	//Constructor
	public GameCell(float x, float y, float size)
	{
		//Our X is equal to incoming X, and so forth
		//adjust our draw position so we are centered
		this.x = x + size / 2;
		this.y = y + size / 2;

		//diameter/radius draw size fix
		this.size = size / 2;
	}

	public void Draw()
	{
		//If we are alive, draw our dot.
		if (alive)
		{
			//draw our dots
			Circle(x, y, size);
		}
	}
}