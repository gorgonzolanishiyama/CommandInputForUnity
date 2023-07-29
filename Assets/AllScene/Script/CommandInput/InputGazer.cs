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
    /// ���̓L�[�m�F�p�R���|�[�l���g
    /// �펞�L�[���͂��Ď����ăC�x���g��Ɏ󂯓n��
    /// </summary>

    public class InputGazer : MonoBehaviour
    {
        public KeyList keyList;                                                //�L�[���X�g�@�@�@�@�@�蓮�ő������

        private HashSet<Action<List<OneFrameButton_Input>>> keyObserves = new HashSet<Action<List<OneFrameButton_Input>>>() ;              //�C�x���g�d���o�^�h�~�p

        public List<OneFrameButton_Input> commandPerFrame = new List<OneFrameButton_Input>() ;       //���̓L�[�m�F�\���p���X�g

        public List<string> commandPefFrameForDebug = new List<string>() ;
        public int count = 0;                                            //�f�o�b�O���̗L���ȃC�x���g�n���h�����B
        public int commandPerFrameCount ;

        void Awake()
        {

            commandPerFrame = new List<OneFrameButton_Input>(new OneFrameButton_Input[commandPerFrameCount]);
            commandPefFrameForDebug = new List<string>(new string[commandPerFrameCount]);
        }

        /// <summary>
        /// �Œ莞�ԃA�b�v�f�[�g�ɂē��̓L�[���m�F����
        /// </summary>
        void FixedUpdate()
        {
            OneFrameButton_Input oneFrameButton_Input;
            oneFrameButton_Input = new OneFrameButton_Input(keyList);
            //�\���L�[�m�F
            if (Input.GetAxisRaw("Vertical") > 0) oneFrameButton_Input.SetStateUpInFixedUpdate(ButtonState.On);
            if (Input.GetAxisRaw("Vertical") < 0) oneFrameButton_Input.SetStateDownInFixedUpdate(ButtonState.On);
            if (Input.GetAxisRaw("Horizontal") < 0) oneFrameButton_Input.SetStateLeftInFixedUpdate(ButtonState.On);
            if (Input.GetAxisRaw("Horizontal") > 0) oneFrameButton_Input.SetStateRightInFixedUpdate(ButtonState.On);
            //�\���L�[�ȊO�̓��̓L�[�m�F
            foreach (char tempKey in keyList.GetOutputWordButtonList())
            {
                if (Input.GetButton(keyList.GetButton(tempKey))) oneFrameButton_Input.SetstateButton(tempKey, ButtonState.On);
            }

            //���̓L�[�v�[���X�V
            commandPerFrame.Insert(0, oneFrameButton_Input);
            commandPerFrame.RemoveAt(commandPerFrame.Count - 1);

            //�f�o�b�O�p�̓��̓L�[�m�F�\���p���X�g�X�V
            commandPefFrameForDebug.Insert(0, oneFrameButton_Input.GetString());
            commandPefFrameForDebug.RemoveAt(commandPefFrameForDebug.Count - 1);
            
            foreach (var keyObserve in keyObserves)
            {
                keyObserve.Invoke(commandPerFrame);
            }
        }

        /// <summary>
        /// �C�x���g�o�^
        /// </summary>
        /// <param name="command">�o�^����C�x���g</param>
        public void SetCommand( CharacterCommand command)
        {
            keyObserves.Add(command.inputSetEvent);
            count = keyObserves.Count;
        }

        /// <summary>
        /// �C�x���g�j��
        /// </summary>
        /// <param name="command">�j������C�x���g</param>
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