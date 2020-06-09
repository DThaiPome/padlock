using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour {

    public Vector2 pos;
    public int color;
    public int type;
    public bool captured = false;

    Animator anim;

    /* COLORS
     * 0-Grey
     * 1-White
     * 2-Green
     * */

    /* TYPES
     * 0 - Normal
     * 1 - Side
     * 2 - Top
     * 3 - Bottom
     * 4 - Top Corner
     * 5 - Bottom Corner
     * */

	void Start () {
        anim = gameObject.GetComponent<Animator>();
	}
	
	void Update () {
        if (!captured)
        {
            anim.SetInteger("color", color);
            anim.SetInteger("type", type);
        } else
        {
            anim.SetInteger("color", 100);
            anim.SetInteger("type", 100);
        }
	}

    public void CapturedAnim ()
    {
        captured = true;
        anim.Play("Captured");
    }

    public void StopCapture ()
    {
        captured = false;
    }

}