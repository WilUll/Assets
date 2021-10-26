using UnityEngine;

public class GameOfLife : ProcessingLite.GP21
{
    GameCell[,] cells; //Our game grid matrix
    float cellSize = 0.25f; //Size of our cells
    int numberOfColums;
    int numberOfRows;
    bool isPaused = true;

    void Start()
    {
        //Lower framerate makes it easier to test and see whats happening.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
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
            }
        }
    }

    void Update()
    {
        //Clear screen
        Background(0);

        //Controls
        //Speed up or slows down on arrow keys
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) && Application.targetFrameRate < 120)
        {
            Application.targetFrameRate *= 2;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) && Application.targetFrameRate >1)
        {
            Application.targetFrameRate /= 2;
        }

        if (Input.GetAxis("Mouse ScrollWheel")< 0 && Camera.main.orthographicSize < 5)
        {
            Camera.main.orthographicSize++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main.orthographicSize > 1)
        {
            Camera.main.orthographicSize--;
        }


        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            isPaused = !isPaused;
            if (isPaused == true)
            {

                Application.targetFrameRate = 60;
            }
            else
            {
                Application.targetFrameRate = 8;
            }
        }


        if (isPaused == false)
        {

            for (int y = 0; y < numberOfRows; ++y)
            {
                for (int x = 0; x < numberOfColums; ++x)
                {
                    int neighbours = 0;

                    //Goes through the y axis from top to bottom
                    //Goes through the x axis from left to right

                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (x + j > 0 && x + j < numberOfColums && y + i > 0 && y + i < numberOfRows)
                            {
                                if (i == 0 && j == 0)
                                {

                                }
                                else if (cells[x + j, y + i].alive)
                                {
                                    neighbours++;
                                }
                            }
                        }

                    }



                    //RULES
                    //Any live cell with fewer than two live neighbors dies as if caused by underpopulation.
                    //Any live cell with two or three live neighbors lives on to the next generation.
                    //Any live cell with more than three live neighbors dies, as if by overpopulation.
                    //Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
                    //Checks the rules
                    if (neighbours == 3)
                    {
                        cells[x, y].aliveNextGen = true;
                        cells[x, y].ballAge++;
                    }
                    else if (neighbours == 2 && cells[x, y].alive)
                    {
                        cells[x, y].aliveNextGen = true;
                        cells[x, y].ballAge++;
                    }
                    else
                    {
                        cells[x, y].aliveNextGen = false;
                        cells[x, y].ballAge = 0;
                    }
                    Mathf.Clamp(cells[x, y].ballAge, 0, 15);
                    
                }
            }
        }


        //Paint/remove cells with mouse buttons
        if (Input.GetMouseButton(0) && MouseX > 0 && MouseX < Width && MouseY > 0 && MouseY < Height)
        {
            cells[(int)(MouseX / cellSize), (int)(MouseY / cellSize)].aliveNextGen = true;
        }
        else if (Input.GetMouseButton(1) && MouseX > 0 && MouseX < Width && MouseY > 0 && MouseY < Height)
        {
            cells[(int)(MouseX / cellSize), (int)(MouseY / cellSize)].aliveNextGen = false;
        }

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
    public int ballAge = 0;

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
            if (ballAge > 0)
            {
                if (ballAge >= 1 && ballAge <= 5)
                {
                    Stroke(255, 255, 255);
                }
                else if (ballAge > 5 && ballAge <= 10)
                {
                    Stroke(255, 255, 125);
                }
                else if (ballAge > 10 && ballAge <= 15)
                {
                    Stroke(255, 125, 125);
                }
                else if (ballAge > 10 && ballAge <= 15)
                {
                    Stroke(255, 0, 125);
                }
                else if (ballAge > 15)
                {
                    Stroke(255, 0, 0);
                }
            }

            //draw our dots
            Circle(x, y, size);
        }
        if (!alive)
        {
            Stroke(255, 255, 255);
        }
    }
}