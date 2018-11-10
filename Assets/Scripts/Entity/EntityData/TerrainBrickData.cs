using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatPaw
{
    /// <summary>
    /// 地砖数据
    /// </summary>
    [Serializable]
    public class TerrainBrickData : TerrainData
    {
        public TerrainBrickData(int entityId, int typeId, int x, int y, int z) : base(entityId, typeId, x, y, z)
        {
        }
    }
}
