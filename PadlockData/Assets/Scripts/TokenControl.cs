using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TokenControl : MonoBehaviour {

    public GameObject white;
    public GameObject green;

    Animator whiteAnim;
    Animator greenAnim;

    BoardControl bC;
    GameControl gC;
    SoundHandler sH;

    //List of Components for all the squares (to reduce lag). Filled in BoardControl
    public List<BoardSquare> squareList;

    public Vector2 whitePos;
    public Vector2 greenPos;

    bool wOOB = false;
    bool gOOB = false;

    bool wInput = false;

    bool bInput = false;

    WaitForSeconds bufferTime;

    void Start() {
        sH = GetComponent<SoundHandler>();
        bC = GameObject.Find("Board").GetComponent<BoardControl>();
        gC = GetComponent<GameControl>();
        whitePos = bC.wStart;
        greenPos = bC.gStart;
        bufferTime = new WaitForSeconds(0.125f);

        whiteAnim = white.GetComponent<Animator>();
        greenAnim = green.GetComponent<Animator>();
        //SetColor(true);
        //SetColor(false);
    }

    void FixedUpdate() {
        white.transform.position = whitePos * bC.step;
        green.transform.position = greenPos * bC.step;

        white.transform.position = new Vector3(white.transform.position.x, white.transform.position.y, -1);
        green.transform.position = new Vector3(green.transform.position.x, green.transform.position.y, -1);

        float wHor = 0;
        float wVer = 0;
        float bHor = 0;
        float bVer = 0;

        //Input axes
        if (gC.playable)
        {
            wHor = Input.GetAxis("White Horizontal");
            wVer = Input.GetAxis("White Vertical");
            bHor = Input.GetAxis("Green Horizontal");
            bVer = Input.GetAxis("Green Vertical");
        }


        //White Movement
        Vector2 newWHPos = new Vector2(whitePos.x + wHor, whitePos.y);
        Vector2 newWVPos = new Vector2(whitePos.x, whitePos.y + wVer);
        if ((wHor == 1 || wHor == -1) && !wInput && whitePos.x + wHor > -1 && whitePos.x + wHor < bC.dimensions.x && greenPos != newWHPos)
        {
            whitePos = newWHPos;
            wInput = true;
            StartCoroutine(Buffer(1));
            SetColor(1);
            sH.PlaySound("White Move");
        } else if ((wVer == 1 || wVer == -1) && !wInput && whitePos.y + wVer > -1 && whitePos.y + wVer < bC.dimensions.y && greenPos != newWVPos)
        {
            whitePos = newWVPos;
            wInput = true;
            StartCoroutine(Buffer(1));
            SetColor(1);
            sH.PlaySound("White Move");
        }

        //green Movement
        Vector2 newGHPos = new Vector2(greenPos.x + bHor, greenPos.y);
        Vector2 newGVPos = new Vector2(greenPos.x, greenPos.y + bVer);
        if ((bHor == 1 || bHor == -1) && !bInput && greenPos.x + bHor > -1 && greenPos.x + bHor < bC.dimensions.x && whitePos != newGHPos)
        {
            greenPos = newGHPos;
            bInput = true;
            StartCoroutine(Buffer(2));
            SetColor(2);
            sH.PlaySound("Green Move");
        }
        else if ((bVer == 1 || bVer == -1) && !bInput && greenPos.y + bVer > -1 && greenPos.y + bVer < bC.dimensions.y && whitePos != newGVPos)
        {
            greenPos = newGVPos;
            bInput = true;
            StartCoroutine(Buffer(2));
            SetColor(2);
            sH.PlaySound("Green Move");
        }

        if (OOB() && gC.playable)
        {
            gC.playable = false;
            if (wOOB)
            {
                gC.gScore++;
                StartKillAnim(1);
            } else
            {
                gC.wScore++;
                StartKillAnim(2);
            }
        }

    }

    void Setup ()
    {
        whitePos = bC.wStart;
        greenPos = bC.gStart;
        bC.Setup();
    }

    public BoardSquare PullSquareComponent (Vector2 squarePos)
    {
        BoardSquare result = squareList[0];

        foreach (BoardSquare tSquare in squareList)
        {
            if (tSquare.pos == squarePos)
            {
                result = tSquare;
            }
        }

        return result;
    }

    void SetColor (int color)
    {
        Vector2 tPos;

        if (color == 1)
        {
            tPos = whitePos;
        } else
        {
            tPos = greenPos;
        }
        BoardSquare tBS = PullSquareComponent(tPos);
        if (tBS.type == 0)
        {
            tBS.color = color;
            tBS.captured = false;
            bC.FindAndFloodFill(color);
        }
    }

    public void DisplaceToken(int color)
    {
        Vector2 sPos;
        if (color == 1)
        {
            sPos = whitePos;
        } else
        {
            sPos = greenPos;
        }
        int shortest = 99999;
        int dir = 0;
        Vector2 change = new Vector2 (0,0);
        for(int i = 0; i < 4; i++)
        {
            bool end = false;
            for (int j = 1; !end; j++)
            {
                Vector2 cPos = new Vector2(0, 0);
                switch (i)
                {
                    case 0:
                        cPos = new Vector2(j, 0);
                        break;
                    case 1:
                        cPos = new Vector2(0, j);
                        break;
                    case 2:
                        cPos = new Vector2(-j, 0);
                        break;
                    case 3:
                        cPos = new Vector2(0, -j);
                        break;
                }
                Vector2 tPos = sPos + cPos;
                int sColor = 0;
                if (!((tPos.x < 0 || tPos.x > bC.dimensions.x - 1) || (tPos.y < 0 || tPos.y > bC.dimensions.y - 1)))
                {
                    sColor = PullSquareComponent(tPos).color;
                }
                if (color == sColor || sColor == 0)
                {
                    end = true;
                    if (j < shortest || (j == shortest && i > dir))
                    {
                        shortest = j;
                        dir = i;
                        change = tPos;
                    }
                }
            }
        }

        sPos = change;
        if (gC.playable)
        {
            switch (color)
            {
                case 1:
                    whitePos = sPos;
                    PlayWhiteAnimation("WhiteDisplaced");
                    sH.PlaySound("White Displaced");
                    break;
                case 2:
                    greenPos = sPos;
                    PlayGreenAnimation("GreenDisplaced");
                    sH.PlaySound("Green Displaced");
                    break;
            }
        }
        if (!OOB())
        {
            SetColor(color);
        }

    }

    bool OOB ()
    {
        wOOB = whitePos.x < 0 || whitePos.x > bC.dimensions.x - 1 || whitePos.y < 0 || whitePos.y > bC.dimensions.y - 1;
        gOOB = greenPos.x < 0 || greenPos.x > bC.dimensions.x - 1 || greenPos.y < 0 || greenPos.y > bC.dimensions.y - 1;
        return wOOB || gOOB;
    }

    IEnumerator Buffer (int color)
    {
        yield return bufferTime;
        if (color == 1)
        {
            wInput = false;
        } else
        {
            bInput = false;
        }
    }

    void StartKillAnim(int color)
    {
        gC.playable = false;
        sH.PlaySound("Token Out");
        switch(color)
        {
            case 1:
                whiteAnim.Play("WhiteOut", 0);
                break;
            case 2:
                greenAnim.Play("GreenOut", 0);
                break;
        }
    }

    public void EndKillAnim (int color)
    {
        switch(color)
        {
            case 1:
                whiteAnim.Play("WhiteDefault");
                break;
            case 2:
                greenAnim.Play("GreenDefault");
                break;
        }
        Setup();
    }

    void PlayWhiteAnimation(string animName)
    {
        whiteAnim.Play(animName, 0, 0.0f);
        whiteAnim.Play("WhiteDefault");
    }

    void PlayGreenAnimation(string animName)
    {
        greenAnim.Play(animName, 0, 0.0f);
        greenAnim.Play("GreenDefault");
    }

}