using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputCommand{
    /// <summary>
    /// Command�̃R���o�[�^
    /// </summary>
    public class CommandConverter
    {
        /// <summary>
        /// CommandData���X�g��Command���X�g�ɕϊ�����
        /// </summary>
        /// <param name="commandDatas">CommandData�̃��X�g</param>
        /// <returns>Command���X�g</returns>
        static public List<Command> FromCommandDatas(List<CommandData> commandDatas)
        {
            List<Command> commands = new List<Command>();
            foreach (CommandData temp in commandDatas)
            {
                commands.Add(new Command(temp.GetOutputWord(), temp.GetInputtAcceptanceFrame(), temp.GetCommandRegEXs()));
            }
            return commands;
        }
    }
}
