using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnterState : State 
{
    public EnterState(StateMachine s) : base(s)
    {
    }

    public override void Enter(State prevState)
    {
        UnityEngine.Random.seed = (int)Time.time;
    }
    
    public override void OnGameEvent(GameEvent e)
    {
        if(e.data == "NewGame")
        {
            stateMachine.SetState(new InitState(stateMachine));
        }
    }
}
