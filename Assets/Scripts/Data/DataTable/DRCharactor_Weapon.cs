using GameFramework.DataTable;
using System.Collections.Generic;
namespace CatPaw
{
    public class DRCharactor_Weapon : IDataRow
    {
        public int Id { get; set; }

        /// <summary>
        /// 武器类型
        /// </summary>
        public string WeaponType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string WeaponName { get; set; }

        /// <summary>
        /// 穿越角色
        /// </summary>
        public bool ThroughCharactor { get; set; }

        /// <summary>
        /// 穿越墙壁
        /// </summary>
        public bool ThroughWall { get; set; }

        /// <summary>
        /// 角色持有武器
        /// </summary>
        public bool CharactorOwner { get; set; }

        /// <summary>
        /// 发射后武器从手中消失, 如回旋镖这类
        /// </summary>
        public bool Visibled { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            //index++;
            WeaponType = text[index++];
            WeaponName = text[index++];
            ThroughCharactor = bool.Parse(text[index++]);
            ThroughWall = bool.Parse(text[index++]);
            CharactorOwner = bool.Parse(text[index++]);
            Visibled = bool.Parse(text[index++]);
        }
    }
}
