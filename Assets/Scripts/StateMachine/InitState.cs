using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InitState : State
{
    public override string RequireLevelLoad() { return "InitPlayers"; }

    public Dictionary<string, Player> players;
    public int numRounds = 5;

    Text rounds;
    Button readyButton;

    public InitState(StateMachine s) : base(s)
    {
    }

    public override void Enter(State prevState = null)
    {
        players = new Dictionary<string, Player>();
        GetStateRelevantObjectsFromScene();
        rounds = SceneObjects["Rounds"].GetComponentInChildren<Text>();
        rounds.text = "5";
        readyButton = SceneObjects["Start"].GetComponent<Button>();
    }

    public override void OnGameEvent(GameEvent e)
    {
        if(e.data == "START")
        {
            stateMachine.SetState(new RaceState(stateMachine));
        }
        else if(e.data.StartsWith("Rounds"))
        {
            numRounds = (int)e.owner.GetComponent<Slider>().value;
            rounds.text = numRounds.ToString();
        }
        else 
        {
            players[e.owner.name] = new Player();
            players[e.owner.name].animal = e.owner.name;
            e.owner.GetComponent<Button>().interactable = false;
            if(players.Count > 1)
            { 
                readyButton.interactable = true;
            }
        }
    }
}
