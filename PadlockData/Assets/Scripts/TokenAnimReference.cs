using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenAnimReference : MonoBehaviour {

    TokenControl tC;

	void Start () {
        tC = GameObject.Find("Board").GetComponent<TokenControl>();
	}
	
	void Update () {
		
	}

    public void EndKillAnim(int color)
    {
        tC.EndKillAnim(color);
    }

}
