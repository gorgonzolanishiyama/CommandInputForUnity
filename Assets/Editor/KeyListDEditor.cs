using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace InputCommand
{
    /// <summary>
    /// KeyListのEditer
    /// </summary>
    [CustomEditor(typeof(KeyList))]
    public class KeyListEditor : Editor
    {
        private SerializedProperty keysCountProperty;   //キーの数(十字キーを覗く)　(例)ゲームボーイなら2、武力なら2
        private SerializedProperty keysProperty;        //キーのプロパティ　Input.Button(X)のXとコマンドで使用するYを保持

        /// <summary>
        /// インスペクタが有効になった時
        /// </summary>
        private void OnEnable()
        {
            keysCountProperty = serializedObject.FindProperty("keysCount");
            keysProperty = serializedObject.FindProperty("keys");
        }

        /// <summary>
        /// インスペクタ更新
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("ボタンとキー名対応表");
            EditorGUILayout.PropertyField(keysCountProperty, GUIContent.none);

            if (keysCountProperty.intValue < 0)
                //ボタン入力が-1ならバグる。
            {
                EditorGUILayout.HelpBox("Element count cannot be negative.", MessageType.Error);
                return;
            }
            else
            {
                EditorGUILayout.LabelField("左にボタン名(AttackとかJumpとか)を");
                EditorGUILayout.LabelField("右にコマンドに使う(AとかJとか)を入れてください");
                if (keysProperty.isArray)
                {
                    keysProperty.arraySize = keysCountProperty.intValue;
                    EditorGUI.indentLevel++;

                    for (int i = 0; i < keysProperty.arraySize; i++)
                    {
                        SerializedProperty elementProperty = keysProperty.GetArrayElementAtIndex(i);
                        //横一行
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUIUtility.labelWidth = 0.02f;
                            EditorGUILayout.LabelField(i + ":");
                            EditorGUILayout.PropertyField(elementProperty.FindPropertyRelative("button"), GUIContent.none);
                            EditorGUILayout.PropertyField(elementProperty.FindPropertyRelative("outputWord"), GUIContent.none);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUI.indentLevel--;
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
