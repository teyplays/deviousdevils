using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class SpeechBubbleManager : MonoBehaviour
{

    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Speech Bubbles")]
    [SerializeField] private GameObject thoughtBubble;
    [SerializeField] private GameObject voiceBubble;

    [Header("Dialogue Sentences")]
    [TextArea]
    [SerializeField] private string[] dialogueSentences;

    [Header("Da Voice (true) or Thought (False) (MAKE THIS LENGTH THE SAME LENGTH!!!)")]
    [SerializeField] private bool[] daVoice;

    private float animationDelay = 0.6f;

    private int index;
    private bool sentenceDone;
    private bool opened;
    private TextMeshProUGUI dialogueText;
    private Animator thoughtBubbleAnimator;
    private TextMeshProUGUI voiceText;
    private Animator voiceBubbleAnimator;
    private bool voiceTalking;

    public IEnumerator StartDialogue()
    {
        if (index < dialogueSentences.Length) // Ensure sentences remain
        {
            if (daVoice[index]) // Check if voice or thoughts are talking
            {
                voiceTalking = true;
            }
            else
            {
                voiceTalking = false;
            }
            if (thoughtBubble.gameObject.transform.localScale.x == 0 && !voiceTalking) // Open thought bubble if thoughts about to talk
            {
                if (voiceBubble.gameObject.transform.localScale.x != 0) // Close voice bubble if open
                {
                    voiceText.text = string.Empty;
                    voiceBubbleAnimator.SetTrigger("Close");
                    opened = false;
                    yield return new WaitForSeconds(animationDelay);
                }
                thoughtBubbleAnimator.SetTrigger("Open");
                yield return new WaitForSeconds(animationDelay);
                opened = true;
            }
            else if (voiceBubble.gameObject.transform.localScale.x == 0 && voiceTalking) // Open voice bubble if voice about to talk
            {
                if (thoughtBubble.gameObject.transform.localScale.x != 0) // Close thought bubble if open
                {
                    dialogueText.text = string.Empty;
                    thoughtBubbleAnimator.SetTrigger("Close");
                    opened = false;
                    yield return new WaitForSeconds(animationDelay);
                }
                voiceBubbleAnimator.SetTrigger("Open");
                yield return new WaitForSeconds(animationDelay);
                opened = true;
            }
            sentenceDone = false;
            StartCoroutine("TypeDialogue");
        }
        else // close all bubbles if no more dialogue
        {
            opened = false;
            if (voiceTalking)
            {
                voiceText.text = string.Empty;
                voiceBubbleAnimator.SetTrigger("Close");
            }
            else
            {
                dialogueText.text = string.Empty;
                thoughtBubbleAnimator.SetTrigger("Close");
            }
            yield return new WaitForSeconds(animationDelay);
        }
    }

    private IEnumerator TypeDialogue()
    {
        foreach (char letter in dialogueSentences[index].ToCharArray())
        {
            if (voiceTalking) // Ensure text is typed to correct bubble
            {
                voiceText.text += letter;
            }
            else
            {
                dialogueText.text += letter;
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        sentenceDone = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        opened = false;
        sentenceDone = true;
        voiceTalking = false;
        thoughtBubbleAnimator = thoughtBubble.GetComponent<Animator>();
        dialogueText = thoughtBubble.GetComponentInChildren<TextMeshProUGUI>();
        voiceBubbleAnimator = voiceBubble.GetComponent<Animator>();
        voiceText = voiceBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (daVoice.Length != dialogueSentences.Length) // Ensure arrays are the same size (Elements must line up)
        {
            Debug.Log("Warning: The voice boolean array and dialogue sentences array are diffrent lengths, ensure these are the same for dialogue bubbles to function properly.");
        }
        StartCoroutine("StartDialogue");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && opened) // Check for input
        {
            if (sentenceDone) // Get next sentence
            {
                if (voiceTalking)
                {
                    voiceText.text = string.Empty;
                }
                else
                {
                    dialogueText.text = string.Empty;
                }
                index++;
                StartCoroutine("StartDialogue");
            }
            else // Skip dialogue scroll
            {
                StopCoroutine("TypeDialogue");
                if (voiceTalking)
                {
                    voiceText.text = dialogueSentences[index];
                }
                else
                {
                    dialogueText.text = dialogueSentences[index];
                }
                sentenceDone = true;
            }
        }
    }
}
