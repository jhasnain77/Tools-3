using UnityEngine;
using UnityEngine.UI;

public class LanguagesListObject : MonoBehaviour
{

    public LanguageToggle sample;
    public Transform languagesParent;
    public Button systemLanguageButton;
    LanguageToggle[] toggles;

    public void LoadLanguages()
    {
        LanguagesList list = Localization.GetLanguages();
        LanguageData[] languages = list.languages;
        toggles = new LanguageToggle[languages.Length];
        for (int i = 0; i < languages.Length; i++)
        {
            LanguageToggle languageToggle = Instantiate(sample, languagesParent);
            languageToggle.SetData(languages[i], i == list.defaultLanguage, SetLanguage);
            languageToggle.gameObject.name = "Language " + i;
            languageToggle.gameObject.SetActive(true);
            toggles[i] = languageToggle;
        }
        Destroy(sample);
        string defaultLang = languages[list.defaultLanguage].index;
        SetLanguage(defaultLang); // load the default localization
        systemLanguageButton.onClick.RemoveAllListeners();
        systemLanguageButton.onClick.AddListener(SetSystemLanguage);
        systemLanguageButton.interactable = true;
    }

    void SetLanguage(string index)
    {
        Localization.Load(index);
    }

    void SetToggle(string index)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if(toggles[i].languageIndex == index)
            {
                toggles[i].toggle.isOn = true;
                break;
            }
        }
    }

    void SetSystemLanguage()
    {
        string lang = "en";
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                lang = "en";
                break;
            case SystemLanguage.French:
                lang = "fr";
                break;
            default:
                lang = "en";
                break;
        }
        SetLanguage(lang);
        SetToggle(lang);
    }
}
