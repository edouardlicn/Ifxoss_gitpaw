using GameFramework;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

using GameFramework.Event;

namespace CatPaw {
    public sealed class TerrainManager {

        public int ColSzie {
            get {
                return m_ColSize;
            }
        }

        public int RowSize {
            get {
                return m_RowSize;
            }
        }

        List<int> m_EnitityIds = new List<int>();
        int m_ShowEntitiesCounter;

        int m_ColSize;
        int m_RowSize;

        Action m_OnLoadSuccessCallback;
        Action m_OnLoadFailureCallback;

        LoadAssetCallbacks m_LoadTerrainDataTableCallbacks;
        List<string> m_ToLoadTerrainDataTables = new List<string>();
        Dictionary<string, TextAsset> m_TerrainDataTables = new Dictionary<string, TextAsset>();

        Action m_HideEntitiesCompleteCallback;

        List<TerrainBrickA5> m_Teleporters = new List<TerrainBrickA5>();

        List<TerrainBrickA0> m_BornPointsA0 = new List<TerrainBrickA0>();
        List<TerrainBrickA1> m_BornPointsA1 = new List<TerrainBrickA1>();

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="terrainName">地图名字 对应地图配置文件的文件夹名字</param>
        /// <param name="successCallback">加载成功回调</param>
        /// <param name="failureCallback">加载失败回调</param>
        public void Load(string terrainName, Action successCallback, Action failureCallback) {
            m_OnLoadSuccessCallback = successCallback;
            m_OnLoadFailureCallback = failureCallback;
            LoadTerrainDataTables(terrainName);
        }

        /// <summary>
        /// 卸载
        /// </summary>
        public void Unload() {

            m_BornPointsA0.Clear();
            m_BornPointsA1.Clear();

            UnloadTerrainDataTables();

            m_OnLoadSuccessCallback = null;
            m_OnLoadFailureCallback = null;
        }

        /// <summary>
        /// 获取起始点1位置 没有初始点时返回Vecter3.zero
        /// </summary>
        public Vector3 GetBornPointA0() {
            return GetBornPoint<TerrainBrickA0>(m_BornPointsA0);
        }

        /// <summary>
        /// 获取起始点2位置 没有初始点时返回Vecter3.zero
        /// </summary>
        public Vector3 GetBornPointA1() {
            return GetBornPoint<TerrainBrickA1>(m_BornPointsA1);
        }

        /// <summary>
        /// 尝试获取其他传送点位置
        /// </summary>
        /// <param name="teleporter">当前传送点</param>
        /// <param name="otherTeleporter">其他传送点位置</param>
        public bool TryGetOtherTeleporter(TerrainBrickA5 teleporter, out Vector3 otherTeleporter) {
            otherTeleporter = teleporter.CachedTransform.position;
            List<TerrainBrickA5> others = new List<TerrainBrickA5>();
            foreach (var brick in m_Teleporters) {
                if (brick != teleporter) {
                    others.Add(brick);
                }
            }
            if (others.Count == 0) {
                return false;
            }
            if (others.Count == 1) {
                otherTeleporter = others[0].CachedTransform.position;
                return true;
            }

            otherTeleporter = others[UnityEngine.Random.Range(0, others.Count)].CachedTransform.position;

            return true;
        }

        #region 加载地形配置

        void LoadDataTableSuccessCallBack(string assetName, object asset, float duration, object userData) {
            m_TerrainDataTables.Add(assetName, (TextAsset)asset);

            if (m_ToLoadTerrainDataTables.Contains(assetName)) {
                m_ToLoadTerrainDataTables.Remove(assetName);
            }

            if (m_ToLoadTerrainDataTables.Count == 0) {
                // 加载地形配置结束

                ShowEntities();
            }
        }

        void LoadDataTableFailureCallBack(string assetName, LoadResourceStatus status, string errorMessage, object userData) {
            if (m_OnLoadFailureCallback != null) {
                m_OnLoadFailureCallback();
            }
        }

        void LoadDataTableUpdateCallBack(string assetName, float progress, object userData) {

        }

        void LoadDataTableDependencyAssetCallBack(string assetName, string dependencyAssetName,
        int loadedCount, int totalCount, object userData) {

        }

        void LoadTerrainDataTables(string terrainName) {
            if (m_LoadTerrainDataTableCallbacks != null) {
                throw new GameFrameworkException("Please invoke UnloadTerrainDataTables() at first.");
            }

            // 设置回调

            m_LoadTerrainDataTableCallbacks = new LoadAssetCallbacks(
                LoadDataTableSuccessCallBack,
                LoadDataTableFailureCallBack,
                LoadDataTableUpdateCallBack,
                LoadDataTableDependencyAssetCallBack
            );

            // 记录要加载的地形配置

            m_ToLoadTerrainDataTables.Add(AssetUtility.GetTerrainDataAsset(string.Format("{0}/{0}_0", terrainName)));
            m_ToLoadTerrainDataTables.Add(AssetUtility.GetTerrainDataAsset(string.Format("{0}/{0}_1", terrainName)));

            // 加载地形配置

            var arr = m_ToLoadTerrainDataTables.ToArray();

            foreach (var assetName in arr) {
                GameEntry.Resource.LoadAsset(assetName, m_LoadTerrainDataTableCallbacks);
            }

        }

        void UnloadTerrainDataTables() {

            m_HideEntitiesCompleteCallback = () => {

                /// 隐藏全部实体后 卸载地图配置

                foreach (var kvp in m_TerrainDataTables) {
                    GameEntry.Resource.UnloadAsset(kvp.Value);
                }
                m_TerrainDataTables.Clear();
                m_LoadTerrainDataTableCallbacks = null;
            };

            HideEntities();

        }

        #endregion

        #region 加载创建地形

        void ShowEntities() {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, ShowEntitySuccessCallback);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, ShowEntityFailureCallback);

            BeforeShowTerrain();
            ShowTerrain();
        }

        void BeforeShowTerrain() {

            foreach (var kvp in m_TerrainDataTables) {
                string[] mapRows = kvp.Value.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                m_RowSize = mapRows.Length;
                if (mapRows.Length > 0) {
                    m_ColSize = mapRows[0].Split(',').Length;
                    m_ShowEntitiesCounter += m_RowSize * m_ColSize;
                }
            }

        }

        void ShowTerrain() {
            int layer = 0;
            foreach (var kvp in m_TerrainDataTables) {
                int row = 0;
                string[] mapRows = kvp.Value.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string mapRow in mapRows) {
                    int col = 0;
                    string[] mapUnits = mapRow.Split(',');
                    foreach (string mapUnit in mapUnits) {
                        ShowTerrainFactory(mapUnit, col, layer, row);
                        col++;
                    }
                    row++;
                }
                layer++;
            }
        }

        void AfterShowTerrain() {

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, ShowEntitySuccessCallback);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, ShowEntityFailureCallback);

            if (m_OnLoadSuccessCallback != null) {
                m_OnLoadSuccessCallback();
            }
        }

        void ShowTerrainFactory(string mapUnit, int col, int layer, int row) {

            switch (mapUnit) {
            case "A0": {
                    GameEntry.Entity.ShowTerrainBrick<TerrainBrickA0>(mapUnit, col, layer, row);
                }
                break;
            case "A1": {
                    GameEntry.Entity.ShowTerrainBrick<TerrainBrickA1>(mapUnit, col, layer, row);
                }
                break;
            case "A2": {
                    GameEntry.Entity.ShowTerrainBrick<TerrainBrickA2>(mapUnit, col, layer, row);
                }
                break;
            case "A3": {
                    GameEntry.Entity.ShowTerrainBrick<TerrainBrickA3>(mapUnit, col, layer, row);
                }
                break;
            case "A4": {
                    GameEntry.Entity.ShowTerrainBrick<TerrainBrickA4>(mapUnit, col, layer, row);
                }
                break;
            case "A5": {
                    GameEntry.Entity.ShowTerrainBrick<TerrainBrickA5>(mapUnit, col, layer, row);
                }
                break;
            case "A6": {
                    GameEntry.Entity.ShowTerrainBrick<TerrainBrickA6>(mapUnit, col, layer, row);
                }
                break;
            default: {
                    GameEntry.Entity.ShowTerrainCube<TerrainCube01>(mapUnit, col, layer, row);
                }
                break;
            }

        }

        void HideEntities() {

            GameEntry.Event.Subscribe(HideEntityCompleteEventArgs.EventId, HideEntitySuccessCallback);

            foreach (var id in m_EnitityIds) {
                GameEntry.Entity.HideEntity(id);
            }
        }

        Vector3 GetBornPoint<T>(List<T> bricks) where T : TerrainBrick {

            if (bricks.Count == 1) {
                return bricks[0].CachedTransform.position;
            }

            if (bricks.Count > 1) {
                return bricks[UnityEngine.Random.Range(0, bricks.Count)].CachedTransform.position;
            }

            Log.Warning("Not found bornpoint<" + typeof(T).Name + ">.");

            return Vector3.zero;
        }

        void ShowEntitySuccessCallback(object sender, GameEventArgs e) {
            ShowEntitySuccessEventArgs args = e as ShowEntitySuccessEventArgs;

            if (args == null) {
                return;
            }

            /// 尝试获取起始点/复活点

            if (!FindBornPoints(args)) {

                /// 尝试获取传送点

                if (!FindTeleporters(args)) {



                }
            }

            m_EnitityIds.Add(args.Entity.Id);

            if (m_EnitityIds.Count == m_ShowEntitiesCounter) {
                m_ShowEntitiesCounter = 0;
                AfterShowTerrain();
            }

        }

        bool FindTeleporters(ShowEntitySuccessEventArgs args) {
            EntityLogic entityLogic = args.Entity.Logic;

            TerrainBrickA5 a5 = entityLogic as TerrainBrickA5;

            if (a5 != null) {
                m_Teleporters.Add(a5);
                return true;
            }
            return false;
        }

        bool FindBornPoints(ShowEntitySuccessEventArgs args) {
            EntityLogic entityLogic = args.Entity.Logic;
            TerrainBrickA0 a0 = entityLogic as TerrainBrickA0;
            if (a0 != null) {
                m_BornPointsA0.Add(a0);
                return true;
            }

            TerrainBrickA1 a1 = entityLogic as TerrainBrickA1;
            if (a1 != null) {
                m_BornPointsA1.Add(a1);
                return true;
            }

            return false;
        }

        void ShowEntityFailureCallback(object sender, GameEventArgs e) {
            if (m_OnLoadFailureCallback != null) {
                m_OnLoadFailureCallback();
            }
        }

        void HideEntitySuccessCallback(object sender, GameEventArgs e) {
            HideEntityCompleteEventArgs args = e as HideEntityCompleteEventArgs;
            if (args == null) {
                return;
            }
            m_EnitityIds.Remove(args.EntityId);
            if (m_EnitityIds.Count == 0) {
                GameEntry.Event.Unsubscribe(HideEntityCompleteEventArgs.EventId, HideEntitySuccessCallback);
                if (m_HideEntitiesCompleteCallback != null) {
                    m_HideEntitiesCompleteCallback();
                    m_HideEntitiesCompleteCallback = null;
                }
            }

        }

        #endregion

    }
}
