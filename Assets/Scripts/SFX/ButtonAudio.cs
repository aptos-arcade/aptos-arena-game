using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
    
    [SerializeField] private AudioClip hoverAudioClip;
    [SerializeField] private AudioClip clickAudioClip;
    
    private AudioSource audioSource;
    private EventTrigger eventTrigger;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        eventTrigger = GetComponent<EventTrigger>();
        
        var hoverEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        hoverEntry.callback.AddListener((data) => { PlayHoverSound(); });
        eventTrigger.triggers.Add(hoverEntry);
        
        var clickEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        clickEntry.callback.AddListener((data) => { PlayClickSound(); });
        eventTrigger.triggers.Add(clickEntry);
    }

    private void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverAudioClip);
    }

    private void PlayClickSound()
    {
        audioSource.PlayOneShot(clickAudioClip);
    }
}
