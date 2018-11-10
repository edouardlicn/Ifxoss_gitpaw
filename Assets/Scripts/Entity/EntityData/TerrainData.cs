using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CatPaw
{
    [Serializable]
    public class TerrainData : EntityData
    {
        [SerializeField]
        int m_X;
        [SerializeField]
        int m_Y;
        [SerializeField]
        int m_Z;

        public TerrainData(int entityId, int typeId, int x, int y, int z) : base(entityId, typeId)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }


        public int X
        {
            get
            {
                return m_X;
            }
        }


        public int Y
        {
            get
            {
                return m_Y;
            }
        }


        public int Z
        {
            get
            {
                return m_Z;
            }
        }
    }
}
