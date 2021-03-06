﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEvent 
{ 
    public GameObject owner;
    public string data;
    public GameEvent(GameObject owner, string data)
    {
        this.owner = owner;
        this.data = data;
    }
}

public class EventManager
{
    public delegate void EventDelegate<T>(T e) where T : GameEvent;

    static Dictionary<System.Type, System.Delegate> delegates = new Dictionary<System.Type, System.Delegate>();

    public static void AddListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        if (delegates.ContainsKey(typeof(T)))
        {
            System.Delegate tempDel = delegates[typeof(T)];

            delegates[typeof(T)] = System.Delegate.Combine(tempDel, del);
        }
        else
        {
            delegates[typeof(T)] = del;
        }
    }

    public static void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
    {
        if (delegates.ContainsKey(typeof(T)))
        {
            var currentDel = System.Delegate.Remove(delegates[typeof(T)], del);

            if (currentDel == null)
            {
                delegates.Remove(typeof(T));
            }
            else
            {
                delegates[typeof(T)] = currentDel;
            }
        }
    }

    public static void Fire(GameEvent e)
    {
        if (e == null)
        {
            Debug.Log("Invalid event argument: " + e.GetType().ToString());
            return;
        }

        if (delegates.ContainsKey(e.GetType()))
        {
            delegates[e.GetType()].DynamicInvoke(e);
        }
    }
}