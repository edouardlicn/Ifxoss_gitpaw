using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityGameFramework.Runtime;
using GameFramework;

namespace CatPaw
{

    public abstract class TerrainBase : Entity {

        public const float OFFSET = 2f;

        [SerializeField]
        CampType m_OwnerCampType;

        public CampType OwnerType
        {
            get
            {
                return m_OwnerCampType;
            }
            protected set
            {
                m_OwnerCampType = value;
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            TerrainData terrainData = userData as TerrainData;

            if (terrainData == null)
            {
                throw new GameFrameworkException();
            }

            transform.position = new Vector3(terrainData.X * OFFSET, terrainData.Y * OFFSET, terrainData.Z * OFFSET);

            Collider c = GetComponent<Collider>();

            if(c == null) {
                return;
            }

            DRTerrain dr = GameEntry.DataTable.GetDataTable<DRTerrain>().GetDataRow(terrainData.TypeId);

            if(dr == null) {

                return;

            }

            c.isTrigger = dr.CanMovePass;

        }
    }
}