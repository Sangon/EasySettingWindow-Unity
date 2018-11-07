using UnityEngine;

public class LightController : MonoBehaviour {

    [SerializeField]
    private Light Sun;
    [SerializeField]
    private Light GreenLight;
    [SerializeField]
    private Light RedLight;

    [SettingWindowItem("Sun enabled")]
    public bool SunOn {
        get {
            return Sun.enabled;
        }
        set {
            Sun.enabled = value;
        }
    }

    [SettingWindowItem("Green light enabled")]
    public bool GreenLightOn {

        get {
            return GreenLight.enabled;
        }
        set {
            GreenLight.enabled = value;
        }

    }

    [SettingWindowItem("Red light enabled")]
    public bool RedLightOn {

        get {
            return RedLight.enabled;
        }
        set {
            RedLight.enabled = value;
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

    [SettingWindowItem("Green light intensity")]
    public float GreenLightIntensity {

        get {
            return GreenLight.intensity;
        }
        set {
            GreenLight.intensity = value;
        }

    }

    [SettingWindowItem("Red light intensity")]
    public float RedLightIntensity {

        get {
            return RedLight.intensity;
        }
        set {
            RedLight.intensity = value;
        }

    }

    void Start () {
        //Add the object to the setting window handler so it knows to add our settings.
        SettingWindowHandler.Instance.AddBlock(this, "Lights");
    }

}
