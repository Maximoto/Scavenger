using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EndState : State
{
    public override string RequireLevelLoad() { return "End"; }

    public int numRounds = 5;

    public EndState(StateMachine s)
        : base(s)
    {
    }

    public override void Enter(State prevState)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/Animals");
        Dictionary<string, Sprite> animals = new Dictionary<string,Sprite>();
        foreach(Sprite animal in sprites)
        {
            animals[animal.texture.name] = animal;
        }

        List<Player> players = new List<Player>();
        foreach(Player player in (prevState as RaceState).players.Values)
        {
            players.Add(player);
        }

        players.Sort(
            delegate(Player lhs, Player rhs)
            {
                return rhs.score.CompareTo(lhs.score);
            });

        GetStateRelevantObjectsFromScene();

        string[] positions = new string[] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eigth" };
        for(int index = 0; index < positions.Length; ++index)
        {
            if(index < players.Count)
            {
                SceneObjects[positions[index]].GetComponent<Image>().sprite = animals[players[index].animal.ToLower()];
            }
            else
            {
                SceneObjects[positions[index]].SetActive(false);
            }
        }
    }

    public override void OnGameEvent(GameEvent e)
    {
        if (e.data == "Exit")
        {
            stateMachine.SetState(new InitState(stateMachine));
        }
    }
}
