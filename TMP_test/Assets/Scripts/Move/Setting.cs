using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Setting : MonoBehaviour
{
    public AudioMixer mixer; // mixer 의 사운드 -80 ~ 0 : slider의 사운드 -40 ~ 0 : scroll의 사운드 0 ~ 1
    // mixer 종류(float) : BGM,SE
    public Slider slider_BGM;
    public Slider slider_SE;

    Dialogue dialogue;

    public bool isChange;
    void Start()
    {
        slider_BGM.value = 0f;
        slider_SE.value = 0f;
        mixer.SetFloat("BGM", 0f);
        mixer.SetFloat("SE", 0f);
        dialogue = GetComponent<Dialogue>();

        isChange = false;
    }
    public void AudioControl_BGM()
	{
        float sound = slider_BGM.value;

        if (sound == -40f) mixer.SetFloat("BGM", -80);
        else mixer.SetFloat("BGM", sound);
	}
    public void AudioControl_SE()
	{
        float sound = slider_SE.value;
        if (sound == -40f) mixer.SetFloat("SE", -80);
        else mixer.SetFloat("SE", sound);

         isChange = true;
	}

	private void Update()
	{
        if (isChange)
        {
            if (Input.GetMouseButtonUp(0))
            {
                dialogue.playsound_SE();
                isChange = false;
            }
        }
	}
}