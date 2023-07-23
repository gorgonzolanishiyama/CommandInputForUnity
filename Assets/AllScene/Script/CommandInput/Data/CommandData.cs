using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputCommand
{
    /// <summary>
    /// コマンド入力用クラス内クラス
    /// </summary>
    [System.Serializable]
    public class CommandData
    {
        public string outputWord;                 //出力文字
        public int inputAcceptanceFrame;          //入力猶予時間
        public string commandRegEXs;              //コマンド(正規表現)

        /// <summary>
        /// アクセサー 出力 出力文字
        /// </summary>
        /// <returns>出力文字</returns>
        public string GetOutputWord()
        {
            return outputWord;
        }

        /// <summary>
        /// アクセサー 出力 入力猶予時間
        /// </summary>
        /// <returns>入力猶予時間</returns>
        public string GetCommandRegEXs()
        {
            return commandRegEXs;
        }

        /// <summary>
        /// アクセサー 出力 コマンド(正規表現)
        /// </summary>
        /// <returns>コマンド(正規表現)</returns>
        public int GetInputtAcceptanceFrame()
        {
            return inputAcceptanceFrame;
        }
    }
}
