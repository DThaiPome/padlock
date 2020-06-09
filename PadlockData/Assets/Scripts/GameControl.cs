using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public int wScore;
    public int gScore;

    public int winScore;

    public int roundCount = 0;

    BoardControl bC;

    SoundHandler sH;

    public int countDown;

    public bool playable = false;
    public bool firstGame = true;

    WaitForSeconds oneSecond;

	void Start () {
        bC = GetComponent<BoardControl>();
        sH = GetComponent<SoundHandler>();
        oneSecond = new WaitForSeconds(0.75f);

        StartCoroutine(StartGame());
	}
	
	void Update () {
		
	}

    public IEnumerator StartGame()
    {
        playable = false;
        roundCount++;
        if ((wScore - 1 > gScore && wScore >= winScore) || (gScore - 1 > wScore && gScore >= winScore))
        {
            firstGame = true;
        }
        if (firstGame)
        {
            roundCount = 1;
            wScore = 0;
            gScore = 0;
            sH.PlaySound("Intro");
            yield return new WaitForSeconds(3.5f);
            firstGame = false;
        }
        countDown = 3;
        for (int i = 0; i < 3; i++)
        {
            sH.PlaySound("White Capture");
            yield return oneSecond;
            countDown -= 1;
        }
        sH.PlaySound("Game Start");
        playable = true;
    }

}
