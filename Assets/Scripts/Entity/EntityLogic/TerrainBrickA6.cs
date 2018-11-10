using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityGameFramework.Runtime;
using GameFramework;

namespace CatPaw {
    public sealed class TerrainBrickA6 : TerrainBrick {


        protected override void OnCharactorEnter(Charactor other) {
            base.OnCharactorEnter(other);

            // TODO.

            // 踩中玩家以每秒5%的速度恢复油漆

        }
    }
}