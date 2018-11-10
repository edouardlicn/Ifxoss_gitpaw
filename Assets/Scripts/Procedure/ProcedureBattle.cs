using System;
using System.Collections.Generic;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace CatPaw
{
    public class ProcedureBattle : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        //private CharSelectorForm m_CharSelectorForm;

        private UGuiForm m_Form = null;


        //private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoManage = false;
        //private float m_GotoMenuDelaySeconds = 0f;
        private string m_battleLevel;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// 角色选择-确认
        /// </summary>
        public void OnCharSelectorConfirm()
        {
            if (m_CurrentGame == null)
            {
                m_CurrentGame = new SurvivalGame(m_battleLevel);
                m_CurrentGame.Initialize();

                // 确认好角色后, 关掉当前 UI
                if (m_Form != null)
                {
                    m_Form.Close();
                }

                GameEntry.UI.OpenUIForm(UIFormId.BattleForm, this);
            }
        }

        /// <summary>
        /// 加载本地化资源文件
        /// </summary>
        public void Test()
        {
            var local = GameEntry.Localization;
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 1,
                Title = local.GetRawString("Main.GameOverTitle"),
                //Message = local.GetString("Main.GameOverMessage", _getColorName()),
                //OnClickConfirm = _confirm,
            });
        }

        /// <summary>
        /// 角色选择-取消
        /// </summary>
        public void OnCharSelectorCancel()
        {
            m_GotoManage = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            //m_Games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            //m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.CharSelectorForm, this);

            m_GotoManage = false;
            // 获取关卡数据
            this.m_battleLevel = procedureOwner.GetData<VarString>(Constant.ProcedureData.BattleLevel).Value;

        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_Form = (UGuiForm)ne.UIForm.Logic;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_Form != null)
            {
                m_Form.Close(isShutdown);
                m_Form = null;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);


            if (m_GotoManage)
            {
                if (m_CurrentGame != null)
                {
                    m_CurrentGame.Shutdown();
                    m_CurrentGame = null;
                }

                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Hall"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            else
            {
                if (m_CurrentGame != null && !m_CurrentGame.GameOver)
                {
                    if (m_Form is BattleForm)
                    {
                        BattleForm battleForm = (BattleForm)m_Form;
                        battleForm.SetTime(0, 0);
                    }
                    m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                    return;
                }
            }

        }
    }
}
