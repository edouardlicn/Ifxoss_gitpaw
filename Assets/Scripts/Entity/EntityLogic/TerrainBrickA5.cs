using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityGameFramework.Runtime;
using GameFramework;
using GameFramework.Event;

namespace CatPaw {
    public sealed class TerrainBrickA5 : TerrainBrick {


        protected override void OnCharactorEnter(Charactor other) {
            base.OnCharactorEnter(other);


            // TODO.

            // 踩中玩家传送到场景内除本方块外的随机另一传送方块
            // 如果没有其他传送方块则本方块无效

            GameEntry.Event.Fire(this, new TeleportCharactorEventArgs().Fill(other.Id, Id));
        }
    }
}