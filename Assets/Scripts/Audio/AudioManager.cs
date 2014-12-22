using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour 
{
    public AudioClip[] sounds;
	void Start () 
    {
        sounds = Resources.LoadAll<AudioClip>("Audio");
        EventManager.AddListener<GameEvent>(OnButtonEvent);
	}

    void OnButtonEvent(GameEvent e)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(sounds[1]);
    }
}
