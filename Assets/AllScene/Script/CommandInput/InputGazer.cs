using InputCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Build;
using UnityEngine;


namespace InputCommand
{
    /// <summary>
    /// 入力キー確認用コンポーネント
    /// 常時キー入力を監視してイベント先に受け渡す
    /// </summary>

    public class InputGazer : MonoBehaviour
    {
        public KeyList keyList;                                                //キーリスト　　　　　手動で代入する

        private HashSet<Action<List<OneFrameButton_Input>>> keyObserves = new HashSet<Action<List<OneFrameButton_Input>>>() ;              //イベント重複登録防止用

        public List<OneFrameButton_Input> commandPerFrame = new List<OneFrameButton_Input>() ;       //入力キー確認表示用リスト

        public List<string> commandPefFrameForDebug = new List<string>() ;
        public int count = 0;                                            //デバッグ時の有効なイベントハンドラ数。
        public int commandPerFrameCount ;

        void Awake()
        {

            commandPerFrame = new List<OneFrameButton_Input>(new OneFrameButton_Input[commandPerFrameCount]);
            commandPefFrameForDebug = new List<string>(new string[commandPerFrameCount]);
        }

        /// <summary>
        /// 固定時間アップデートにて入力キーを確認する
        /// </summary>
        void FixedUpdate()
        {
            OneFrameButton_Input oneFrameButton_Input;
            oneFrameButton_Input = new OneFrameButton_Input(keyList);
            //十字キー確認
            if (Input.GetAxisRaw("Vertical") > 0) oneFrameButton_Input.SetStateUpInFixedUpdate(ButtonState.On);
            if (Input.GetAxisRaw("Vertical") < 0) oneFrameButton_Input.SetStateDownInFixedUpdate(ButtonState.On);
            if (Input.GetAxisRaw("Horizontal") < 0) oneFrameButton_Input.SetStateLeftInFixedUpdate(ButtonState.On);
            if (Input.GetAxisRaw("Horizontal") > 0) oneFrameButton_Input.SetStateRightInFixedUpdate(ButtonState.On);
            //十字キー以外の入力キー確認
            foreach (char tempKey in keyList.GetOutputWordButtonList())
            {
                if (Input.GetButton(keyList.GetButton(tempKey))) oneFrameButton_Input.SetstateButton(tempKey, ButtonState.On);
            }

            //入力キープール更新
            commandPerFrame.Insert(0, oneFrameButton_Input);
            commandPerFrame.RemoveAt(commandPerFrame.Count - 1);

            //デバッグ用の入力キー確認表示用リスト更新
            commandPefFrameForDebug.Insert(0, oneFrameButton_Input.GetString());
            commandPefFrameForDebug.RemoveAt(commandPefFrameForDebug.Count - 1);
            
            foreach (var keyObserve in keyObserves)
            {
                keyObserve.Invoke(commandPerFrame);
            }
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <param name="command">登録するイベント</param>
        public void SetCommand( CharacterCommand command)
        {
            keyObserves.Add(command.inputSetEvent);
            count = keyObserves.Count;
        }

        /// <summary>
        /// イベント破棄
        /// </summary>
        /// <param name="command">破棄するイベント</param>
        public void RemoveCommand( CharacterCommand command)
        {
            keyObserves.Remove(command.inputSetEvent);
            count = keyObserves.Count;
        }

        public List<OneFrameButton_Input> GetCommandList()
        {
            return commandPerFrame;
        }
    }
}