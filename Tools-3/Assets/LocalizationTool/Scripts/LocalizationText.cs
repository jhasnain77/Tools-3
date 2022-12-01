using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("UI/Localization/Text")]
public class LocalizationText : MonoBehaviour
{

    [Tooltip("Localization field for the text.")]
    public LocalizationField defaultField;
    public Text text;
    public bool SetOnEnable = true;
    public bool SetOnLocalizationChanged = true;

    void Awake()
    {
        // Call the event to change the language
        Localization.LocalizationChanged += SetDefaultTextOnLocalizationChanged;
    }

    // Set as default text
    void OnEnable()
    {
        if (SetOnEnable)
        {
            SetDefaultText();
        }
    }

    // Set untranslated text
    public void SetText(string text)
    {
        GetComponent();
        this.text.text = text;
    }

    // Translate the default text and then re-assign the default to the translated text
    public void SetDefaultText()
    {
        GetComponent();
        SetText(Localization.GetText(defaultField.index));
    }

    // Translate the text by a field with a specific index
    public void SetTextByIndex(int fieldIndex, bool setAsDefault = true)
    {
        GetComponent();
        SetText(Localization.GetText(fieldIndex));
        if (setAsDefault)
        {
            defaultField.index = fieldIndex;
        }
    }

    void SetDefaultTextOnLocalizationChanged()
    {
        if (SetOnLocalizationChanged)
        {
            SetDefaultText();
        }
    }

    // Set a text component
    void GetComponent()
    {
        if(text == null)
        {
            text = GetComponent<Text>();
        }
    }

    void Reset()
    {
        GetComponent();
    }
}

[System.Serializable]
public class LocalizationField
{
    public int index;
}
