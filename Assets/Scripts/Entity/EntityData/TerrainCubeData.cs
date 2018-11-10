using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatPaw
{
    [Serializable]
    public class TerrainCubeData : TerrainData
    {
        public TerrainCubeData(int entityId, int typeId, int x, int y, int z) : base(entityId, typeId, x, y, z)
        {
        }
    }
}
