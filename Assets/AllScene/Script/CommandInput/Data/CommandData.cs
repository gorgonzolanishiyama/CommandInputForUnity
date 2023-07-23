using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputCommand
{
    /// <summary>
    /// �R�}���h���͗p�N���X���N���X
    /// </summary>
    [System.Serializable]
    public class CommandData
    {
        public string outputWord;                 //�o�͕���
        public int inputAcceptanceFrame;          //���͗P�\����
        public string commandRegEXs;              //�R�}���h(���K�\��)

        /// <summary>
        /// �A�N�Z�T�[ �o�� �o�͕���
        /// </summary>
        /// <returns>�o�͕���</returns>
        public string GetOutputWord()
        {
            return outputWord;
        }

        /// <summary>
        /// �A�N�Z�T�[ �o�� ���͗P�\����
        /// </summary>
        /// <returns>���͗P�\����</returns>
        public string GetCommandRegEXs()
        {
            return commandRegEXs;
        }

        /// <summary>
        /// �A�N�Z�T�[ �o�� �R�}���h(���K�\��)
        /// </summary>
        /// <returns>�R�}���h(���K�\��)</returns>
        public int GetInputtAcceptanceFrame()
        {
            return inputAcceptanceFrame;
        }
    }
}
