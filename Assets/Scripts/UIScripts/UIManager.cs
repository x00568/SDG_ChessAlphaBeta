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
    public GameManager gamemanger;
    public GameObject winPanel;

    void Update()
    {



        if (gamemanger.BlackWin())
            {
                winPanel.SetActive(true);
                winText.text = " 黑方" + "WINNER!";
                Time.timeScale = 0;
            }
            if (gamemanger.WhiteWin())
            {
                winPanel.SetActive(true);
                winText.text = " 白方" + "WINNER!";
                Time.timeScale = 0;
            }
      
        if (gamemanger.WhoPlay())
        {
            turnText.text = "白方走";
        }
        else
        {
            if (gamemanger.timer < 0.5)
                turnText.text = "黑方走";
            else if (gamemanger.timer >= 0.5 && gamemanger.timer < 3)
                turnText.text = "黑方正在思考";
            else if (gamemanger.timer >= 3)
            {
                turnText.text = "黑方正在移动";
            }
        }
            if (gamemanger.BlackAttack())
            {
                BKingText.text = "King Check";
            }
             else
                BKingText.text = " ";

            if (gamemanger.WhiteAttack())
                WKingText.text = "King Check";
            else
                WKingText.text = " ";
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
