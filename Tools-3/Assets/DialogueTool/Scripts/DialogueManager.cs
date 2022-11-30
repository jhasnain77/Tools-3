using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] DialogueContainer dialogueContainer;   // This is only for debugging. Will include a method that
                                                            // declares this when called from a game manager script.

    [SerializeField] GameObject choiceBox;
    [SerializeField] Text dialogueText;
    [SerializeField] List<Text> choicesText;

    private string currentNodeGUID;
    private List<DialogueNodeData> nodeData;
    private List<NodeLinkData> linkData;

    private List<NodeLinkData> links;
    private List<string> choices;

    private int currentSelection;

    private void Start() {
        nodeData = dialogueContainer.DialogueNodeData;
        linkData = dialogueContainer.NodeLinks;
        currentNodeGUID = nodeData[nodeData.Count - 1].Guid;
        LoadNode();
    }

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Go to the next node corresponding to the choice
            // Set the current Guid to the one that corresponds with the choice
            if (links.Count > 0) {
                currentNodeGUID = links[currentSelection].TargetNodeGuid;
                LoadNode();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            choicesText[currentSelection].color = Color.black;
            if (currentSelection < choices.Count - 1) {
                currentSelection++;
            } else {
                currentSelection = 0;
            }
            choicesText[currentSelection].color = Color.blue;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            choicesText[currentSelection].color = Color.black;
            if (currentSelection == 0) {
                currentSelection = choices.Count - 1;
            } else {
                currentSelection--;
            }
            choicesText[currentSelection].color = Color.blue;
        }

    }

    private void LoadNode() {

        dialogueText.text = nodeData.Where(x => x.Guid == currentNodeGUID).ToList()[0].DialogueText;

        currentSelection = 0;

        links = linkData.Where(x => x.BaseNodeGuid == currentNodeGUID).ToList();
        choices = new List<string>();

        foreach (var text in choicesText)
        {
            text.gameObject.SetActive(false);
        }

        foreach (var nodeLink in links)
        {
            choices.Add(nodeLink.PortName);
        }
        if (choices.Count == 1) {
            choiceBox.SetActive(false);
        } else {
            choiceBox.SetActive(true);
            choiceBox.GetComponent<RectTransform>().sizeDelta = new Vector2(250, choices.Count * 50);
            for (var i = 0; i < choices.Count; i++) {
                choicesText[i].text = choices[i];
                choicesText[i].gameObject.SetActive(true);
            }
        }

        choicesText[currentSelection].color = Color.blue;
    }
}
