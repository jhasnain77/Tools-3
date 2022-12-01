using UnityEngine;

public delegate void LocalizationAction();

public static class Localization
{

    public static event LocalizationAction LocalizationChanged; // Event for when localization changes
    public static bool Created { get; private set; } // Boolean to record if the data has been created
    public static string Language { get; private set; } // Language variable
    static string[] fields; // All fields that will need to be changed
    
    // Function to load data for a certain language
    public static void Load(string langIndex)
    {
        Language = langIndex; // Set the language to the desired language
        LocalizationData data = (LocalizationData)Resources.Load(langIndex); // Load data from scriptable objects
        // If there's data, do stuff, otherwise, don't do anything
        if (data != null) {
            fields = data.fields; // Set the fields
            Created = true;
            LocalizationChanged(); // Call event to update language
        }
        else
        {
            fields = null;
            Created = false;
        }
    }

    // Function to get the text for a specific field
    public static string GetText(int fieldIndex)
    {
        // If there hasn't been a field created, create one, otherwise return the text for that field
        if(!Created || fieldIndex > fields.Length || fieldIndex < 0)
        {
            return "Field " + fieldIndex;
        }
        else
        {
            return fields[fieldIndex];
        }
    }

    // Get the list of languages
    public static LanguagesList GetLanguages()
    {
        return (LanguagesList)Resources.Load("Languages");
    }
}