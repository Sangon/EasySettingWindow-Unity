using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereObject : MonoBehaviour {

    private float m_sphereRadius = 1f;

    //Properties or fields can be private
    [SettingWindowItem("Radius of the sphere")]
    private float SphereRadius {

        get {
            return m_sphereRadius;
        }

        set {
            transform.localScale = new Vector3(value, value, value);
            m_sphereRadius = value;
        }
    }

    private void Start() {

        //Add this object to the settings window handler so it knows to include our variables.
        SettingWindowHandler.Instance.AddBlock(this, name);
    }

}
