using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityGameFramework.Runtime;
using GameFramework;

namespace CatPaw {
    public sealed class TerrainBrickA4 : TerrainBrick {

        [Range(1f, 9f)]
        [SerializeField]
        float m_ReverseMoveDirectionDuraction;

        protected override void OnInit(object userData) {
            base.OnInit(userData);

            m_ReverseMoveDirectionDuraction = 3f;
        }

        protected override void OnCharactorEnter(Charactor other) {
            base.OnCharactorEnter(other);

            // 踩中玩家上下左右方向颠倒
            // 维持3秒
            // 如已在方向颠倒状态下无效

            if (other.CanReverseMoveDirection()) {
                other.StartReverseMoveDirection(m_ReverseMoveDirectionDuraction);
            }

        }

    }
}