using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


namespace InputCommand
{
    //
    // このソースファイルでは
    // OneFrameButton_Base(コマンド表のデータか)とOneFrameButton_Input(入力値)を定義する。
    // 

    /// <summary>
    /// 1フレームのコマンドデータを表すクラス
    /// </summary>
    public class OneFrameButton_Base
    {
        private  KeyList keyList;   // 本ゲームで使用されるキーの対応リスト(例)"Up"⇒"8"　コンストラクタでセットされる
        public Dictionary<char, ButtonState> buttonStates = new Dictionary<char, ButtonState>(); //1フレーム当たりのデータ
        public int cycle; //比較するフレーム数。(溜めが60フレームならここに60Fと入る。標準は1)

        /// <summary>
        /// コンストラクタ
        /// キーリストをセットする。
        /// </summary>
        /// <param name="keyList">使用されるキーのリスト</param>
        public OneFrameButton_Base(KeyList keyList)
        {
            this.keyList = keyList;
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            {
                buttonStates[tartgetchar] = ButtonState.None;
            }
        }

        /// <summary>
        /// サイクル回数分、キー比較を行う
        /// </summary>
        /// <param name="anotherFrameButtons">比較対象</param>
        /// <returns>比較の合否</returns>
        /// 
        public bool CompareToInstance(List<OneFrameButton_Input> anotherFrameButtons)
        {
            for (int i = 0; i < cycle; i++)
            {
                if (!CompareToInstance(anotherFrameButtons[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// キーリストを元に、比較を行う
        /// </summary>
        /// <param name="anotherFrameButton">比較対象</param>
        /// <returns>比較の合否</returns> 
        private bool CompareToInstance(OneFrameButton_Input anotherFrameButton) 
        { 
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            //全キーを比較する
            {
                if (buttonStates[tartgetchar] == ButtonState.None)
                //評価無効なら評価継続
                {
                    continue;
                }
                if (buttonStates[tartgetchar] != anotherFrameButton.buttonStates[tartgetchar])
                //比較し異なる用であれば偽を返す
                {
                    return false;
                }
            }
            return true;//全てが評価外か比較し合格なら真を返す
        }

        /// <summary>
        /// 文字列を解釈してコマンドデータに変換する
        /// </summary>
        /// <param name="command">解釈するコマンド文字列 (例)「.2.[^6]...｣｢.2.6...｣｢.[^2].6...｣｢....A..｣ </param>
        /// <param name="cycle"> 何フレーム前まで確認するか </param>
        public void SetCommand(string command, int cycle)
        {
            foreach (char tempSign in keyList.GetAllOutputWordList())
                //キーリストに従いループする
            {
                int match = command.IndexOf(tempSign); //含む個所を調べる
                if (match >= 0)
                    //含む場合
                {
                    if (match > 0)
                    {
                        if (command[match - 1] == '^')
                            //否定として使われている場合
                        {
                            buttonStates[tempSign] = ButtonState.Off ;
                        }
                        else
                            //肯定として使われている場合
                        {
                            buttonStates[tempSign] = ButtonState.On;
                        }
                    }
                    else
                    //一文字目の場合は肯定とする
                    {
                        buttonStates[tempSign] = ButtonState.On;
                    }
                }
                else
                    //含まない場合(-1)が返っている。
                {
                    buttonStates[tempSign] = ButtonState.None;
                }
            }
            this.cycle = cycle;
        }
        /// <summary>
        /// 直接キーを追加する場合(ダミーデータ作成を想定)
        /// </summary>
        /// <param name="key">追加するキー　(キーリスト参照)</param>
        /// <param name="state">追加するステート</param>

        public void SetCommandElement(char key, ButtonState state)
        {
            buttonStates[key] = state;
        }

        /// <summary>
        /// Debug用の文字列作成
        /// </summary>
        /// <returns>このコマンドの文字列表現 (例)「?2?-???｣｢?2?6???｣｢?-?6???｣????A??｣</returns>
        public string GetString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            {
                sb.Append(buttonStates[tartgetchar] == ButtonState.On ? tartgetchar + "" : buttonStates[tartgetchar] == ButtonState.Off ? "-" : "?");
            }
            return sb.ToString();
        }

    }

    /// <summary>
    ///  1フレームのインプットデータを表すクラス
    /// </summary>
    public class OneFrameButton_Input
    {
        public KeyList keyList;//キーリスト
        public Dictionary<char, ButtonState> buttonStates = new Dictionary<char, ButtonState>(); //1フレーム当たりのデータ

        /// <summary>
        /// コンストラクタ
        /// キーリストをセットする。
        /// </summary>
        /// <param name="keyList">使用されるキーのリスト</param>
        public OneFrameButton_Input(KeyList keyList)
        {
            this.keyList = keyList;
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            {
                buttonStates[tartgetchar] = ButtonState.Off;
            }
        }

        /// <summary>
        /// 上入力状態の確認
        /// </summary>
        /// <param name="state">入力状態</param>
        public void SetStateUpInFixedUpdate(ButtonState state)
        {
            buttonStates['8'] = state;
        }
        /// <summary>
        /// 下入力状態の確認
        /// </summary>
        /// <param name="state">入力状態</param>
        public void SetStateDownInFixedUpdate(ButtonState state)
        {
            buttonStates['2'] = state;
        }
        /// <summary>
        /// 左入力状態の確認
        /// </summary>
        /// <param name="state">入力状態</param>
        public void SetStateLeftInFixedUpdate(ButtonState state)
        {
            buttonStates['4'] = state;
        }
        /// <summary>
        /// 右入力状態の確認
        /// </summary>
        /// <param name="state">入力状態</param>
        public void SetStateRightInFixedUpdate(ButtonState state)
        {
            buttonStates['6'] = state;
        }

        /// <summary>
        /// 指定したキーの入力状態を設定する
        /// </summary>
        /// <param name="sign">キーの文字 (キーリストを参照)</param>
        /// <param name="state">入力状態</param>
        public void SetstateButton(char sign , ButtonState state)
            //指定値
        {
            buttonStates[sign] = state;
        }


        public void Reset()
        {
            foreach(char key in keyList.GetAllOutputWordList())
            {
                buttonStates[key] = ButtonState.Off;
            }
        }

        /// <summary>
        /// Debug用の文字列作成
        /// </summary>
        /// <returns>現在のコマンドを示す文字列表現 (例)-2??A??(しゃがみA)</returns>

        public string GetString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            {
                sb.Append(buttonStates[tartgetchar] == ButtonState.On ? tartgetchar + "": buttonStates[tartgetchar] == ButtonState.Off? "-" : "?" ) ;
            }
            return sb.ToString();
        }
    }
}
