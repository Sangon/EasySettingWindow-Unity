using UnityEngine;
using UnityEngine.Events;

public class FloatInput : MonoBehaviour {

    private Animator m_anim;
    private UnityEngine.UI.InputField m_inputField;

    private void Start() {
        m_inputField = GetComponent<UnityEngine.UI.InputField>();
    }

    public void Bind(UnityAction<string> unityAction) {
        m_inputField.onValueChanged.AddListener(unityAction);
    }

    public void Unbind(UnityAction<string> unityAction) {
        m_inputField.onValueChanged.AddListener(unityAction);
    }
}
