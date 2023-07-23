using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputCommand{
    /// <summary>
    /// Commandのコンバータ
    /// </summary>
    public class CommandConverter
    {
        /// <summary>
        /// CommandDataリストをCommandリストに変換する
        /// </summary>
        /// <param name="commandDatas">CommandDataのリスト</param>
        /// <returns>Commandリスト</returns>
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
