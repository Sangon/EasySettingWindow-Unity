//using System;
//using System.Collections.Generic;
//using UnityEditor;


////[CustomEditor(typeof(SettingWindowHandler))]
//public class SettingWindowHandlerEditor : Editor {

//    private SerializedProperty SettingBlockTemplate;
//    private SerializedProperty Content;

//    private SerializedProperty BooleanSettingItemTemplate;
//    private SerializedProperty IntSettingItemTemplate;
//    private SerializedProperty FloatSettingItemTemplate;

//    private void OnEnable() {

//        SettingBlockTemplate = serializedObject.FindProperty("SettingBlockTemplate");
//        Content = serializedObject.FindProperty("Content");

//        BooleanSettingItemTemplate = serializedObject.FindProperty("BooleanSettingItemTemplate");
//        IntSettingItemTemplate = serializedObject.FindProperty("IntSettingItemTemplate");
//        FloatSettingItemTemplate = serializedObject.FindProperty("FloatSettingItemTemplate");

//    }

//    public override void OnInspectorGUI() {

//        serializedObject.Update();
//        EditorGUILayout.PropertyField(SettingBlockTemplate);
//        EditorGUILayout.PropertyField(Content);

//        EditorGUILayout.Space();

//        EditorGUILayout.PropertyField(BooleanSettingItemTemplate);
//        EditorGUILayout.PropertyField(IntSettingItemTemplate);
//        EditorGUILayout.PropertyField(FloatSettingItemTemplate);

//        //if (GUILayout.Button("Refresh settings")) {

//        //    GameObject contentGo = (GameObject)Content.objectReferenceValue;

//        //    foreach (Transform child in contentGo.transform) {
//        //        DestroyImmediate(child.gameObject);
//        //    }

//        //    foreach (var block in GetSettingBlocks()) {

//        //        var fields = block.GetFields();
//        //        GameObject blockTemplate = Instantiate((GameObject)SettingBlockTemplate.objectReferenceValue, contentGo.transform);
//        //        blockTemplate.transform.Find("SettingBlockTitle").GetComponent<Text>().text = ((SettingWindowBlockAttribute)block.GetCustomAttributes(typeof(SettingWindowBlockAttribute),true)[0]).Title;

//        //        foreach (var field in fields) {

//        //            foreach (Attribute attribute in field.GetCustomAttributes(true)) {

//        //                if (attribute.GetType() == typeof(SettingWindowItemAttribute)) {
//        //                    GameObject item = null;
//        //                    Type fieldtype = field.FieldType;

//        //                    //Only support these datatypes for now.
//        //                    switch (fieldtype.ToString()) {
//        //                        case "System.Single":
//        //                            item = Instantiate((GameObject)FloatSettingItemTemplate.objectReferenceValue, blockTemplate.transform);

//        //                            object value;
//        //                            //Get the instance of the object
//        //                            SettingWindowHandler.Instance.SettingWindowBlocks.TryGetValue(block, out value);
//        //                            item.transform.Find("SettingItemText").GetComponent<InputField>().text = (string)field.GetValue(value);

//        //                            break;
//        //                        case "System.Boolean":
//        //                            item = Instantiate((GameObject)BooleanSettingItemTemplate.objectReferenceValue, blockTemplate.transform);
//        //                            break;
//        //                        case "System.Int32":
//        //                            item = Instantiate((GameObject)IntSettingItemTemplate.objectReferenceValue, blockTemplate.transform);
//        //                            break;
//        //                        default:
//        //                            break;
//        //                    }

//        //                    //The title should be same in everything.
//        //                    if (item) {
//        //                        item.transform.Find("SettingItemText").GetComponent<Text>().text = ((SettingWindowItemAttribute)attribute).Description;
//        //                    }

//        //                }
//        //            }
//        //        }
//        //    }
//        //}

//        serializedObject.ApplyModifiedProperties();
//    }


//    private IEnumerable<Type> GetSettingBlocks() {

//        foreach (Type type in typeof(SettingWindowBlockAttribute).Assembly.GetTypes()) {

//            if (type.GetCustomAttributes(typeof(SettingWindowBlockAttribute), true).Length > 0) {
//                yield return type;
//            }

//        }

//    }
//}
