using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPC : MonoBehaviour
{

    [SerializeField] private NPCConversation myConvo;
    private bool talked;

    // Start is called before the first frame update
    void Start()
    {
        talked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!talked && myConvo != null)
        {
            ConversationManager.Instance.StartConversation(myConvo);
            talked = true;
        }
    }
}
