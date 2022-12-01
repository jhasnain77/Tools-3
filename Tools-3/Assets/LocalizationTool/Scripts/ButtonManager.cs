using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] LocalizationText storyText;

    public void Next()
    {
        if (storyText.defaultField.index < 14)
        {
            storyText.defaultField.index = storyText.defaultField.index + 1;
        } else
        {
            storyText.defaultField.index = 3;
        }
        storyText.SetDefaultText();
    }

}
