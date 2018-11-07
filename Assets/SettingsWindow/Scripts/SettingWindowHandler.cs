using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindowHandler : MonoBehaviour {

    public static SettingWindowHandler Instance { get; private set; }

    public GameObject SettingBlockTemplate;
    public GameObject Content;

    public GameObject BooleanSettingItemTemplate;
    public GameObject IntSettingItemTemplate;
    public GameObject FloatSettingItemTemplate;

    private Animator anim;
    private bool m_state = true;


    public Dictionary<string, List<object>> SettingWindowBlocks = new Dictionary<string, List<object>>();

    private void Awake() {

        if (Instance == null) {
            Instance = this;
            anim = GetComponentInChildren<Animator>();
        } else {
            Debug.LogError("A SettingWindowHandler already exists on :" + Instance.gameObject.name);
            Destroy(this);
        }

    }

    /// <summary>
    /// Add a settingblock to the handler. If a
    /// To add blocks to the settings window call RefreshSettingsWindow
    /// </summary>
    /// <param name="from"></param>
    /// <param name="title"></param>
    public void AddBlock(object from, string title = "") {
        // TODO: If the object has SettingWindowBlockAttribute we should use the title from that instead.
        //((SettingWindowBlockAttribute)from.GetType().GetCustomAttributes(true).Where(s => s == typeof(SettingWindowBlockAttribute)).ToArray()[0]).Title;

        if (string.IsNullOrEmpty(title)) {
            title = from.GetType().ToString();
        }

        List<object> itemlist;
        SettingWindowBlocks.TryGetValue(title, out itemlist);

        if (itemlist == null) {
            itemlist = new List<object>();
            SettingWindowBlocks.Add(title, itemlist);
        }

        itemlist.Add(from);

        AddBlockToSettingsWindow(from,title);

        //Reset the position of the list when adding new blocks.
        Content.transform.Translate(new Vector3(0, -99999, 0));
    }

    private void AddBlockToSettingsWindow(object settingsItem, string blockName) {

        GameObject blockTemplate = Instantiate(SettingBlockTemplate, Content.transform);
        blockTemplate.transform.Find("SettingBlockTitle").GetComponent<Text>().text = blockName;

        //TODO: How do we plan on handling static variables.
        BindingFlags flags = BindingFlags.Public |
                             BindingFlags.NonPublic |
                             BindingFlags.Instance |
                             BindingFlags.DeclaredOnly;

        var fields = settingsItem.GetType().GetFields(flags);
        var props = settingsItem.GetType().GetProperties(flags);

        //Iterate through all the fields in the object and generate items from them.
        foreach (var field in fields) {
            GenerateItemFromField(field, settingsItem, blockTemplate);
        }
        
        //Iterate through all the properties in the object and generate items from them.
        foreach (var prop in props) {
            GenerateItemFromProperty(prop, settingsItem, blockTemplate);
        }
    }

    private GameObject GenerateItemFromProperty(PropertyInfo prop, object settingsItem, GameObject block) {

        GameObject item = null;
        Type propertyType = prop.PropertyType;

        //Iterate through all the attributes in a field.
        foreach (Attribute attribute in prop.GetCustomAttributes(true)) {
            //If the field is marked with a window item attribute add it to the block.
            if (attribute.GetType() == typeof(SettingWindowItemAttribute)) {
                //We could generate the whole template here instead of providing it from the editor.
                //Only support these datatypes for now.
                switch (propertyType.ToString()) {
                    case "System.Single":
                        item = Instantiate(FloatSettingItemTemplate, block.transform);
                        item.transform.Find("FloatInput").GetComponent<InputField>().text = prop.GetValue(settingsItem,null).ToString();
                        item.transform.Find("FloatInput").GetComponent<InputField>().onValueChanged.AddListener((state) => {

                            try {
                                prop.SetValue(settingsItem, float.Parse(state), null);
                            } catch (FormatException e) {
                                Debug.Log("Tried to set wrong type of value to a property");
                            }

                        });
                        break;
                    case "System.Boolean":
                        item = Instantiate(BooleanSettingItemTemplate, block.transform);
                        item.transform.Find("ToggleableSlider").GetComponent<Toggle>().isOn = (bool)prop.GetValue(settingsItem,null);
                        item.transform.Find("ToggleableSlider").GetComponent<Toggle>().onValueChanged.AddListener((state) => {

                            try {
                                prop.SetValue(settingsItem, state, null);
                            } catch (FormatException e) {
                                Debug.Log("Tried to set wrong type of value to a property");
                            }

                        });
                        break;
                    case "System.Int32":
                        item = Instantiate(IntSettingItemTemplate, block.transform);
                        item.transform.Find("IntInput").GetComponent<InputField>().text = prop.GetValue(settingsItem,null).ToString();
                        item.transform.Find("IntInput").GetComponent<InputField>().onValueChanged.AddListener((state) => {

                            try {
                                prop.SetValue(settingsItem, int.Parse(state), null);
                            } catch (FormatException e) {
                                Debug.Log("Tried to set wrong type of value to a property");
                            }

                        });
                        break;
                    default:
                        break;
                }

                if (item) {
                    item.transform.Find("SettingItemText").GetComponent<Text>().text = ((SettingWindowItemAttribute)attribute).Description;
                }

            }
        }
        return item;

    }


    private GameObject GenerateItemFromField(FieldInfo field, object settingsItem, GameObject block) {

        GameObject item = null;
        Type fieldType = field.FieldType;

        //Iterate through all the attributes in a field
        foreach (Attribute attribute in field.GetCustomAttributes(true)) {
            //If the field is marked with a window item attribute add it to the block.
            if (attribute.GetType() == typeof(SettingWindowItemAttribute)) {
                //We could generate the whole template here instead of providing it from the editor.
                //Only support these datatypes for now.
                switch (fieldType.ToString()) {
                    case "System.Single":
                        item = Instantiate(FloatSettingItemTemplate, block.transform);
                        item.transform.Find("FloatInput").GetComponent<InputField>().text = field.GetValue(settingsItem).ToString();
                        item.transform.Find("FloatInput").GetComponent<InputField>().onValueChanged.AddListener((state) => {

                            try {
                                field.SetValue(settingsItem, float.Parse(state));
                            } catch (FormatException e) {
                                Debug.Log("Tried to set wrong type of value to a field");
                            }

                        });
                        break;
                    case "System.Boolean":
                        item = Instantiate(BooleanSettingItemTemplate, block.transform);
                        item.transform.Find("ToggleableSlider").GetComponent<Toggle>().isOn = (bool)field.GetValue(settingsItem);
                        item.transform.Find("ToggleableSlider").GetComponent<Toggle>().onValueChanged.AddListener((state) => {

                            try {
                                field.SetValue(settingsItem, state);
                            } catch (FormatException e) {
                                Debug.Log("Tried to set wrong type of value to a field");
                            }

                        });

                        break;
                    case "System.Int32":
                        item = Instantiate(IntSettingItemTemplate, block.transform);
                        item.transform.Find("IntInput").GetComponent<InputField>().text = field.GetValue(settingsItem).ToString();
                        item.transform.Find("IntInput").GetComponent<InputField>().onValueChanged.AddListener((state) => {

                            try {
                                field.SetValue(settingsItem, int.Parse(state));
                            } catch (FormatException e) {
                                Debug.Log("Tried to set wrong type of value to a field");
                            }

                        });

                        break;
                    default:
                        break;
                }

                if (item) {
                    item.transform.Find("SettingItemText").GetComponent<Text>().text = ((SettingWindowItemAttribute)attribute).Description;
                }

            }
        }
        return item;

    }

    private void TrySetValue<T>(FieldInfo field, object obj, T value) {
        try {
            field.SetValue(obj, value);
        } catch (FormatException e) {

        }
    }

    public void ToggleSettingWindow() {

        m_state = !m_state;

        if (m_state) {
            anim.Play("FadeWindowOut");
        } else {
            anim.Play("FadeWindowIn");
        }

    }

}