using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using System.Collections.Generic;
namespace CatPaw
{
    public partial class ProcedureChangeScene : ProcedureBase
    {
        private const int MenuSceneId = 1;

        private int m_ScenesId;
        private bool m_IsChangeSceneComplete = false;
        private int m_BackgroundMusicId = 0;
        //private Dictionary<string, bool> m_UnLoadingScene;
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_IsChangeSceneComplete = false;

            //GameEntry.Event.Subscribe(UnloadSceneSuccessEventArgs.EventId, OnUnLoadSceneSuccess);
            //GameEntry.Event.Subscribe(UnloadSceneFailureEventArgs.EventId, OnUnLoadSceneFailure);

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            // 停止所有声音
            GameEntry.Sound.StopAllLoadingSounds();
            GameEntry.Sound.StopAllLoadedSounds();

            // 隐藏所有实体
            GameEntry.Entity.HideAllLoadingEntities();
            GameEntry.Entity.HideAllLoadedEntities();


            /*if(m_UnLoadingScene == null) {
                m_UnLoadingScene = new Dictionary<string, bool>();
            }else{
                m_UnLoadingScene.Clear();
            }*/
            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                //m_UnLoadingScene.Add(loadedSceneAssetNames[i],false);
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            this.m_ScenesId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(this.m_ScenesId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", this.m_ScenesId.ToString());
                return;
            }
            if (!GameEntry.Scene.SceneIsLoading(AssetUtility.GetSceneAsset(drScene.AssetName)))
            {
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);
            }
            m_BackgroundMusicId = drScene.BackgroundMusicId;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            //GameEntry.Event.Unsubscribe(UnloadSceneSuccessEventArgs.EventId, OnUnLoadSceneSuccess);
            //GameEntry.Event.Unsubscribe(UnloadSceneFailureEventArgs.EventId, OnUnLoadSceneFailure);

            GameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
            GameEntry.Event.Unsubscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
            GameEntry.Event.Unsubscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            /*if(m_UnLoadingScene == null || m_UnLoadingScene.Count == 0){
                LoadScene(procedureOwner);
            }else{
                IEnumerator<bool> ietr = m_UnLoadingScene.Values.GetEnumerator();
                while (ietr.MoveNext())
                {
                    if (!ietr.Current)
                        return;
                }
                LoadScene(procedureOwner);
            }*/
           


            if (!m_IsChangeSceneComplete)
            {
                return;
            }

            switch (this.m_ScenesId)
            {
                case 1:
                    ChangeState<ProcedureMenu>(procedureOwner);
                    break;
                case 2:                    
                    ChangeState<ProcedureBattle>(procedureOwner);                
                    break;
                case 3:
                    ChangeState<ProcedureHall>(procedureOwner);
                    break;
                default:
                    ChangeState<ProcedureBattle>(procedureOwner);
                    break;
            }
            
        }

        /*private void LoadScene(ProcedureOwner procedureOwner )
        {
            this.m_ScenesId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(this.m_ScenesId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", this.m_ScenesId.ToString());
                return;
            }
            if(!GameEntry.Scene.SceneIsLoading(AssetUtility.GetSceneAsset(drScene.AssetName))){
                GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);
            }
            m_BackgroundMusicId = drScene.BackgroundMusicId;
        }

        private void OnUnLoadSceneSuccess(object sender, GameEventArgs e) {
            UnloadSceneSuccessEventArgs ne = (UnloadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Log.Info("UnLoad scene '{0}' OK.", ne.SceneAssetName);
            if(m_UnLoadingScene.ContainsKey(ne.SceneAssetName))
            {
                m_UnLoadingScene[ne.SceneAssetName] = true;
            }
        }

        private void OnUnLoadSceneFailure(object sender, GameEventArgs e) {
            UnloadSceneFailureEventArgs ne = (UnloadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Log.Info("UnLoad scene '{0}' Fail.", ne.SceneAssetName);

        }*/

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

            if (m_BackgroundMusicId > 0)
            {
                GameEntry.Sound.PlayMusic(m_BackgroundMusicId);
            }

            m_IsChangeSceneComplete = true;
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());
        }
    }
}
