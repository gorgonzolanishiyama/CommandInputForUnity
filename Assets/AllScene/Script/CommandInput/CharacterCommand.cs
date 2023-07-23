using InputCommand;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace InputCommand
{
    /// <summary>
    /// ���̓R�}���h�m�F�R���|�[�l���g
    /// 
    /// �L�����N�^�[�ɃA�^�b�`���邱�Ƃ�z��
    /// 
    /// </summary>
    public class CharacterCommand : MonoBehaviour
    {
        public InputGazer inputGazer;                                               //�C���v�b�g���e�m�F�@�蓮�ő������
        public CommandListStore commandListStore;                                   //�ΏۃR�}���h���X�g�@�蓮�ő������
        public KeyList keyList;                                                     //�L�[���X�g�@�@�@�@�@�蓮�ő������

        private List<CommandForSkill> commands = new List<CommandForSkill>();       //�R�}���h���X�g���i�[��������
        private HashSet<CommandForSkill> commandsChecking = new HashSet<CommandForSkill>(); //���Ď����Ă���R�}���h
        public List<string> ForDebug = new List<string>();                          //�f�o�b�O�p�@�C���X�y�N�^�\���p

        /// <summary>
        /// �����ݒ�
        /// </summary>
        void Start()
        {
            foreach (Command command in commandListStore.GetCommandList())
                //�ΏۃR�}���h���X�g���̑S�ẴR�}���h���g���Ղ��悤�ɕϊ�����B
            {
                CommandForSkill commandForSkill = new CommandForSkill();
                commandForSkill.SetOutputWord( command.GetOutputWord() );
                commandForSkill.SetOneFrameButton_Bases( command.CreateCommandData(keyList) );
                commands.Add(commandForSkill);

                //�ȉ��̓f�o�b�O�\���p
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < commandForSkill.GetOneFrameButton_Bases().Count; i++)
                {
                    stringBuilder.Append(commandForSkill.GetOneFrameButton_Bases()[i].GetString());
                    stringBuilder.Append(' ');
                }
                ForDebug.Add(stringBuilder.ToString());
            }

            inputGazer.SetCommand(this);    //InputGazer�ɃC�x���g��o�^����B
        }

        /// <summary>
        /// �j����
        /// </summary>
        private void OnDestroy()
        {
            inputGazer.RemoveCommand(this); //�j�����ꂽ���@InputGazer������j������B
        }

        /// <summary>
        /// �t���[�����ƂɃC���v�b�g���󂯎��B
        /// �C�x���g
        /// </summary>
        /// <param name="oneFrameButton_Input">�C���v�b�g���ꂽ�R�}���h(1�t���[����)</param>
        public void inputSetEvent(OneFrameButton_Input oneFrameButton_Input)
        {
            foreach (CommandForSkill commandForSkill in commands)
                //�Ď��R�}���h��ǉ����邩�ǂ���
            {
                if (commandForSkill.GetFirstFrameButton_Base().CompareToInstance(oneFrameButton_Input))
                    //���R�}���h���C���v�b�g�R�}���h�Ɠ�������Βǉ�����
                {
                    commandsChecking.Add(commandForSkill);
                }
            }
            HashSet<CommandForSkill> newCommandsCheckings = new HashSet<CommandForSkill>();            //�C�x���g�A�b�v�f�[�g����
            foreach (CommandForSkill commandForSkill in commandsChecking)
                //�Ď�����
            {
                if (commandForSkill.EvarateUpdate_IsUpdateContinue(oneFrameButton_Input))
                    //�A�b�v�f�[�g���p������ꍇ�͐^
                {
                    newCommandsCheckings.Add(commandForSkill);
                }
            }
            commandsChecking = newCommandsCheckings;


        }


        /// <summary>
        /// �R�}���h�����p�̃N���X���N���X
        /// </summary>
        [System.Serializable]
        public class CommandForSkill
        {
            private string outputWord;                                        //�������ɏo�͂���镶����
            private List<OneFrameButton_Base> oneFrameButton_Bases;     //�R�}���h���X�g
            private int inputAcceptanceTimeFrame = 30;                            //�C���v�b�g����
            private int countTimeFrame = 0;                             //���͗P�\����
            private int countCommand = 0;                               //�R�}���h�̑����ʒu

            /// <summary>
            /// �A�N�Z�T�[outputWord
            /// </summary>
            /// <param name="outputWord"></param>
            public void SetOutputWord(string outputWord)
            {
                this.outputWord = outputWord;
            }
            /// <summary>
            /// �A�N�Z�T�[OneFrameButton_Bases
            /// </summary>
            /// <param name="oneFrameButton_Bases"></param>
            public void SetOneFrameButton_Bases( List<OneFrameButton_Base> oneFrameButton_Bases )
            {
                this.oneFrameButton_Bases = oneFrameButton_Bases;
            }
            /// <summary>
            /// �A�N�Z�T�[InputTimeFrame
            /// </summary>
            /// <param name="inputAcceptanceTimeFrame"></param>
            public void SetInputAcceptanceTimeFrame(int inputAcceptanceTimeFrame )
            {
                this.inputAcceptanceTimeFrame = inputAcceptanceTimeFrame;
            }
            /// <summary>
            /// �A�N�Z�T�[ �o��GetOneFrameButton_Bases
            /// </summary>
            /// <returns></returns>

            public List<OneFrameButton_Base> GetOneFrameButton_Bases()
            {
                return oneFrameButton_Bases;
            }
            /// <summary>
            /// ��ԍŏ��̃R�}���h���o��
            /// </summary>
            /// <returns>OneFrameButton_Bases�̍ŏ��̃R�}���h</returns>
            public OneFrameButton_Base GetFirstFrameButton_Base()
            {
                return oneFrameButton_Bases[0];
            }

            /// <summary>
            /// ������
            /// </summary>
            public void Init()
            {
                countTimeFrame = 0;
                countCommand = 0;
            }
            /// <summary>
            /// �R�}���h�Ɠ��̓R�}���h�̔�r
            /// �]���p���ۂ�Ԃ�
            /// </summary>
            /// <param name="oneFrameButton_Input"></param>
            /// <returns>�]���p����</returns>
            public bool EvarateUpdate_IsUpdateContinue(OneFrameButton_Input oneFrameButton_Input)
            {
                countTimeFrame++;
                if (countTimeFrame > inputAcceptanceTimeFrame)
                {
                    Init();
                    return false;
                }
                if (oneFrameButton_Bases[countCommand].CompareToInstance(oneFrameButton_Input))
                {
                    countCommand++;
                    if (countCommand > oneFrameButton_Bases.Count - 1)
                    {
                        EvarateAllMatch();
                        return true;
                    }
                }
                return true;
            }
            /// <summary>
            /// ���������ۂ̍s��
            /// </summary>
            public void EvarateAllMatch()
            {
                Debug.Log(outputWord);
                Init();
            }
        }
    }
}