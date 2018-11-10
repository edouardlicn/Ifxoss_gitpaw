using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityGameFramework.Runtime;
using GameFramework;

namespace CatPaw {
    public sealed class TerrainBrickA3 : TerrainBrick {

        protected override void OnCharactorEnter(Charactor other) {
            base.OnCharactorEnter(other);


            // 踩中玩家移动速度减慢50%
            // 维持3秒
            // 如已在减速状态下无效
        }
    }
}