using InputCommand;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;




namespace InputCommand
{
    /// <summary>
    /// コマンドを保存するスクリプタブルオブジェクト
    /// </summary>

    [CreateAssetMenu(fileName = "CommandList", menuName = "ScriptableObjects/Command", order = 1)]
    public class CommandListStore : ScriptableObject
    {

        public List<CommandData> commandData = new List<CommandData>();   //コマンド情報　 手動入力or後で自動入力
        List<Command> commands = new List<Command>();

        /// <summary>
        ///　開始時にcommandDataをCommandクラスに変換します
        /// </summary>
        private void OnEnable()
        {
            commands = CommandConverter.FromCommandDatas(commandData);
        }
        /// <summary>
        /// コマンドデータのリストを出力する
        /// </summary>
        /// <returns>コマンドデータリスト(Commandクラス)</returns>
        public List<Command> GetCommandList()
        {
            return commands;
        }
    }
}
