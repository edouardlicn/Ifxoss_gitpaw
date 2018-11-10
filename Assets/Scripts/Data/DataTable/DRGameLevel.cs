using GameFramework.DataTable;
using System.Collections.Generic;

namespace CatPaw
{
    /// <summary>
    /// 关卡信息 # 关卡名称,游戏规则,BGM,是否有野怪
    /// </summary>
    public class DRGameLevel : IDataRow
    {
        public int Id { get; set; }

        /// <summary>
        /// 关卡名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 游戏规则
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// BGM
        /// </summary>
        public string BGM { get; set; }

        /// <summary>
        /// 是否有野怪
        /// </summary>
        public bool HaveEnemy { get; set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            //index++;
            Name = text[index++];
            Rule = text[index++];
            BGM = text[index++];
            HaveEnemy = bool.Parse( text[index++]);
        }
    }
}
