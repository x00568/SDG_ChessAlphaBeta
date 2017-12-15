using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour
{
    public Move _move;
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) &&_move != null)
        {
            GameControl.Instance.SwapChess(_move);
        }
    }
}
