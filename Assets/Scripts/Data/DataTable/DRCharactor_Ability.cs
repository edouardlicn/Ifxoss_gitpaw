using GameFramework.DataTable;
using System.Collections.Generic;

namespace CatPaw
{ 
    /// <summary>
    /// 角色能力  #系统名称,护甲,武器类型,武器名称,子弹数,换弹CD,伤害,攻击逻辑
    /// </summary>
    public class DRCharactor_Ability : IDataRow
    {
        public int Id { get; set; }
        
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 护甲
        /// </summary>
        public int Armor { get; set; }

        /// <summary>
        /// 武器类型
        /// </summary>
        public string WeaponType { get; set; }

        /// <summary>
        /// 武器名称
        /// </summary>
        public string WeaponName { get; set; }

        /// <summary>
        /// 子弹数
        /// </summary>
        public int BulletMax { get; set; }

        /// <summary>
        /// 换弹时间
        /// </summary>
        public int CoolDown { get; set; }

        /// <summary>
        /// 伤害
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        ///  攻击逻辑
        /// </summary>
        public string Logic { get; set; }


        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            //index++;
            Key = text[index++];
            Armor = int.Parse(text[index++]);
            WeaponType = text[index++];
            WeaponName = text[index++];
            BulletMax = int.Parse(text[index++]);
            CoolDown = int.Parse(text[index++]);
            Damage = int.Parse(text[index++]);
            Logic = text[index++];
        }
    }
}
