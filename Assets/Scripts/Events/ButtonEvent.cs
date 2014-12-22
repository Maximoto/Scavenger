using UnityEngine;
using System.Collections;

public class ButtonEvent : MonoBehaviour
{
    public void ButtonPressed(string data)
    {
        EventManager.Fire(new GameEvent(gameObject, data));
    }
}
