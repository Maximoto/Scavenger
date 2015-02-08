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
    List<string> remainingReturns;
    List<string> remainingThings;

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

        string returnPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Data/Return");
        string thingPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Data/Thing");

        returns = System.IO.File.ReadAllText(returnPath+".txt").Split('\n');
        things = System.IO.File.ReadAllText(thingPath+".txt").Split('\n');
        remainingReturns = new List<string>(returns);
        remainingThings = new List<string>(things);

        GetStateRelevantObjectsFromScene();
        thingText = SceneObjects["Thing"].GetComponentInChildren<Text>() as Text;
        returnText = SceneObjects["Return"].GetComponentInChildren<Text>() as Text;
        roundText = SceneObjects["Round"].GetComponentInChildren<Text>() as Text;

        ready = SceneObjects["Ready"];

        roundText.text = (prevState as InitState).numRounds.ToString();

        int y = 0;
        foreach (Player player in players.Values)
        {
            player.EnableButton(SceneObjects[player.animal]);
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
        int remove = Random.Range(0, remainingThings.Count);
        thingText.text = remainingThings[remove];
        remainingThings.RemoveAt(remove);
        if(remainingThings.Count == 0)
        {
            remainingThings = new List<string>(things);
        }

        yield return new WaitForSeconds(2.0f);

        updateReturn = true;
        yield return new WaitForSeconds(2.0f);
        updateReturn = false;
        remove = Random.Range(0, remainingReturns.Count);
        returnText.text = remainingReturns[remove];
        remainingReturns.RemoveAt(remove);
        if(remainingReturns.Count == 0)
        {
            remainingReturns = new List<string>(returns);
        }
        
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
            thingText.text = remainingThings[Random.Range(0, remainingThings.Count)];
        }
        if(updateReturn)
        {
            returnText.text = remainingReturns[Random.Range(0, remainingReturns.Count)];
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
