using GameFramework.DataTable;
using System.Collections.Generic;

namespace CatPaw
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class DRCharactor_Base : IDataRow
    {
        public int Id { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 游戏内名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 敌人类型
        /// </summary>
        public string EnemyType { get; set; }

        /// <summary>
        /// 敌人资源
        /// </summary>
        public string EnemyResource { get; set; }

        /// <summary>
        /// AI级别
        /// </summary>
        public string AILevel { get; set; }

        /// <summary>
        /// 类型基础
        /// </summary>
        public string TypeBase { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Remark { get; set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            //index++;
            Key = text[index++];
            Name = text[index++];
            EnemyType = text[index++];
            EnemyResource = text[index++];
            AILevel = text[index++];
            TypeBase = text[index++];
            Remark = text[index++];
        }
    }
}
