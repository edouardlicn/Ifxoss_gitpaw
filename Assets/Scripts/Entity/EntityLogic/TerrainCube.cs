using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityGameFramework.Runtime;

using GameFramework;

namespace CatPaw
{
    public abstract class TerrainCube : TerrainBase
    {
        [SerializeField]
        private TerrainCubeData m_TerrainCubeData = null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TerrainCubeData = userData as TerrainCubeData;
            if (m_TerrainCubeData == null)
            {
                throw new GameFrameworkException();
            }

            if(m_TerrainCubeData.Y == 1) {
                if(m_TerrainCubeData.Z == 0) {
                    var r = GetComponent<Renderer>();
                    var m = r.material;
                    var c = m.color;
                    m.color = new Color(c.r, c.g, c.b, c.a * 0.3f);
                    m.SetRenderingMode(RenderingMode.Transparent);
                }
            }

        }

        protected override void OnHide(object userData)
        {
            m_TerrainCubeData = null;

            base.OnHide(userData);
        }
        
    }
}
