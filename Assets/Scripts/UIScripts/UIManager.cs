using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text turnText;
    public Text winText;

    public Text WKingText;
    public Text BKingText;
    public GameManager _gameManger;
    public GameObject _winPanel;

    void Update()
    {
        Win();
        TurnChess();
        KingCheck();
    }
    //重新开始游戏
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    //悔棋
    public void Undo()
    {
        GameControl.Instance.Undo();
    }
    //判断哪方赢
    public void Win()
    {
        if (GameControl.Instance.BlackWin())
        {
            winText.text = " 黑方" + "WINNER!";
            Time.timeScale = 0;
        }
        if (GameControl.Instance.WhiteWin())
        {
            winText.text = " 白方" + "WINNER!";
            Time.timeScale = 0;
        }

       

    }
    //检测黑白棋子哪方先走
    public void TurnChess()
    {
        if(GameControl.Instance.WhichPlay())
        {
            turnText.text = "白方走";
        }
        else
        {
            if (_gameManger.timer < 0.5)
                turnText.text = "黑方走";
            else if (_gameManger.timer >= 0.5 && _gameManger.timer < 3)
                turnText.text = "黑方正在思考";
            else if (_gameManger.timer >= 3)
            {
                turnText.text = "黑方正在移动";
            }
        }
    }
    //检测King是否被攻击
    public void KingCheck()
    {
        if (GameControl.Instance.BlackKingCheck())
        {
            BKingText.text = "King Check";
            return;
        }

        if(GameControl.Instance.WhiteKingCheck()){
            WKingText.text = "King Check";
            return;
        }

            BKingText.text = "";
            WKingText.text = "";
    }
}
