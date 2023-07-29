using InputCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

namespace InputCommand
{
    /// <summary>
    /// コマンドの外部からのデータ
    /// 
    /// 外部からのデータに相当する
    /// 
    /// </summary>
    public class Command
    {
        string outputWord;             //コマンドが成立したときにアウトプットされるstring
        int inputAcceptanceFrame;          //インプット許容範囲
        string commandRegEX;     //正規表現を加味した文字列 これが後にフレームごとに分割される。


        Dictionary<string, OneFrameButton_Base> commandCache = new Dictionary<string, OneFrameButton_Base>(); //コマンドのキャッシュ
        //  commandRegEXのルール
        //  最初 "^" 最後"$" ([コマンド]){フレーム数,}
        //
        //　commandRegEXの例
        //  26A(ﾊﾄﾞｰｹﾝ!)の場合
        // ^(\\[(.2.[^6]...)\\]){1,}.*(\\[(.2.6...)\\]){1,}.*(\\[(.[^2].6...)\\]){1,}.*(\\[(....A..)\\]){1,}.*$
        //

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputWord"></param>
        /// <param name="inputFrame"></param>
        /// <param name="commandRegEX"></param>
        public Command(string outputWord, int inputFrame, string commandRegEX)
        {
            this.outputWord = outputWord;
            this.inputAcceptanceFrame = inputFrame;
            this.commandRegEX = commandRegEX;
        }
        /// <summary>
        /// アクセサ　出力 outputWprd
        /// </summary>
        /// <returns></returns>
        public string GetOutputWord()
        {
            return this.outputWord;
        }
        /// <summary>
        /// アクセサ 出力 inputAcceptanceFrame
        /// </summary>
        /// <returns></returns>
        public int GetInputAcceptanceFrame()
        {
            return this.inputAcceptanceFrame;
        }
        /// <summary>
        /// アクセサ 出力 CommandRegEx
        /// </summary>
        /// <returns></returns>
        public string GetCommandRegEX()
        {
            return this.commandRegEX;
        }

        /// <summary>
        /// コマンド文字列をキーリストに従いながら1フレームごとのデータに分割する。
        /// </summary>
        /// <param name="keyList">キーリストに従いボタンを確認する</param>
        /// <returns>変換したコマンド</returns>
        public List<OneFrameButton_Base> CreateCommandData(KeyList keyList)
        {
            List<OneFrameButton_Base> buttonBases = new List<OneFrameButton_Base>();
            string[] CommandPart = commandRegEX.Replace("}.*(", "}@{").Split('@'); //各フレームで分割している。
            Regex regex = new Regex(@".*\[(?<command>.*)\].*{(?<cycle>[0-9]*).*}.*");

            foreach (string partCommand in CommandPart)
            //各フレームごとに走査
            {
                try
                {
                    Match match = regex.Match(partCommand);
                    if (match.Success)
                    {
                        int cycleMatched = int.Parse(match.Groups["cycle"].Value);  //繰り返し数取得
                        string commandMatched = match.Groups["command"].Value;      //コマンドの作成

                        if (!commandCache.TryGetValue(commandMatched, out OneFrameButton_Base command))
                        //同じ文字が出たらキャッシュを返す
                        {
                            command = new OneFrameButton_Base(keyList);
                            command.SetCommand(commandMatched, cycleMatched);
                            commandCache.Add(commandMatched, command);
                        }
                        buttonBases.Add(command);
/*

                        for (int i = 0; i < cycleMatched; i++)
                        //繰り返し数反映
                        {
                            buttonBases.Add(command);
                        }
*/
                    }
                }
                catch (Exception ex)
                {
                    // 例外発生時の処理
                    Debug.WriteLine("Error in CreateCommandData! Dummy Data used currentry: " + ex.Message);
                    Debug.WriteLine(ex);
                    //DummuyDataデータを使用します。
                    OneFrameButton_Base command = new OneFrameButton_Base(keyList);
                    command = CreateDummyCommand(keyList);
                    buttonBases.Add(command);
                    continue;
                }
            }
            return buttonBases;
        }

        private OneFrameButton_Base CreateDummyCommand(KeyList keyList)
        {
            // 仮のダミーデータの作成
            OneFrameButton_Base dummyCommand = new OneFrameButton_Base(keyList);
            // 仮のデータをセットするなどの処理を行う
            foreach (char key in keyList.GetAllOutputWordList())
            {
                dummyCommand.SetCommandElement(key, ButtonState.None);
            }
            return dummyCommand;
        }
    }
}