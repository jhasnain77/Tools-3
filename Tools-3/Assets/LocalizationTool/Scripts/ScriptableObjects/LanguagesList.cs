using UnityEngine;

// Languages scriptable object
[CreateAssetMenu(fileName = "New LanguagesList", menuName = "LocalizationTool/Languages", order = 52)]
public class LanguagesList : ScriptableObject
{
    public int defaultLanguage;
    public LanguageData[] languages;
}

// Additional class to store language data for different languages
[System.Serializable]
public class LanguageData
{
    public string index, name;

    public LanguageData(string index, string name)
    {
        this.index = index;
        this.name = name;
    }
}
