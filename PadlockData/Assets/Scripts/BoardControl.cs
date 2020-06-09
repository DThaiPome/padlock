using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControl : MonoBehaviour {

    public List<GameObject> squares;

    public GameObject square;

    public Vector2 dimensions;
    public float step;

    TokenControl tC;

    GameControl gC;

    SoundHandler sH;

    bool moved;

    public Vector2 wStart;
    public Vector2 gStart;

    void Start() {
        sH = GetComponent<SoundHandler>();
        tC = GetComponent<TokenControl>();
        gC = GetComponent<GameControl>();
        moved = false;
        //Spawn all squares and give all squares positions
        squares = new List<GameObject>();
        for (int i = -1; i < dimensions.y + 1; i++)
        {
            for (int j = -1; j < dimensions.x + 1; j++)
            {
                GameObject s = (GameObject)Instantiate(square);
                BoardSquare sBs = s.GetComponent<BoardSquare>();
                sBs.pos = new Vector2(j, i);
                s.transform.position = new Vector3(j * step, i * step, 0);
                sBs.color = 0;
                sBs.type = 0;

                //Set initial squares
                if (i == wStart.y && j == wStart.x)
                {
                    sBs.color = 1;
                }
                if (i == gStart.y && j == gStart.x)
                {
                    sBs.color = 2;
                }

                //Check for edge pieces

                //Bottom row
                if (i == -1)
                {
                    if (j == -1)
                    {
                        sBs.color = 1;
                        sBs.type = 5;
                    } else if (j < dimensions.x / 2)
                    {
                        sBs.color = 1;
                        sBs.type = 3;
                    } else if (j == dimensions.x)
                    {
                        sBs.color = 2;
                        sBs.type = 5;
                    } else
                    {
                        sBs.color = 2;
                        sBs.type = 3;
                    }
                    //Top row
                } else if (i == dimensions.y)
                {
                    if (j == -1)
                    {
                        sBs.color = 1;
                        sBs.type = 4;
                    } else if (j < dimensions.x / 2)
                    {
                        sBs.color = 1;
                        sBs.type = 2;
                    } else if (j == dimensions.x)
                    {
                        sBs.color = 2;
                        sBs.type = 4;
                    } else if (j >= dimensions.x / 2)
                    {
                        sBs.color = 2;
                        sBs.type = 2;
                    }

                    //Left side
                } else if (j == -1)
                {
                    sBs.color = 1;
                    sBs.type = 1;

                    //Right side
                } else if (j == dimensions.x)
                {
                    sBs.color = 2;
                    sBs.type = 1;
                }

                squares.Add(s);
                tC.squareList.Add(sBs);
            }
        }
    }

    public void Setup()
    {
        foreach (GameObject s in squares)
        {
            BoardSquare tBS = s.GetComponent<BoardSquare>();
            if (tBS.type == 0)
            {
                tBS.color = 0;
                tBS.captured = false;
                if (tBS.pos == wStart)
                {
                    tBS.color = 1;
                }
                if (tBS.pos == gStart)
                {
                    tBS.color = 2;
                }
            }
        }
        StartCoroutine(gC.StartGame());
    }

    public GameObject GetSquare(Vector2 pos)
    {
        GameObject result = squares[0];

        foreach (GameObject s in squares)
        {
            if (s.gameObject.GetComponent<BoardSquare>().pos == pos)
            {
                result = s;
            }
        }

        return result;
    }

    public void FindAndFloodFill(int color)
    {
        List<BoardSquare> checkedSquares = new List<BoardSquare>();
        foreach (BoardSquare square in tC.squareList)
        {
            //Skips the whole check if the color is the same or if the square is a border tile

            bool valid = square.color != color && square.type == 0 && !checkedSquares.Contains(square);

            if (valid)
            {
                List<BoardSquare> fillQueue = new List<BoardSquare>();
                Queue<BoardSquare> checkQueue = new Queue<BoardSquare>();

                //The first square
                BoardSquare startSquare = square;
                checkQueue.Enqueue(square);
                fillQueue.Add(square);

                //Double check list
                List<BoardSquare> doubles = new List<BoardSquare>();

                //Check everything until there is nothing left to check (and while the selection is valid)
                while (checkQueue.Count > 0)
                {
                    startSquare = checkQueue.Dequeue();
                    /*Check a square in each cardinal direction;
                     * 0 - East
                     * 1 - North
                     * 2 - West
                     * 3 - South
                     * */
                    for (int i = 0; i < 4; i++)
                    {
                        //Movement per direction
                        int horMov = 0;
                        int verMov = 0;
                        switch(i)
                        {
                            case 0:
                                horMov = 1;
                                break;
                            case 1:
                                verMov = 1;
                                break;
                            case 2:
                                horMov = -1;
                                break;
                            case 3:
                                verMov = -1;
                                break;
                        }

                        //Square to check
                        Vector2 checkSquarePos = new Vector2(startSquare.pos.x + horMov, startSquare.pos.y + verMov);
                        BoardSquare checkSquare = tC.PullSquareComponent(checkSquarePos);

                        //Check if out of bounds, and fail the check if it is
                        if (TileOOB(checkSquarePos))
                        {
                            valid = false;
                        } else
                        {
                            //Check for a boundary or an inner square
                            if (checkSquare.color != color)
                            {
                                //Queue to fill and to be checked (if it's not already in there)
                                if (!fillQueue.Contains(checkSquare))
                                {
                                    fillQueue.Add(checkSquare);
                                    checkQueue.Enqueue(checkSquare);
                                    checkedSquares.Add(checkSquare);
                                }
                            } else
                            {
                                bool doubleCheck = false;
                                //Check for corners
                                Vector2 cornerAPos = new Vector2(checkSquarePos.x + verMov, checkSquarePos.y + horMov);
                                Vector2 cornerBPos = new Vector2(checkSquarePos.x - verMov, checkSquarePos.y - horMov);
                                BoardSquare cornerA = tC.PullSquareComponent(cornerAPos);
                                BoardSquare cornerB = tC.PullSquareComponent(cornerBPos);

                                if (TileOOB(cornerAPos) || TileOOB(cornerBPos))
                                {
                                    valid = false;
                                } else
                                {
                                    //Case 1: Both Corners are Empty
                                    if (cornerA.color != color && cornerB.color != color)
                                    {
                                        //Check if both squares above the corners are empty
                                        Vector2 nitAPos = new Vector2(cornerAPos.x - horMov, cornerAPos.y - verMov);
                                        Vector2 nitBPos = new Vector2(cornerBPos.x - horMov, cornerBPos.y - verMov);
                                        BoardSquare nitA = tC.PullSquareComponent(nitAPos);
                                        BoardSquare nitB = tC.PullSquareComponent(nitBPos);
                                        if (nitA.color == color || nitB.color == color)
                                        {
                                            doubleCheck = true;
                                        }
                                    //Case 2: One corner is empty
                                    } else if (cornerA.color != color || cornerB.color != color)
                                    {
                                        //Check the square next to the starting square to check for a special case
                                        BoardSquare nitCorner = null;
                                        if (cornerA.color != color)
                                        {
                                            nitCorner = cornerA;
                                        } else
                                        {
                                            nitCorner = cornerB;
                                        }
                                        Vector2 nitCheckPos = new Vector2(nitCorner.pos.x - horMov, nitCorner.pos.y - verMov);
                                        BoardSquare nitCheck = tC.PullSquareComponent(nitCheckPos);
                                        doubleCheck = !(nitCheck.color != color);
                                    }
                                    //And if both corners are filled just do nothing
                                    if (doubleCheck && !doubles.Contains(checkSquare))
                                    {
                                        doubles.Add(checkSquare);
                                    }
                                }
                            }
                        }
                    }
                }

                //Do a double check; Find an exterior tile around each direction of each tile
                foreach(BoardSquare dS in doubles)
                {
                    int doubleCount = 0;
                    bool within = true;
                    BoardSquare checkedSquare;
                    for (int i = 0; within && i < 4; i++)
                    {
                        //Movement per direction
                        int horMov = 0;
                        int verMov = 0;
                        switch (i)
                        {
                            case 0:
                                horMov = 1;
                                break;
                            case 1:
                                verMov = 1;
                                break;
                            case 2:
                                horMov = -1;
                                break;
                            case 3:
                                verMov = -1;
                                break;
                        }
                        Vector2 newSquarePos = dS.pos;
                        //Move one tile in one direction
                        do
                        {
                            newSquarePos += new Vector2(horMov, verMov);
                            checkedSquare = tC.PullSquareComponent(newSquarePos);
                        } while (!TileOOB(newSquarePos) && fillQueue.Contains(checkedSquare));
                        if (TileOOB(newSquarePos))
                        {
                            within = false;
                        } else
                        {
                            if (checkedSquare.color == color)
                            {
                                doubleCount++;
                            } else
                            {
                                within = false;
                            }
                        }
                    }

                    if (doubleCount != 4)
                    {
                        valid = false;
                    }
                }

                //Fill all squares
                if (valid)
                {
                    int token = 0;

                    foreach (BoardSquare tS in fillQueue)
                    {
                        if (tS.type == 0)
                        {
                            tS.color = color;
                            tS.CapturedAnim();
                        }
                        //Check for token
                        if (tC.whitePos == tS.pos && color == 2)
                        {
                            token = 1;
                        } else if (tC.greenPos == tS.pos && color == 1)
                        {
                            token = 2;
                        }
                    }

                    if (token != 0)
                    {
                        tC.DisplaceToken(token);
                    } else
                    {
                        if (color == 1)
                        {
                            sH.PlaySound("White Capture");
                        } else
                        {
                            sH.PlaySound("Green Capture");
                        }
                    }
                    
                }
            }

        }
    }

    bool TileOOB (Vector2 tPos)
    {
        bool result = tPos.x < -1 || tPos.x > dimensions.x + 1 || tPos.y < -1 || tPos.y > dimensions.y + 1;
        return result;
    }

}