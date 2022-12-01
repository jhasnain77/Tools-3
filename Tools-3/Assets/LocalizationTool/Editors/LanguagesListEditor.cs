using UnityEngine;
using UnityEditor;

// Language editor for adding and editing languages

public class LanguagesListEditor : LocalizationEditor
{

    int selectedLanguage;

    [MenuItem("Tools/LocalizationTool/Data Editor/Languages List")]
    static void Init()
    {
        OpenLanguagesListEditor();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("File: Assets/LocalizationTool/Resources/Languages.asset");
        if (GUILayout.Button("Load the localization data"))
        {
            EditorGUI.FocusTextInControl(null);
            LoadData();
            selectedLanguage = 0;
        }
        if (!DataLoaded())
        {
            EditorGUILayout.HelpBox("Localization data not loaded...", MessageType.Info);
            return;
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save the languages list"))
        {
            EditorGUI.FocusTextInControl(null);
            SaveLanguages();
            SaveLocalizations();
        }
        if (GUILayout.Button("Default language: " + languagesList.languages[languagesList.defaultLanguage].name))
        {
            EditorGUI.FocusTextInControl(null);
            GenericMenu menu = new GenericMenu();
            LanguageData[] languages = languagesList.languages;
            for (int i = 0; i < languages.Length; i++)
            {
                menu.AddItem(new GUIContent(languages[i].name), i == languagesList.defaultLanguage, OnDefaultLanguageSelected, i);
            }
            menu.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add a new language"))
        {
            EditorGUI.FocusTextInControl(null);
            AddLanguage();
        }
        LanguageData languageData = languagesList.languages[selectedLanguage];
        if (GUILayout.Button("Selected language: " + languageData.name))
        {
            EditorGUI.FocusTextInControl(null);
            GenericMenu menu = new GenericMenu();
            LanguageData[] languages = languagesList.languages;
            for (int i = 0; i < languages.Length; i++)
            {
                menu.AddItem(new GUIContent(languages[i].name), i == selectedLanguage, OnLanguageSelected, i);
            }
            menu.ShowAsContext();
        }
        if (GUILayout.Button("Remove this language"))
        {
            EditorGUI.FocusTextInControl(null);
            DeleteSelectedLanguage();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Language data:", EditorStyles.boldLabel);
        languageData.index = EditorGUILayout.TextField("Index: ", languageData.index);
        languageData.name = EditorGUILayout.TextField("Name: ", languageData.name);
    }

    void OnLanguageSelected(object i)
    {
        selectedLanguage = (int)i;
    }

    void OnDefaultLanguageSelected(object i)
    {
        languagesList.defaultLanguage = (int)i;
    }

    void AddLanguage()
    {
        int i = languagesList.languages.Length;
        LanguageData languageData = new LanguageData("lng" + i.ToString(), "Language " + i); // create a new language
        LanguageData[] newLanguages = GetFirstElements(i + 1, i, languagesList.languages); // create a new array with more length of elements
        newLanguages[i] = languageData; // append the language
        languagesList.languages = newLanguages;
        LocalizationData localizationData = LoadLocalizationAsset("lng" + i); // create a new localization
        LocalizationData[] newlocalizations = GetFirstElements(i + 1, i, localizations); // create a new array with more length of elements
        newlocalizations[i] = localizationData; // append the localization
        localizations = newlocalizations;
        SaveLanguages();
        selectedLanguage = i;
    }

    void DeleteSelectedLanguage()
    {
        if (selectedLanguage != languagesList.defaultLanguage)
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to remove the language \"" + languagesList.languages[selectedLanguage].name + "\" from localization?", "YES", "NO"))
            {
                LanguageData[] oldLanguages = languagesList.languages;
                int length = oldLanguages.Length - 1;
                DeleteLocalizationAsset(oldLanguages[selectedLanguage].index);
                // make the language last in the array
                if (selectedLanguage != length)
                {
                    LanguageData lastLanguage = oldLanguages[length];
                    oldLanguages[length] = oldLanguages[selectedLanguage];
                    oldLanguages[selectedLanguage] = lastLanguage;
                    LocalizationData lastLocalization = localizations[length];
                    localizations[length] = localizations[selectedLanguage];
                    localizations[selectedLanguage] = lastLocalization;
                }
                languagesList.languages = GetFirstElements(length, length, oldLanguages); // create a new array wihout last element
                localizations = GetFirstElements(length, length, localizations); // create a new array wihout last element
                selectedLanguage = Mathf.Clamp(selectedLanguage - 1, 0, length - 1);
                SaveLanguages();
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Unable to remove default language of localization!", "\"" + languagesList.languages[selectedLanguage].name + "\" is default language of localization and you can't remove it from list!", "OK");
        }
    }
}