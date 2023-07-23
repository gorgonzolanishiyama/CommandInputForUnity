using InputCommand;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;




namespace InputCommand
{
    /// <summary>
    /// �R�}���h��ۑ�����X�N���v�^�u���I�u�W�F�N�g
    /// </summary>

    [CreateAssetMenu(fileName = "CommandList", menuName = "ScriptableObjects/Command", order = 1)]
    public class CommandListStore : ScriptableObject
    {

        public List<CommandData> commandData = new List<CommandData>();   //�R�}���h���@ �蓮����or��Ŏ�������
        List<Command> commands = new List<Command>();

        /// <summary>
        ///�@�J�n����commandData��Command�N���X�ɕϊ����܂�
        /// </summary>
        private void OnEnable()
        {
            commands = CommandConverter.FromCommandDatas(commandData);
        }
        /// <summary>
        /// �R�}���h�f�[�^�̃��X�g���o�͂���
        /// </summary>
        /// <returns>�R�}���h�f�[�^���X�g(Command�N���X)</returns>
        public List<Command> GetCommandList()
        {
            return commands;
        }
    }
}
