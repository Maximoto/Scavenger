using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public int score;
    public string animal;
    public Button button;
    public Text text;

    public void Initialize(GameObject go)
    {
        text = go.GetComponentInChildren<Text>();
        button = go.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            EventManager.Fire(new GameEvent(go, animal));
        });
        button.name = animal;
    }
}
