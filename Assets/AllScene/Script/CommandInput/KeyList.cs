using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace InputCommand
{
    //Unity��PrefaranceSetteing�ݒ�L�[�ƃR�}���h�Ɏg�p����o�̓L�[�̑Ή��\
    //ScriptableObject
    //
    [CreateAssetMenu(fileName = "ButtonList", menuName = "ScriptableObjects/ButtonNameSet", order = 1)]
    public class KeyList : ScriptableObject
    {

        public int keysCount = 0;                   //�@Inspecter�Ŏg�p����B�@���̓��X�g��
        public List<Key> keys = new List<Key>();    //�@Inspecter�Ŏg�p����B�@���͂���L�[�̑Ή��v�f
                                                    //�f�t�H���g�\���L�[�̐ݒ�L�[�o�̓L�\�Ή��\
        public Dictionary<string, char> Name_CrossKey = new Dictionary<string, char>();
        //�\���L�[�ȊO�̃{�^���̐ݒ�L�[�o�̓L�\�Ή��\
        Dictionary<string, char> Name_Button = new Dictionary<string, char>();

        //Name_CrossKey��Name_Button�̐ݒ�L�[�o�̓L�\-�����\
        Dictionary<string, char> Integral_Name = new Dictionary<string, char>();

        /// <summary>
        /// �C���X�y�N�^�N����
        /// keys ���X�g���� Name_Button ���쐬���AName_CrossKey �Ɠ������� Integral_Name �𐶐����܂��B
        /// </summary>
        public void OnEnable()
        {
            Name_CrossKey = new Dictionary<string, char>()
        {
                { "Up"      , '8' },
                { "Down"    , '2' },
                { "Left"    , '4' },
                { "Right"   , '6' }
        };

            ////���͂���Ă���keys����Name_Button���쐬����B
            foreach (Key tempkey in keys)
            //List<Key>��Dictionary<char,string>�`���ɕϊ�����B
            {
                Name_Button[tempkey.GetButton()] = tempkey.GetOutputWord();
            }

            Integral_Name = Name_CrossKey.Concat(Name_Button).ToDictionary(x => x.Key, x => x.Value); //Name_Button��Integral_Name�𓝍�����
        }

        /// <summary>
        /// �ݒ�L�[�ɑΉ�����o�̓L�[��Ԃ��B
        /// </summary>
        /// <param name="key">�o�͂�v������ݒ�L�[</param>
        /// <returns>�Ή�����o�̓L�\</returns>
        public char GetOutputWord(string key)
        {
            return Integral_Name[key];
        }
        /// <summary>
        /// �o�̓L�[�ɑΉ�����ݒ�L�[��Ԃ��B
        /// </summary>
        /// <param name="key">�o�͂�v������o�̓L�[</param>
        /// <returns>�Ή�����ݒ�L�\</returns>
        public string GetButton(char outputWord)
        {
            return Name_Button.FirstOrDefault(el => el.Value == outputWord).Key;
        }
        /// <summary>
        /// �S�Ă̏o�̓L�[�\��Ԃ�
        /// </summary>
        /// <returns>�S�Ă̏o�̓L�[�\</returns>
        public List<char> GetAllOutputWordList()
        {
            return Integral_Name.Values.ToList();
        }
        /// <summary>
        /// �\���L�[�ȊO�̏o�̓L�[��Ԃ�
        /// </summary>
        /// <returns>�\���L�[�ȊO�̏o�̓L�[��Ԃ�</returns>
        public List<char> GetOutputWordButtonList()
        {
            return Name_Button.Values.ToList();
        }
        /// <summary>
        /// �\���L�[�̏o�͕\��Ԃ�
        /// </summary>
        /// <returns>�\���L�[�̏o�͕\</returns>
        public List<char> GetOutputWordCrossKeyList()
        {
            return Name_CrossKey.Values.ToList();
        }


        /// <summary>
        /// Inspect���͗p�̃N���X���N���X
        /// </summary>
        [System.Serializable]
        public class Key
        {
            public string button;   //�ݒ�L�[������
            public char outputWord; //�o�̓L�[����

            /// <summary>
            /// �o�̓L�[������Ԃ�
            /// </summary>
            /// <returns></returns>
            public char GetOutputWord()
            {
                return outputWord;
            }
            /// <summary>
            /// �ݒ�L�[�������Ԃ�
            /// </summary>
            /// <returns></returns>
            public string GetButton()
            {
                return button;
            }
        }
    }
}