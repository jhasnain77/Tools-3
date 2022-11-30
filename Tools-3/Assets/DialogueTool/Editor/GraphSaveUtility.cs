using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class GraphSaveUtility
{

    private DialogueGraphView targetGraph;
    private DialogueContainer containerCache;

    private List<Edge> Edges => targetGraph.edges.ToList();
    private List<DialogueNode> Nodes => targetGraph.nodes.ToList().Cast<DialogueNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView) {
        return new GraphSaveUtility {
            targetGraph = targetGraphView
        };
    }

    public void SaveGraph(string fileName) {
        if (!Edges.Any()) {
            return;
        }

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

        for (var i = 0; i < connectedPorts.Length; i++) {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            dialogueContainer.NodeLinks.Add(new NodeLinkData {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }

        foreach (var node in Nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData {
                Guid = node.GUID,
                DialogueText = node.DialogueText,
                Position = node.GetPosition().position
            });
        }

        if (!AssetDatabase.IsValidFolder("Assets/DialogueTool/Resources")) {
            AssetDatabase.CreateFolder("Assets/DialogueTool", "Resources");
        }

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/DialogueTool/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName) {
        containerCache = Resources.Load<DialogueContainer>(fileName);

        if (containerCache == null) {
            EditorUtility.DisplayDialog("File Not Found", "The requested file does not exist.", "Ok");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ClearGraph() {
        Nodes.Find(x => x.EntryPoint).GUID = containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (var node in Nodes) {
            if (node.EntryPoint) {
                continue;
            }
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => targetGraph.RemoveElement(edge));
            
            targetGraph.RemoveElement(node);
        }
    }

    private void CreateNodes() {
        foreach (var nodeData in containerCache.DialogueNodeData)
        {
            var temp = targetGraph.CreateDialogueNode(nodeData.DialogueText);
            temp.GUID = nodeData.Guid;
            targetGraph.AddElement(temp);

            var nodePorts = containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => targetGraph.AddChoicePort(temp, x.PortName));

        }
    }

    private void ConnectNodes() {
        // Debug.Log(Nodes.Count);
        for (var i = 0; i < Nodes.Count; i++)
        {
            // Debug.Log(Nodes[i]);
            var connections = containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
            // Debug.Log(connections.Count);
            for (var j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(containerCache.DialogueNodeData.First(x => 
                    x.Guid == targetNodeGuid).Position, targetGraph.defaultNodeSize));
            }
        }
    }
    
    private void LinkNodes(Port output, Port input) {
        var temp = new Edge {
            output = output,
            input = input
        };

        temp?.input.Connect(temp);
        temp?.output.Connect(temp);
        targetGraph.Add(temp);
    }

}
