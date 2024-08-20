using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubbleTrigger : MonoBehaviour
{

    [SerializeField] private GameObject thoughtBubbleContainer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            thoughtBubbleContainer.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
