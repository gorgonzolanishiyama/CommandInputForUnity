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
    // ���̃\�[�X�t�@�C���ł�
    // OneFrameButton_Base(�R�}���h�\�̃f�[�^��)��OneFrameButton_Input(���͒l)���`����B
    // 

    /// <summary>
    /// 1�t���[���̃R�}���h�f�[�^��\���N���X
    /// </summary>
    public class OneFrameButton_Base
    {
        private  KeyList keyList;   // �{�Q�[���Ŏg�p�����L�[�̑Ή����X�g(��)"Up"��"8"�@�R���X�g���N�^�ŃZ�b�g�����
        public Dictionary<char, ButtonState> buttonStates = new Dictionary<char, ButtonState>(); //1�t���[��������̃f�[�^
        public int cycle; //��r����t���[�����B(���߂�60�t���[���Ȃ炱����60F�Ɠ���B�W����1)

        /// <summary>
        /// �R���X�g���N�^
        /// �L�[���X�g���Z�b�g����B
        /// </summary>
        /// <param name="keyList">�g�p�����L�[�̃��X�g</param>
        public OneFrameButton_Base(KeyList keyList)
        {
            this.keyList = keyList;
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            {
                buttonStates[tartgetchar] = ButtonState.None;
            }
        }

        /// <summary>
        /// �T�C�N���񐔕��A�L�[��r���s��
        /// </summary>
        /// <param name="anotherFrameButtons">��r�Ώ�</param>
        /// <returns>��r�̍���</returns>
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
        /// �L�[���X�g�����ɁA��r���s��
        /// </summary>
        /// <param name="anotherFrameButton">��r�Ώ�</param>
        /// <returns>��r�̍���</returns> 
        private bool CompareToInstance(OneFrameButton_Input anotherFrameButton) 
        { 
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            //�S�L�[���r����
            {
                if (buttonStates[tartgetchar] == ButtonState.None)
                //�]�������Ȃ�]���p��
                {
                    continue;
                }
                if (buttonStates[tartgetchar] != anotherFrameButton.buttonStates[tartgetchar])
                //��r���قȂ�p�ł���΋U��Ԃ�
                {
                    return false;
                }
            }
            return true;//�S�Ă��]���O����r�����i�Ȃ�^��Ԃ�
        }

        /// <summary>
        /// ����������߂��ăR�}���h�f�[�^�ɕϊ�����
        /// </summary>
        /// <param name="command">���߂���R�}���h������ (��)�u.2.[^6]...��.2.6...��.[^2].6...��....A..� </param>
        /// <param name="cycle"> ���t���[���O�܂Ŋm�F���邩 </param>
        public void SetCommand(string command, int cycle)
        {
            foreach (char tempSign in keyList.GetAllOutputWordList())
                //�L�[���X�g�ɏ]�����[�v����
            {
                int match = command.IndexOf(tempSign); //�܂ތ��𒲂ׂ�
                if (match >= 0)
                    //�܂ޏꍇ
                {
                    if (match > 0)
                    {
                        if (command[match - 1] == '^')
                            //�ے�Ƃ��Ďg���Ă���ꍇ
                        {
                            buttonStates[tempSign] = ButtonState.Off ;
                        }
                        else
                            //�m��Ƃ��Ďg���Ă���ꍇ
                        {
                            buttonStates[tempSign] = ButtonState.On;
                        }
                    }
                    else
                    //�ꕶ���ڂ̏ꍇ�͍m��Ƃ���
                    {
                        buttonStates[tempSign] = ButtonState.On;
                    }
                }
                else
                    //�܂܂Ȃ��ꍇ(-1)���Ԃ��Ă���B
                {
                    buttonStates[tempSign] = ButtonState.None;
                }
            }
            this.cycle = cycle;
        }
        /// <summary>
        /// ���ڃL�[��ǉ�����ꍇ(�_�~�[�f�[�^�쐬��z��)
        /// </summary>
        /// <param name="key">�ǉ�����L�[�@(�L�[���X�g�Q��)</param>
        /// <param name="state">�ǉ�����X�e�[�g</param>

        public void SetCommandElement(char key, ButtonState state)
        {
            buttonStates[key] = state;
        }

        /// <summary>
        /// Debug�p�̕�����쐬
        /// </summary>
        /// <returns>���̃R�}���h�̕�����\�� (��)�u?2?-???��?2?6???��?-?6???�????A??�</returns>
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
    ///  1�t���[���̃C���v�b�g�f�[�^��\���N���X
    /// </summary>
    public class OneFrameButton_Input
    {
        public KeyList keyList;//�L�[���X�g
        public Dictionary<char, ButtonState> buttonStates = new Dictionary<char, ButtonState>(); //1�t���[��������̃f�[�^

        /// <summary>
        /// �R���X�g���N�^
        /// �L�[���X�g���Z�b�g����B
        /// </summary>
        /// <param name="keyList">�g�p�����L�[�̃��X�g</param>
        public OneFrameButton_Input(KeyList keyList)
        {
            this.keyList = keyList;
            foreach (char tartgetchar in keyList.GetAllOutputWordList())
            {
                buttonStates[tartgetchar] = ButtonState.Off;
            }
        }

        /// <summary>
        /// ����͏�Ԃ̊m�F
        /// </summary>
        /// <param name="state">���͏��</param>
        public void SetStateUpInFixedUpdate(ButtonState state)
        {
            buttonStates['8'] = state;
        }
        /// <summary>
        /// �����͏�Ԃ̊m�F
        /// </summary>
        /// <param name="state">���͏��</param>
        public void SetStateDownInFixedUpdate(ButtonState state)
        {
            buttonStates['2'] = state;
        }
        /// <summary>
        /// �����͏�Ԃ̊m�F
        /// </summary>
        /// <param name="state">���͏��</param>
        public void SetStateLeftInFixedUpdate(ButtonState state)
        {
            buttonStates['4'] = state;
        }
        /// <summary>
        /// �E���͏�Ԃ̊m�F
        /// </summary>
        /// <param name="state">���͏��</param>
        public void SetStateRightInFixedUpdate(ButtonState state)
        {
            buttonStates['6'] = state;
        }

        /// <summary>
        /// �w�肵���L�[�̓��͏�Ԃ�ݒ肷��
        /// </summary>
        /// <param name="sign">�L�[�̕��� (�L�[���X�g���Q��)</param>
        /// <param name="state">���͏��</param>
        public void SetstateButton(char sign , ButtonState state)
            //�w��l
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
        /// Debug�p�̕�����쐬
        /// </summary>
        /// <returns>���݂̃R�}���h������������\�� (��)-2??A??(���Ⴊ��A)</returns>

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
