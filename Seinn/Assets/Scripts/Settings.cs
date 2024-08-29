using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Slider volSlider;
    public Toggle fullScreenToggle;
    public TMP_Dropdown ResDropdown;

    private Resolution[] res;

    void Start()
    {
        SetResolutionList();
        SetSliderValue();
        SetFullScreenValue(); 
    }

    private void SetFullScreenValue()
    {
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    private void SetSliderValue()
    {
        float audioLevel = 0f;
        AudioManager.instance.master.audioMixer.GetFloat("Master", out audioLevel);
        volSlider.value = Mathf.Exp(audioLevel / 20);
    }

    private void SetResolutionList()
    {
        res = Screen.resolutions;
        ResDropdown.ClearOptions();
        List<string> options = new List<string>();
        //int i = 0;
        int currentRes = 11;
        for(int i=0; i<res.Length; i++)
        {
            if (res[i].width / res[i].height == 4 / 3)
            {
                options.Add(res[i].width + "x" + res[i].height + ":" + res[i].refreshRate);

                if (res[i].height == Screen.currentResolution.height &&
                   res[i].width == Screen.currentResolution.width )
                {
                    currentRes = i;
                }
            }
        }
        ResDropdown.AddOptions(options);
        ResDropdown.value =  currentRes;
        ResDropdown.RefreshShownValue();
        ResDropdown.itemText.text = ResDropdown.options[ResDropdown.value].text;
    }

    public void SetResolution(int resIndex)
    {
        Resolution re = res[resIndex];
        Screen.SetResolution(re.width, re.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetMasterLevel(float sliderValue)
    {
        AudioManager.instance.master.audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }
}
