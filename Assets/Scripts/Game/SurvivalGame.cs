using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityGameFramework.Runtime;
using GameFramework.Event;

namespace CatPaw {

    public class SurvivalGame : GameBase {

        public bool IsGameStarted {
            get {
                return m_IsGameStarted;
            }
        }

        string m_TerrainName;

        TerrainManager m_TerrainManager;

        CameraController m_CameraController;
        CharactorManager m_CharactorManager;

        Charactor m_Charactor;

        bool m_IsGameStarted;

        readonly Dictionary<CampType, int> m_ScoreMap = new Dictionary<CampType, int>();

        float m_ElapseSeconds;

        int m_GameDuraction;

        public SurvivalGame(string terrainName) {
            m_TerrainName = terrainName;
            m_TerrainManager = new TerrainManager();
            m_CameraController = new CameraController();
            m_CharactorManager = new CharactorManager();

            m_ScoreMap.Add(CampType.Player, 0);
            m_ScoreMap.Add(CampType.Player2, 0);
            m_ScoreMap.Add(CampType.Neutral, 0);
            m_ScoreMap.Add(CampType.Neutral2, 0);
            m_ScoreMap.Add(CampType.Enemy, 0);
            m_ScoreMap.Add(CampType.Enemy2, 0);
        }

        public override void Initialize() {
            base.Initialize();

            /// 2分钟游戏时间
            m_GameDuraction = 2 * 60;
            m_ElapseSeconds = 0f;
            m_IsGameStarted = false;

            /// 加载地形

            m_TerrainManager.Load(m_TerrainName, LoadTerrainSuccessCallback, LoadTerrainFailureCallback);
        }

        void LoadTerrainSuccessCallback() {

            /// TODO.
            /// 
            /// 加载角色
            /// 

            m_CharactorManager.Load("cat001", CampType.Player, LoadCharactorSuccessCallback);
        }

        void LoadTerrainFailureCallback() {

        }

        void LoadCharactorSuccessCallback(Charactor charactor) {

            m_Charactor = charactor;

            m_Charactor.StartBorn(m_TerrainManager.GetBornPointA0());
            m_CameraController.Init(m_Charactor.CachedTransform);

            GameEntry.Event.Subscribe(TeleportCharactorEventArgs.EventId, TeleportCharactorEventHandler);
            GameEntry.Event.Subscribe(OccupyTerrainEventArgs.EventId, OccupyTerrainEventHandler);

            /// TODO.
            ///
            /// 游戏开始
            /// 

            m_IsGameStarted = true;


                    Log.Warning("游戏开始");
        }

        void OccupyTerrainEventHandler(object sender, GameEventArgs e) {

            OccupyTerrainEventArgs args = e as OccupyTerrainEventArgs;

            if(args == null) {
                return;
            }

            if (m_ScoreMap.ContainsKey(args.From)) {
                m_ScoreMap[args.From] -= 1;

                /// TODO.

                //Log.Warning("score[" + args.From + "] = " + m_ScoreMap[args.From]);
            }

            if (m_ScoreMap.ContainsKey(args.To)) {
                m_ScoreMap[args.To] += 1;

                /// TODO.

                //Log.Warning("score[" + args.To + "] = " + m_ScoreMap[args.To]);
            }

        }

        void TeleportCharactorEventHandler(object sender, GameEventArgs e) {
            TeleportCharactorEventArgs args = e as TeleportCharactorEventArgs;

            if(args == null) {
                return;
            }

            Charactor charactor = GameEntry.Entity.GetEntity(args.CharactorId).Logic as Charactor;

            if(charactor == null) {
                return;
            }

            TerrainBrickA5 a5 = GameEntry.Entity.GetEntity(args.FromTerrainBrickId).Logic as TerrainBrickA5;

            if(a5 == null) {
                return;
            }

            Vector3 to;
            if(m_TerrainManager.TryGetOtherTeleporter(a5, out to)) {

                charactor.Teleport(to);

            }



        }

        public override GameMode GameMode {
            get {
                return GameMode.Survival;
            }
        }

        public override void Update(float elapseSeconds, float realElapseSeconds) {
            base.Update(elapseSeconds, realElapseSeconds);

            if (!m_IsGameStarted) {
                return;
            }



            m_ElapseSeconds += elapseSeconds;
            if(m_ElapseSeconds >= 1f) {
                m_ElapseSeconds -= 1f;
                m_GameDuraction -= 1;

                if(m_GameDuraction == 0) {

                    /// TODO.
                    /// 
                    /// 游戏结束
                    /// 

                    Log.Warning("游戏结束");

                }

            }

            m_CameraController.Update(elapseSeconds, realElapseSeconds);
        }

        public override void Shutdown() {

            m_IsGameStarted = false;

            GameEntry.Event.Unsubscribe(OccupyTerrainEventArgs.EventId, OccupyTerrainEventHandler);
            GameEntry.Event.Unsubscribe(TeleportCharactorEventArgs.EventId, TeleportCharactorEventHandler);

            m_CameraController.Release();

            if (m_Charactor != null) {
                m_CharactorManager.Unload(m_Charactor);
                m_Charactor = null;
            }

            m_TerrainManager.Unload();

            base.Shutdown();


        }

    }
}
