using UnityEngine;
using UnityEditor;

// Data Editor for specific fields

public class LocalizationDataEditor : LocalizationEditor
{

    int selectedField;

    [MenuItem("Tools/LocalizationTool/Data Editor/Localization Data")]
    static void Init()
    {
        OpenLocalizationDataEditor();
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit the list of the languages"))
        {
            EditorGUI.FocusTextInControl(null);
            OpenLanguagesListEditor();
        }
        if (GUILayout.Button("Load the localization data"))
        {
            EditorGUI.FocusTextInControl(null);
            LoadData();
            selectedField = 0;
        }
        if (!DataLoaded())
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Localization data not loaded...", MessageType.Info);
            return;
        }
        if (GUILayout.Button("Save the localizations"))
        {
            EditorGUI.FocusTextInControl(null);
            SaveLocalizations();
            selectedField = 0;
        }
        EditorGUILayout.EndHorizontal();
        LocalizationData defaultLocalization = GetDefaultLocalization();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add a new field"))
        {
            EditorGUI.FocusTextInControl(null);
            AddNewFields();
        }
        if (GUILayout.Button("Selected field " + selectedField + ": " + defaultLocalization.fields[selectedField].SubstringField(20)))
        {
            EditorGUI.FocusTextInControl(null);
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < defaultLocalization.fields.Length; i++)
            {
                menu.AddItem(new GUIContent(i + ": " + defaultLocalization.fields[i].SubstringField(20)), i == selectedField, SelectField, i);
            }
            menu.ShowAsContext();
        }
        if (GUILayout.Button("Remove the field " + selectedField.ToString()))
        {
            EditorGUI.FocusTextInControl(null);
            RemoveSelectedFields();
        }
        EditorGUILayout.EndHorizontal();
        int defaultLanguage = languagesList.defaultLanguage;
        LanguageData[] languages = GetLanguages();
        EditorGUILayout.LabelField(languages[defaultLanguage].name + ": ", EditorStyles.boldLabel);
        defaultLocalization.fields[selectedField] = EditorGUILayout.TextArea(defaultLocalization.fields[selectedField], GUILayout.MaxWidth(position.width - 30));
        EditorGUILayout.Space();
        if (languages.Length > 1)
        {
            EditorGUILayout.LabelField("Another languages: ", EditorStyles.boldLabel);
            for (int l = 0; l < languages.Length; l++)
            {
                if (l != defaultLanguage)
                {
                    AddFields(l, defaultLocalization.fields.Length - localizations[l].fields.Length);
                    EditorGUILayout.LabelField(languages[l].name + ": ");
                    localizations[l].fields[selectedField] = EditorGUILayout.TextArea(GetLocalizationField(l, selectedField), GUILayout.MaxWidth(position.width - 30));
                }
            }
            EditorGUILayout.Space();
        }
        if (GUILayout.Button("Copy in the new field"))
        {
            EditorGUI.FocusTextInControl(null);
            CopyFields();
        }
    }

    void SelectField(object i)
    {
        selectedField = (int)i;
    }

    void AddNewFields()
    {
        for (int i = 0; i < localizations.Length; i++)
        {
            AddFields(i, 1);
        }
        SelectField(GetDefaultLocalization().fields.Length - 1);
    }

    void AddFields(int localizationIndex, int count)
    {
        if (count > 0)
        {
            int l = localizations[localizationIndex].fields.Length;
            string[] fields = GetFirstElements(l + count, l, localizations[localizationIndex].fields);
            for (int i = l; i < fields.Length; i++)
            {
                fields[i] = "Text of localization " + i.ToString();
            }
            localizations[localizationIndex].fields = fields;
        }
    }

    void RemoveSelectedFields()
    {
        if (localizations[languagesList.defaultLanguage].fields.Length > 1)
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to remove the field " + selectedField.ToString() + "?", "YES", "NO"))
            {
                for (int i = 0; i < localizations.Length; i++)
                {
                    string[] newFields = GetFirstElements(localizations[i].fields.Length - 1, selectedField, localizations[i].fields);
                    for (int f = selectedField; f < newFields.Length; f++)
                    {
                        newFields[f] = GetLocalizationField(i, f + 1);
                    }
                    localizations[i].fields = newFields;
                }
                selectedField = Mathf.Clamp(selectedField - 1, 0, GetDefaultLocalization().fields.Length - 1);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Unable to remove selected field!", "Localization must have at least one field!", "OK");
        }
    }

    void CopyFields()
    {
        string[] fields = new string[GetLanguages().Length];
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = localizations[i].fields[selectedField];
        }
        AddNewFields();
        for (int i = 0; i < localizations.Length; i++)
        {
            localizations[i].fields[selectedField] = fields[i];
        }
    }
}