using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleOverWindow1 : MonoBehaviour
{
    // Start is called before the first frame update
    private static BattleOverWindow1 instance;
    private void Awake()
    {
        instance = this;
        Hide();
    }
    // Update is called once per frame
    private void Hide()
    {
        gameObject.SetActive(false);    
    }
    private void show(string winnerString)
    {
        gameObject.SetActive(true);
        transform.Find("winnerText (1)").GetComponent<Text>().text = winnerString;
    }
    public static void Show_Static(string winnerString)
    {
        instance.show(winnerString);
    }
}
