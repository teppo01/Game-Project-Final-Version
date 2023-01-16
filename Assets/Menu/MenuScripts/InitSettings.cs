using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSettings : MonoBehaviour
{

    public SettingsMenu settingsMenu;
    void Start()
    {
        // Jos l�ytyy parempi tapa ladata asetukset k�ynnistyksess�, lupa toteuttaa
        // TODO: testaa buildissa eri asetukset l�pi ja tallentuuko ne (fullscreen toggle ja resoluutio tod.n�k. rikki viel)
        float mainVolume = PlayerPrefs.GetFloat("MasterVolume");
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");
        float mouseSens = PlayerPrefs.GetFloat("mouseSensitivity");
        if (mainVolume == 0) mainVolume = 1;
        if (musicVolume == 0) musicVolume = 1;
        if (effectsVolume == 0) effectsVolume = 1;
        if (mouseSens == 0) mouseSens = 250;

        //settingsMenu.SetResolution(resolution);
        settingsMenu.SetMasterVolume(mainVolume);
        settingsMenu.SetMusicVolume(musicVolume);
        settingsMenu.SetEffectsVolume(effectsVolume);
        settingsMenu.SetSensitivity(mouseSens);
    }

}
