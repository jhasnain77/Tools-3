using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow {

    private DialogueGraphView graphView;

    [MenuItem("Tools/DialogueGraph")]
    private static void ShowWindow() {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("DialogueGraph");
        window.Show();
    }

    private void OnEnable() {

        ConstructGraphView();
        GenerateToolbar();

    }

    private void OnDisable() {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView() {
        graphView = new DialogueGraphView{
            name = "Dialogue Graph"
        };

        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar() {
        var toolbar = new Toolbar();

        var nodeCreateButton = new Button(() => {
            graphView.CreateNode("Dialogue Node");
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }
}