using InputCommand;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using static InputCommand.CharacterCommand;

namespace InputCommand
{
    /// <summary>
    /// 入力コマンド確認コンポーネント
    /// 
    /// キャラクターにアタッチすることを想定
    /// 
    /// </summary>
    public class CharacterCommand : MonoBehaviour
    {
        public InputGazer inputGazer;                                               //リアルタイム入力監視クラス　　　手動で代入する
        public CommandListStore commandListStore;                                   //対象コマンドリスト　手動で代入する
        public KeyList keyList;                                                     //キーリスト　　　　　手動で代入する

        private List<CommandForSkill> commands = new List<CommandForSkill>();       //コマンドリストを格納したもの
        private HashSet<CommandForSkill> commandsChecking = new HashSet<CommandForSkill>(); //今監視しているコマンド
        public List<string> ForDebug = new List<string>();                          //デバッグ用　インスペクタ表示用
        public int ForDebugEvenetNum;                                               //デバッグ用　インスペクタ(現在反応しているイベント表示用)


        /// <summary>
        /// 初期設定
        /// </summary>
        void Start()
        {
            foreach (Command command in commandListStore.GetCommandList())
                //対象コマンドリスト内の全てのコマンドを使い易いように変換する。
            {
                CommandForSkill commandForSkill = new CommandForSkill();
                commandForSkill.SetOutputWord( command.GetOutputWord() );
                commandForSkill.SetOneFrameButton_Bases( command.CreateCommandData(keyList) );


                commandForSkill.SetCharacterCommand(this);                //テスト用にこのクラスをcommandForSkillに登録(commandForSkillからActiveByCommandを実行させるため)

                commands.Add(commandForSkill);

                //以下はデバッグ表示用
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < commandForSkill.GetOneFrameButton_Bases().Count; i++)
                {
                    stringBuilder.Append(commandForSkill.GetOneFrameButton_Bases()[i].GetString());
                    stringBuilder.Append(' ');
                }
                ForDebug.Add(stringBuilder.ToString());
            }

            inputGazer.SetCommand(this);    //InputGazerにイベントを登録する。
        }

        /// <summary>
        /// 破棄時
        /// </summary>
        private void OnDestroy()
        {
            inputGazer.RemoveCommand(this); //破棄された時　InputGazerからも破棄する。
        }

        /// <summary>
        /// フレームごとにインプットを受け取る。
        /// イベント
        /// </summary>
        /// <param name="oneFrameButtons_Input">インプットされたているコマンド</param>
        public void inputSetEvent(List<OneFrameButton_Input> oneFrameButtons_Input)
        {
            foreach (CommandForSkill commandForSkill in commands)
                //監視コマンドを追加するかどうか
            {
                if (commandForSkill.GetFirstFrameButton_Base().CompareToInstance(oneFrameButtons_Input))
                    //第一コマンドがインプットコマンドと等しければ追加する
                {
                    commandsChecking.Add(commandForSkill);
                }
            }
            HashSet<CommandForSkill> newCommandsCheckings = new HashSet<CommandForSkill>();            //イベントアップデート判定
            foreach (CommandForSkill commandForSkill in commandsChecking)
                //監視しているコマンドを確認するループ。
            {
                if (commandForSkill.EvarateUpdate_IsUpdateContinue(oneFrameButtons_Input))
                    //アップデートを継続する場合は真
                {
                    newCommandsCheckings.Add(commandForSkill);
                }
            }
            commandsChecking = newCommandsCheckings;

            ForDebugEvenetNum = commandsChecking.Count;
        }

        /// <summary>
        /// テスト用キー出力結果ファイル。
        /// (CharacterCommand内に書く必要なかったかも)
        /// </summary>
        /// <param name="outputWord"></param>

        public void ActionByCommand(string outputWord)
        {
            BroadcastMessage("ChangeText", outputWord);
        }


        /// <summary>
        /// コマンド処理用のクラス内クラス
        /// </summary>
        [System.Serializable]
        public class CommandForSkill
        {
            private string outputWord;                                        //成立時に出力される文字列
            private List<OneFrameButton_Base> oneFrameButton_Bases;     //コマンドリスト
            private int inputAcceptanceTimeFrame = 30;                            //インプット時間
            private int countTimeFrame = 0;                             //入力猶予時間
            private int countCommand = 0;                               //コマンドの走査位置

            public CharacterCommand parent;                                //テスト用　動作確認が成功したときにCharacterCommandの.ActionByCommandを呼び出す。

            /// <summary>
            /// アクセサーoutputWord
            /// </summary>
            /// <param name="outputWord"></param>
            public void SetOutputWord(string outputWord)
            {
                this.outputWord = outputWord;
            }
            /// <summary>
            /// アクセサーOneFrameButton_Bases
            /// </summary>
            /// <param name="oneFrameButton_Bases"></param>
            public void SetOneFrameButton_Bases( List<OneFrameButton_Base> oneFrameButton_Bases )
            {
                this.oneFrameButton_Bases = oneFrameButton_Bases;
            }

            /// <summary>
            /// アクセサー 出力GetOneFrameButton_Bases
            /// </summary>
            /// <returns></returns>

            public List<OneFrameButton_Base> GetOneFrameButton_Bases()
            {
                return oneFrameButton_Bases;
            }
            /// <summary>
            /// 一番最初のコマンドを出力
            /// </summary>
            /// <returns>OneFrameButton_Basesの最初のコマンド</returns>
            public OneFrameButton_Base GetFirstFrameButton_Base()
            {
                return oneFrameButton_Bases[0];
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Init()
            {
                countTimeFrame = 0;
                countCommand = 0;
            }
            /// <summary>
            /// コマンドと入力コマンドの比較
            /// 評価継続可否を返す
            /// </summary>
            /// <param name="oneFrameButton_Input"></param>
            /// <returns>評価継続可否</returns>
            public bool EvarateUpdate_IsUpdateContinue(List<OneFrameButton_Input> oneFrameButtons_Input)
            {
                countTimeFrame++;
                if (countTimeFrame > inputAcceptanceTimeFrame)
                {
                    Init();
                    return false;
                }
                if (oneFrameButton_Bases[countCommand].CompareToInstance(oneFrameButtons_Input))
                {
                    countCommand++;
                    if (countCommand > oneFrameButton_Bases.Count - 1)
                    {
                        EvarateAllMatch();
                        return false;
                    }
                }
                return true;
            }
            /// <summary>
            /// 成立した際の振る舞い
            /// </summary>
            public void EvarateAllMatch()
            {
                Debug.Log(outputWord);

                this.parent.ActionByCommand(outputWord);


                Init();
            }

            /// <summary>
            /// テスト用
            /// </summary>
            /// <param name="parent">呼び出したクラス</param>
            public void SetCharacterCommand( CharacterCommand parent)
            {
                this.parent = parent;
            }
        }
    }
}