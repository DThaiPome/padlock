using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour {

    AudioSource[] aCs;

    AudioSource greenDisplaced;
    AudioSource whiteDisplaced;
    AudioSource whiteMove;
    AudioSource greenMove;
    AudioSource whiteCapture;
    AudioSource greenCapture;
    AudioSource gameStart;
    AudioSource tokenOut;
    AudioSource intro;

    Dictionary<string, AudioSource> audioDic;

	void Start () {
        aCs = transform.Find("Sounds").GetComponents<AudioSource>();

        greenDisplaced = aCs[0];
        whiteDisplaced = aCs[1];
        whiteMove = aCs[3];
        greenMove = aCs[2];
        whiteCapture = aCs[5];
        greenCapture = aCs[4];
        gameStart = aCs[6];
        tokenOut = aCs[7];
        intro = aCs[8];

        audioDic = new Dictionary<string, AudioSource>();
        audioDic.Add("Green Displaced", greenDisplaced);
        audioDic.Add("White Displaced", whiteDisplaced);
        audioDic.Add("White Move", whiteMove);
        audioDic.Add("Green Move", greenMove);
        audioDic.Add("White Capture", whiteCapture);
        audioDic.Add("Green Capture", greenCapture);
        audioDic.Add("Game Start", gameStart);
        audioDic.Add("Token Out", tokenOut);
        audioDic.Add("Intro", intro);
	}
	
	void Update () {
		
	}

    public void PlaySound(string soundName)
    {
        AudioSource tSound = audioDic[soundName];

        tSound.Play();
    }

}
