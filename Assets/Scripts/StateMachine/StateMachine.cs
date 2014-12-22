using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State
{
    public StateMachine stateMachine;
    public Dictionary<string, GameObject> SceneObjects;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    
    public void GetStateRelevantObjectsFromScene()
    {
        GameObject[] gos =  GameObject.FindGameObjectsWithTag("StateRelevant");
        SceneObjects = new Dictionary<string, GameObject>();
        foreach(GameObject go in gos)
        {
            SceneObjects.Add(go.name, go);
        }
    }

    virtual public string RequireLevelLoad() { return ""; }
    virtual public void Enter(State prevState = null) { }
    virtual public void Update() { }
    virtual public void Exit() { }
    virtual public void OnGameEvent(GameEvent e) { }
}

public class StateMachine : MonoBehaviour
{
    State currentState = null;
    State nextState = null;
    bool requireLevelLoad = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
        currentState = new EnterState(this);
        EventManager.AddListener<GameEvent>(OnGameEvent);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener<GameEvent>(OnGameEvent);
    }

    void OnGameEvent(GameEvent e)
    {
        currentState.OnGameEvent(e);
    }

    public void SetState(State nextState)
    {
        this.nextState = nextState;
        string level = nextState.RequireLevelLoad();
        if(!string.IsNullOrEmpty(level))
        {
            requireLevelLoad = true;
            Application.LoadLevel(level);
        }
    }

    void NextState()
    {
        currentState.Exit();
        nextState.Enter(currentState);
        currentState = nextState;
        nextState = null;
    }

    void OnLevelWasLoaded(int level)
    {
        NextState();
        requireLevelLoad = false;
    }

    void Update()
    {
        // if this flag is set, we are waiting for the next level to load, no need to update
        if (requireLevelLoad)
        {
            return;
        }

        if (nextState != null)
        {
            NextState();
        }
        else
        {
            currentState.Update();
        }
    }
}
