using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace InputCommand
{
    /// <summary>
    /// KeyList��Editer
    /// </summary>
    [CustomEditor(typeof(KeyList))]
    public class KeyListEditor : Editor
    {
        private SerializedProperty keysCountProperty;   //�L�[�̐�(�\���L�[��`��)�@(��)�Q�[���{�[�C�Ȃ�2�A���͂Ȃ�2
        private SerializedProperty keysProperty;        //�L�[�̃v���p�e�B�@Input.Button(X)��X�ƃR�}���h�Ŏg�p����Y��ێ�

        /// <summary>
        /// �C���X�y�N�^���L���ɂȂ�����
        /// </summary>
        private void OnEnable()
        {
            keysCountProperty = serializedObject.FindProperty("keysCount");
            keysProperty = serializedObject.FindProperty("keys");
        }

        /// <summary>
        /// �C���X�y�N�^�X�V
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("�{�^���ƃL�[���Ή��\");
            EditorGUILayout.PropertyField(keysCountProperty, GUIContent.none);

            if (keysCountProperty.intValue < 0)
                //�{�^�����͂�-1�Ȃ�o�O��B
            {
                EditorGUILayout.HelpBox("Element count cannot be negative.", MessageType.Error);
                return;
            }
            else
            {
                EditorGUILayout.LabelField("���Ƀ{�^����(Attack�Ƃ�Jump�Ƃ�)��");
                EditorGUILayout.LabelField("�E�ɃR�}���h�Ɏg��(A�Ƃ�J�Ƃ�)�����Ă�������");
                if (keysProperty.isArray)
                {
                    keysProperty.arraySize = keysCountProperty.intValue;
                    EditorGUI.indentLevel++;

                    for (int i = 0; i < keysProperty.arraySize; i++)
                    {
                        SerializedProperty elementProperty = keysProperty.GetArrayElementAtIndex(i);
                        //����s
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
