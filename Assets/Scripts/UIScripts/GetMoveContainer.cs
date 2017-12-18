using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMoveContainer : MonoBehaviour {
    Move move;
    GameManager manager;
	// Use this for initialization
	void Start () {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        move = this.GetComponent<Container>().move;
	}
	

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && move != null)
        {
            manager.SwapPieces(move);
        }
    }
}
