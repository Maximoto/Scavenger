using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RaceState : State
{
    public override string RequireLevelLoad() { return "Race"; }

    public Dictionary<string, Player> players;
    public int numRounds;

    string[] returns;
    string[] things;

    Text thingText;
    Text returnText;
    Text roundText;

    GameObject ready;

    int points;

    bool updateThing = false;
    bool updateReturn = false;

    public RaceState(StateMachine s)
        : base(s)
    {
        players = new Dictionary<string,Player>();
    }

    public override void Enter(State prevState)
    {
        players = (prevState as InitState).players;
        points = players.Count;

        TextAsset returnAsset = Resources.Load("Data/Return") as TextAsset;
        TextAsset thingAssets = Resources.Load("Data/Thing") as TextAsset;

        returns = returnAsset.text.Split('\n');
        things = thingAssets.text.Split('\n');

        GetStateRelevantObjectsFromScene();
        thingText = SceneObjects["Thing"].GetComponentInChildren<Text>() as Text;
        returnText = SceneObjects["Return"].GetComponentInChildren<Text>() as Text;
        roundText = SceneObjects["Round"].GetComponentInChildren<Text>() as Text;

        ready = SceneObjects["Ready"];

        roundText.text = (prevState as InitState).numRounds.ToString();

        int y = 0;
        foreach (Player player in players.Values)
        {
            player.Initialize(SceneObjects[player.animal]);
            player.button.interactable = false;
            y += 50;
        }
    }

    IEnumerator Refresh()
    {
        ready.GetComponent<Button>().interactable = false;
        points = players.Count;

        updateThing = true;
        yield return new WaitForSeconds(2.0f);
        updateThing = false;
        yield return new WaitForSeconds(2.0f);
        updateReturn = true;
        yield return new WaitForSeconds(2.0f);
        updateReturn = false;
        
        foreach(Player p in players.Values)
        {
            if(p != null)
            {
                p.button.interactable = true;
            }
        }
        yield return new WaitForSeconds(1.0f);
        SceneObjects["Background"].GetComponent<Image>().color = Color.white;
    }

    public override void Update()
    {
        if(updateThing)
        {
            thingText.text = things[Random.Range(0, things.Length)];
        }
        if(updateReturn)
        {
            returnText.text = returns[Random.Range(0, returns.Length)];
        }
    }

    public override void OnGameEvent(GameEvent e)
    {
        if(e.data == "Ready")
        {
            if(roundText.text == "0")
            {
                stateMachine.SetState(new EndState(stateMachine));
            }
            else
            {
                stateMachine.StartCoroutine(Refresh());
            }
            return;
        }

        Player p = players[e.data];
        p.score += points;
        p.text.text = p.score.ToString();
        p.button.interactable = false;

        --points;
        if (points == 0)
        {
            ready.GetComponent<Button>().interactable = true;
            roundText.text = (int.Parse(roundText.text) - 1).ToString();
            thingText.text = "";
            returnText.text = "";
            if (roundText.text == "0")
            {
                ready.GetComponentInChildren<Text>().text = "Results";
            }
        }
    }
}
