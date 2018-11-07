using UnityEngine;

public class ToggleableSlider : MonoBehaviour {

    private Animator m_anim;
    private UnityEngine.UI.Toggle m_toggle;

    private void Awake() {

        m_anim = GetComponent<Animator>();
        m_toggle = GetComponent<UnityEngine.UI.Toggle>();

        Bind((s) => {
            HandleToggleAnimation();
        });

    }

    private void HandleToggleAnimation() {

        if (m_toggle.isOn) {
            m_anim.Play("ToggleOff");
        } else {
            m_anim.Play("ToggleOn");
        }

    }

    public void Bind(UnityEngine.Events.UnityAction<bool> unityAction) {
        m_toggle.onValueChanged.AddListener(unityAction);
    }

    public void Unbind(UnityEngine.Events.UnityAction<bool> unityAction) {
        m_toggle.onValueChanged.RemoveListener(unityAction);
    }
}
