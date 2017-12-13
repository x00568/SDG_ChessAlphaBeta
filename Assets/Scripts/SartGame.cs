using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SartGame : MonoBehaviour
{
    public Dropdown drop;
    public static int depthIndex = 2;
    void Awake()
    {
        drop.onValueChanged.AddListener(ValueChange);
    }
    public void Load()
    {
        SceneManager.LoadScene(1);
    }
    public void ValueChange(int index)
    {
        depthIndex = index + 2;
    }
}
