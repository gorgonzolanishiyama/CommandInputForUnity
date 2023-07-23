using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace InputCommand
{
    //UnityのPrefaranceSetteing設定キーとコマンドに使用する出力キーの対応表
    //ScriptableObject
    //
    [CreateAssetMenu(fileName = "ButtonList", menuName = "ScriptableObjects/ButtonNameSet", order = 1)]
    public class KeyList : ScriptableObject
    {

        public int keysCount = 0;                   //　Inspecterで使用する。　入力リスト数
        public List<Key> keys = new List<Key>();    //　Inspecterで使用する。　入力するキーの対応要素
                                                    //デフォルト十字キーの設定キー出力キ―対応表
        public Dictionary<string, char> Name_CrossKey = new Dictionary<string, char>();
        //十字キー以外のボタンの設定キー出力キ―対応表
        Dictionary<string, char> Name_Button = new Dictionary<string, char>();

        //Name_CrossKeyとName_Buttonの設定キー出力キ―-統合表
        Dictionary<string, char> Integral_Name = new Dictionary<string, char>();

        /// <summary>
        /// インスペクタ起動時
        /// keys リストから Name_Button を作成し、Name_CrossKey と統合して Integral_Name を生成します。
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

            ////入力されているkeysからName_Buttonを作成する。
            foreach (Key tempkey in keys)
            //List<Key>をDictionary<char,string>形式に変換する。
            {
                Name_Button[tempkey.GetButton()] = tempkey.GetOutputWord();
            }

            Integral_Name = Name_CrossKey.Concat(Name_Button).ToDictionary(x => x.Key, x => x.Value); //Name_ButtonとIntegral_Nameを統合する
        }

        /// <summary>
        /// 設定キーに対応する出力キーを返す。
        /// </summary>
        /// <param name="key">出力を要求する設定キー</param>
        /// <returns>対応する出力キ―</returns>
        public char GetOutputWord(string key)
        {
            return Integral_Name[key];
        }
        /// <summary>
        /// 出力キーに対応する設定キーを返す。
        /// </summary>
        /// <param name="key">出力を要求する出力キー</param>
        /// <returns>対応する設定キ―</returns>
        public string GetButton(char outputWord)
        {
            return Name_Button.FirstOrDefault(el => el.Value == outputWord).Key;
        }
        /// <summary>
        /// 全ての出力キー表を返す
        /// </summary>
        /// <returns>全ての出力キー表</returns>
        public List<char> GetAllOutputWordList()
        {
            return Integral_Name.Values.ToList();
        }
        /// <summary>
        /// 十字キー以外の出力キーを返す
        /// </summary>
        /// <returns>十字キー以外の出力キーを返す</returns>
        public List<char> GetOutputWordButtonList()
        {
            return Name_Button.Values.ToList();
        }
        /// <summary>
        /// 十字キーの出力表を返す
        /// </summary>
        /// <returns>十字キーの出力表</returns>
        public List<char> GetOutputWordCrossKeyList()
        {
            return Name_CrossKey.Values.ToList();
        }


        /// <summary>
        /// Inspect入力用のクラス内クラス
        /// </summary>
        [System.Serializable]
        public class Key
        {
            public string button;   //設定キー文字列
            public char outputWord; //出力キー文字

            /// <summary>
            /// 出力キー文字を返す
            /// </summary>
            /// <returns></returns>
            public char GetOutputWord()
            {
                return outputWord;
            }
            /// <summary>
            /// 設定キー文字列を返す
            /// </summary>
            /// <returns></returns>
            public string GetButton()
            {
                return button;
            }
        }
    }
}