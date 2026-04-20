using UnityEngine;
using System.Collections.Generic;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyHealthAlarm : EnemyHealth, IHealth
{
    [SerializeField] TextMeshPro text;
    const float ERROR_MESSAGE_DURATION = 0.5f;

    void Update()
    {
        if (Time.time > lastAnyDamagTime + ERROR_MESSAGE_DURATION)
            text.text = System.DateTime.Now.ToString("hh:mm");
        else
            text.text = "ERR0R";
    }
}
