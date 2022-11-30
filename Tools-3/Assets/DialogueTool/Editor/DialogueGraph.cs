using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow {

    private DialogueGraphView graphView;

    private string fileName = "New Dialogue";

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

        var fileNameTextField = new TextField("File Name");
        fileNameTextField.SetValueWithoutNotify(fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => SaveData()) { text = "Save Data"});
        toolbar.Add(new Button(() => LoadData()) { text = "Load Data"});

        var nodeCreateButton = new Button(() => {
            graphView.CreateNode("Dialogue Node");
        });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void SaveData() {
        if (string.IsNullOrEmpty(fileName)) {
            EditorUtility.DisplayDialog("Invalid file name!", "Enter a valid file name.", "Ok");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(graphView);
        saveUtility.SaveGraph(fileName);
    }

    private void LoadData() {
        if (string.IsNullOrEmpty(fileName)) {
            EditorUtility.DisplayDialog("Invalid file name!", "Enter a valid file name.", "Ok");
            return;
        }

        var loadUtility = GraphSaveUtility.GetInstance(graphView);
        loadUtility.LoadGraph(fileName);
    }
}