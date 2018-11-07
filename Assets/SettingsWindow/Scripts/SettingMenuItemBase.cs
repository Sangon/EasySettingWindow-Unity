using UnityEngine;

public abstract class SettingMenuItemBase : MonoBehaviour {

    public abstract void Bind(UnityEngine.Events.UnityAction<bool> unityAction);
    public abstract void Unbind(UnityEngine.Events.UnityAction<bool> unityAction);

}
