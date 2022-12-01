using UnityEditor;

// Window for localization editor parent class

public class LocalizationEditor : EditorWindow
{

    protected static LanguagesList languagesList;
    protected static LocalizationData[] localizations;

    [MenuItem("Tools/LocalizationTool/Localization Data/Load the data")]
    public static void LoadData()
    {
        LoadLanguagesList();
        LoadLocalizations();
    }

    public static bool DataLoaded()
    {
        return languagesList != null && localizations != null;
    }

    protected static void LoadLanguagesList()
    {
        string path = "Assets/LocalizationTool/Resources/Languages.asset";
        languagesList = AssetDatabase.LoadAssetAtPath<LanguagesList>(path); // load the ScriptableObject from assets
        if (languagesList == null)
        {
            languagesList = CreateInstance<LanguagesList>(); // create a new object if it not exists
        }
        // if the list of languages is empty create a new array with an one language
        if (languagesList.languages == null || languagesList.languages.Length == 0)
        {
            languagesList.languages = new LanguageData[1];
            languagesList.languages[0] = new LanguageData("lng0", "Language 0");
            languagesList.defaultLanguage = 0;
        }
        if (!AssetDatabase.Contains(languagesList))
        {
            AssetDatabase.CreateAsset(languagesList, path);
        }
        EditorUtility.SetDirty(languagesList);
        AssetDatabase.SaveAssets();
    }

    protected static void SaveLanguages()
    {
        if (!AssetDatabase.Contains(languagesList))
        {
            AssetDatabase.CreateAsset(languagesList, "Assets/LocalizationTool/Resources/Languages.asset");
        }
        EditorUtility.SetDirty(languagesList);
        AssetDatabase.SaveAssets();
    }

    // load a localization data of an each language from the list
    protected static void LoadLocalizations()
    {
        LanguageData[] languages = languagesList.languages;
        localizations = new LocalizationData[languages.Length];
        for (int i = 0; i < localizations.Length; i++)
        {
            localizations[i] = LoadLocalizationAsset(languages[i].index);
        }
    }

    protected static LocalizationData LoadLocalizationAsset(string index)
    {
        string path = "Assets/LocalizationTool/Resources/" + index + ".asset";
        LocalizationData localizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(path); // load the ScriptableObject from assets
        if (localizationData == null)
        {
            localizationData = CreateInstance<LocalizationData>(); // create a new object if it not exists
        }
        // if the list of fields is empty create a new array with an one field
        if (localizationData.fields == null || localizationData.fields.Length == 0)
        {
            localizationData.fields = new string[1];
            localizationData.fields[0] = "Text of localization 0";
        }
        if (!AssetDatabase.Contains(localizationData))
        {
            AssetDatabase.CreateAsset(localizationData, path);
        }
        EditorUtility.SetDirty(localizationData);
        AssetDatabase.SaveAssets();
        return localizationData;
    }

    protected static void SaveLocalization(int index)
    {
        if (!AssetDatabase.Contains(localizations[index]))
        {
            AssetDatabase.CreateAsset(localizations[index], "Assets/LocalizationTool/Resources/" + languagesList.languages[index].index + ".asset");
        }
        EditorUtility.SetDirty(localizations[index]);
        AssetDatabase.SaveAssets();
    }

    protected static void DeleteLocalizationAsset(string index)
    {
        AssetDatabase.DeleteAsset("Assets/LocalizationTool/Resources/" + index + ".asset");
    }

    protected static void SaveLocalizations()
    {
        for (int i = 0; i < localizations.Length; i++)
        {
            SaveLocalization(i);
            // because the index of a localization could be changed set it again
            AssetDatabase.RenameAsset("Assets/LocalizationTool/Resources/" + localizations[i].name + ".asset", languagesList.languages[i].index);
        }
    }

    // get a few first elements of an array
    // this method is used to change a length of an array
    protected static T[] GetFirstElements<T>(int length, int limit, T[] oldElements)
    {
        T[] newElements = new T[length];
        for (int i = 0; i < limit; i++)
        {
            newElements[i] = oldElements[i];
        }
        return newElements;
    }

    [MenuItem("Tools/LocalizationTool/Localization Data/Unload the data")]
    public static void RemoveData()
    {
        SaveLanguages();
        SaveLocalizations();
        languagesList = null;
        localizations = null;
    }

    public static void OpenLanguagesListEditor()
    {
        EditorWindow window = GetWindow<LanguagesListEditor>();
        window.Show();
    }

    public static void OpenLocalizationDataEditor()
    {
        EditorWindow window = GetWindow<LocalizationDataEditor>();
        window.Show();
    }

    public static LocalizationData GetDefaultLocalization()
    {
        return localizations[languagesList.defaultLanguage];
    }

    public static LanguageData[] GetLanguages()
    {
        return languagesList.languages;
    }

    public static string GetLocalizationField(int localizationIndex, int fieldIndex)
    {
        return localizations[localizationIndex].fields[fieldIndex];
    }
}