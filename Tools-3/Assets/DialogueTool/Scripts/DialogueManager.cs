using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] DialogueContainer dialogueContainer;   // This is only for debugging and demo. Will include a method that
                                                            // declares this when called from a game manager script.

    [SerializeField] GameObject choiceBox;
    [SerializeField] LocalizationText dialogueText;
    [SerializeField] List<LocalizationText> choicesText;

    private string currentNodeGUID;
    private List<DialogueNodeData> nodeData;
    private List<NodeLinkData> linkData;

    private List<NodeLinkData> links;
    private List<int> choices;

    private int currentSelection;

    private void Start() {
        nodeData = dialogueContainer.DialogueNodeData;
        linkData = dialogueContainer.NodeLinks;
        currentNodeGUID = nodeData[0].Guid;
        LoadNode();
    }

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Z)) {
            // Go to the next node corresponding to the choice
            // Set the current Guid to the one that corresponds with the choice
            if (links.Count > 0) {
                currentNodeGUID = links[currentSelection].TargetNodeGuid;
                LoadNode();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            choicesText[currentSelection].text.color = Color.black;
            if (currentSelection < choices.Count - 1) {
                currentSelection++;
            } else {
                currentSelection = 0;
            }
            choicesText[currentSelection].text.color = Color.blue;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            choicesText[currentSelection].text.color = Color.black;
            if (currentSelection == 0) {
                currentSelection = choices.Count - 1;
            } else {
                currentSelection--;
            }
            choicesText[currentSelection].text.color = Color.blue;
        }

    }

    private void LoadNode() {

        dialogueText.defaultField.index = nodeData.Where(x => x.Guid == currentNodeGUID).ToList()[0].LocalizationIndex;
        dialogueText.SetDefaultText();
        currentSelection = 0;

        links = linkData.Where(x => x.BaseNodeGuid == currentNodeGUID).ToList();
        choices = new List<int>();

        foreach (var text in choicesText)
        {
            text.gameObject.SetActive(false);
        }

        foreach (var nodeLink in links)
        {
            choices.Add(int.Parse(nodeLink.PortName));
            // Debug.Log(int.Parse(nodeLink.PortName));
        }
        choices.Sort();
        if (choices.Count <= 1) {
            choiceBox.SetActive(false);
        } else {
            choiceBox.SetActive(true);
            choiceBox.GetComponent<RectTransform>().sizeDelta = new Vector2(250, choices.Count * 50);
            for (var i = 0; i < choices.Count; i++) {
                choicesText[i].defaultField.index = choices[i];
                choicesText[i].SetDefaultText();
                choicesText[i].gameObject.SetActive(true);
                choicesText[i].text.color = Color.black;
            }
        }

        choicesText[currentSelection].text.color = Color.blue;
    }
}
