using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityGameFramework.Runtime;
using GameFramework;

namespace CatPaw {
    public sealed class TerrainBrickA2 : TerrainBrick {

        const string COLOR1 = "e62f8b";
        const string COLOR2 = "fff462";
        const string COLOR3 = "79c06e";
        const string COLOR4 = "915da3";

        [SerializeField]
        Renderer m_Renderer;

        protected override void OnShow(object userData) {
            base.OnShow(userData);

            m_Renderer = GetComponent<Renderer>();

            //m_Renderer.material.SetRenderingMode(RenderingMode.Transparent);
            m_Renderer.enabled = false;
        }

        protected override void OnCharactorEnter(Charactor other) {
            base.OnCharactorEnter(other);

            // TODO.

            // 起始值透明
            // 被踩后颜色变为该队颜色
            // 音符方块周边除外
            // 占有颜色数最多的队伍胜出

            if (OwnerType == other.OwnerType) {
                return;
            }

            CampType from = OwnerType;

            OwnerType = other.OwnerType;

            CampType to = OwnerType;

            Color toColor = Color.white;

            switch (OwnerType) {
            case CampType.Player:
                ColorUtility.TryParseHtmlString(COLOR1, out toColor);
                break;
            case CampType.Player2:
                ColorUtility.TryParseHtmlString(COLOR2, out toColor);
                break;
            case CampType.Enemy:
                ColorUtility.TryParseHtmlString(COLOR3, out toColor);
                break;
            case CampType.Enemy2:
                ColorUtility.TryParseHtmlString(COLOR4, out toColor);
                break;

            }

            m_Renderer.material.SetColor("_Color", toColor);
            m_Renderer.enabled = true;

            GameEntry.Event.Fire(this, new OccupyTerrainEventArgs().Fill(from, to));

        }
    }
}