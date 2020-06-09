using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

    BoardControl bC;
    GameControl gC;

    public GameObject whiteScoreBlock;
    public GameObject greenScoreBlock;
    public GameObject countdownBlock;
    public GameObject roundCountBlock;

    Text whiteScore;
    Text greenScore;
    Text countdownText;
    Text roundCountText;

    int wScore;
    int gScore;
    int countdown;
    int roundCount;

	void Start () {
        bC = GameObject.Find("Board").GetComponent<BoardControl>();
        gC = GameObject.Find("Board").GetComponent<GameControl>();
        Vector2 boardCenter = new Vector2((bC.dimensions.x - 1) / 2, (bC.dimensions.y - 1) / 2) * bC.step;
        transform.position = new Vector3(boardCenter.x, boardCenter.y, transform.position.z);

        whiteScore = whiteScoreBlock.GetComponent<Text>();
        greenScore = greenScoreBlock.GetComponent<Text>();
        countdownText = countdownBlock.GetComponent<Text>();
        roundCountText = roundCountBlock.GetComponent<Text>();
    }
	
	void Update () {
        wScore = gC.wScore;
        gScore = gC.gScore;
        countdown = gC.countDown;
        roundCount = gC.roundCount;
        whiteScore.text = wScore.ToString();
        greenScore.text = gScore.ToString();
        if (countdown > 0)
        {
            countdownText.text = countdown.ToString();
        } else
        {
            countdownText.text = "";
        }
        roundCountText.text = "ROUND " + roundCount.ToString();
        if (roundCount > 254)
        {
            gC.roundCount = 255;
            roundCountText.text = "ROUND INFINITY";
        }
	}
}
