using UnityEngine;

// Data scriptable object 
[CreateAssetMenu(fileName = "New LocalizationData", menuName = "LocalizationTool/Data", order = 53)]
public class LocalizationData : ScriptableObject
{
    public string[] fields;
}