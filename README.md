# Easy setting window

Fully self contained and easy to use setting window for small projects made in unity. Minimal setup required to use with existing scripts. 

## Info

Currently only works on non-static int/float/boolean fields and properties.
SettingWindowHandler singleton gets initialized in Awake, only add objects in Start or after.

SettingsWindow/Examples/Scenes/Example.unity contains an already set up example scene.

## Getting Started

1. Copy the SettingWindow folder from Assets to your own project.
2. Add SettingsUI from SettingsWindow/Prefabs to your scene.
3. Decorate the variables from your objects you want to appear in the settings with SettingWindowItem attribute. The variable can be private or public.
```C#

    [SettingWindowItem("Sun enabled")]
    public bool SunOn {
        get {
            return Sun.enabled;
        }
        set {
            Sun.enabled = value;
        }
    }
    
    [SettingWindowItem("Sun intensity")]
    public float SunIntensity {

        get {
            return Sun.intensity;
        }
        set {
            Sun.intensity = value;
        }

    }

```
4. Add your object to the settings window by calling **SettingWindowHandler.Instance.AddBlock(object, "Title")** from somewhere in the code. (Start method is a good place)

```C#
        SettingWindowHandler.Instance.AddBlock(this, "Lights");
```
5. The object should appear in the Settings window:

![Setting window](settingswindow.png?raw=true)

Complete code:
```C#
using UnityEngine;

public class LightController : MonoBehaviour {

    [SerializeField]
    private Light Sun;

    [SettingWindowItem("Sun enabled")]
    public bool SunOn {
        get {
            return Sun.enabled;
        }
        set {
            Sun.enabled = value;
        }
    }
    
    [SettingWindowItem("Sun intensity")]
    public float SunIntensity {

        get {
            return Sun.intensity;
        }
        set {
            Sun.intensity = value;
        }

    }

    void Start () {
        //Add the object to the setting window handler so it knows to add our settings.
        SettingWindowHandler.Instance.AddBlock(this, "Lights");
    }

}
```