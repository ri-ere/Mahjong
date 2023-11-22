using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointDisplay : MonoBehaviour
{
    public static void NagashiMangan()
    {
        TextMeshProUGUI point = GameObject.Find("Point").GetComponent<TextMeshProUGUI>();
        point.text = "유국만관\n12000점";
    }
    public static void Yuukyoku()
    {
        TextMeshProUGUI point = GameObject.Find("Point").GetComponent<TextMeshProUGUI>();
        point.text = "유국";
    }
    public static void DisplayPoint(int point, int HanBusu)
    {
        TextMeshProUGUI pointObject = GameObject.Find("Point").GetComponent<TextMeshProUGUI>();
        int han = HanBusu / 1000;
        int busu = HanBusu % 1000;
        if(point >= 12000) pointObject.text = han + "판\n" + point + "점";
        else
        {
            pointObject.text = han + "판, " + busu + "부\n" + point + "점";
        }
    }
}