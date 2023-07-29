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
    /// �R�}���h�̊O������̃f�[�^
    /// 
    /// �O������̃f�[�^�ɑ�������
    /// 
    /// </summary>
    public class Command
    {
        string outputWord;             //�R�}���h�����������Ƃ��ɃA�E�g�v�b�g�����string
        int inputAcceptanceFrame;          //�C���v�b�g���e�͈�
        string commandRegEX;     //���K�\������������������ ���ꂪ��Ƀt���[�����Ƃɕ��������B


        Dictionary<string, OneFrameButton_Base> commandCache = new Dictionary<string, OneFrameButton_Base>(); //�R�}���h�̃L���b�V��
        //  commandRegEX�̃��[��
        //  �ŏ� "^" �Ō�"$" ([�R�}���h]){�t���[����,}
        //
        //�@commandRegEX�̗�
        //  26A(��ް��!)�̏ꍇ
        // ^(\\[(.2.[^6]...)\\]){1,}.*(\\[(.2.6...)\\]){1,}.*(\\[(.[^2].6...)\\]){1,}.*(\\[(....A..)\\]){1,}.*$
        //

        /// <summary>
        /// �R���X�g���N�^
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
        /// �A�N�Z�T�@�o�� outputWprd
        /// </summary>
        /// <returns></returns>
        public string GetOutputWord()
        {
            return this.outputWord;
        }
        /// <summary>
        /// �A�N�Z�T �o�� inputAcceptanceFrame
        /// </summary>
        /// <returns></returns>
        public int GetInputAcceptanceFrame()
        {
            return this.inputAcceptanceFrame;
        }
        /// <summary>
        /// �A�N�Z�T �o�� CommandRegEx
        /// </summary>
        /// <returns></returns>
        public string GetCommandRegEX()
        {
            return this.commandRegEX;
        }

        /// <summary>
        /// �R�}���h��������L�[���X�g�ɏ]���Ȃ���1�t���[�����Ƃ̃f�[�^�ɕ�������B
        /// </summary>
        /// <param name="keyList">�L�[���X�g�ɏ]���{�^�����m�F����</param>
        /// <returns>�ϊ������R�}���h</returns>
        public List<OneFrameButton_Base> CreateCommandData(KeyList keyList)
        {
            List<OneFrameButton_Base> buttonBases = new List<OneFrameButton_Base>();
            string[] CommandPart = commandRegEX.Replace("}.*(", "}@{").Split('@'); //�e�t���[���ŕ������Ă���B
            Regex regex = new Regex(@".*\[(?<command>.*)\].*{(?<cycle>[0-9]*).*}.*");

            foreach (string partCommand in CommandPart)
            //�e�t���[�����Ƃɑ���
            {
                try
                {
                    Match match = regex.Match(partCommand);
                    if (match.Success)
                    {
                        int cycleMatched = int.Parse(match.Groups["cycle"].Value);  //�J��Ԃ����擾
                        string commandMatched = match.Groups["command"].Value;      //�R�}���h�̍쐬

                        if (!commandCache.TryGetValue(commandMatched, out OneFrameButton_Base command))
                        //�����������o����L���b�V����Ԃ�
                        {
                            command = new OneFrameButton_Base(keyList);
                            command.SetCommand(commandMatched, cycleMatched);
                            commandCache.Add(commandMatched, command);
                        }
                        buttonBases.Add(command);
/*

                        for (int i = 0; i < cycleMatched; i++)
                        //�J��Ԃ������f
                        {
                            buttonBases.Add(command);
                        }
*/
                    }
                }
                catch (Exception ex)
                {
                    // ��O�������̏���
                    Debug.WriteLine("Error in CreateCommandData! Dummy Data used currentry: " + ex.Message);
                    Debug.WriteLine(ex);
                    //DummuyData�f�[�^���g�p���܂��B
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
            // ���̃_�~�[�f�[�^�̍쐬
            OneFrameButton_Base dummyCommand = new OneFrameButton_Base(keyList);
            // ���̃f�[�^���Z�b�g����Ȃǂ̏������s��
            foreach (char key in keyList.GetAllOutputWordList())
            {
                dummyCommand.SetCommandElement(key, ButtonState.None);
            }
            return dummyCommand;
        }
    }
}