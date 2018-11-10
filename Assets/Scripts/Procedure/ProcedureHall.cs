using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace CatPaw
{
    public class ProcedureHall : ProcedureBase
    {
        //private bool m_StartGame = false;

        private UGuiForm m_Form = null;

        //private string m_FormName = "HallForm";

        /// <summary>
        /// 关卡等级
        /// </summary>
        private string m_BattleLevel = "";

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }        
        }

        /// <summary>
        /// 跳转到地图界面
        /// </summary>
        public void ToWorldMap()
        {
            OpenForm("MapForm");
        }

        /// <summary>
        /// 跳转到大厅界面
        /// </summary>
        public void ToHall()
        {
            OpenForm("HallForm");
        }

        /// <summary>
        /// 进入指定关卡
        /// </summary>
        /// <param name="pointName"></param>
        public void ToBattleLevel(string pointName)
        {
            this.m_BattleLevel = pointName;
        }

        #region 打开/关闭 UI
        private void OpenForm(string formName)
        {
            CloseForm();
            if (formName.Equals("HallForm"))
            {
                GameEntry.UI.OpenUIForm(UIFormId.HallForm, this);
            }
            else
            {
                GameEntry.UI.OpenUIForm(UIFormId.MapForm, this);
            }
            //m_FormName = formName;
        }

        private void CloseForm(bool isShutdown = false)
        {
            if (m_Form != null)
            {
                m_Form.Close(isShutdown);
                m_Form = null;
            }
        }
        #endregion

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            
            GameEntry.UI.OpenUIForm(UIFormId.HallForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            CloseForm(isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!string.IsNullOrEmpty(this.m_BattleLevel))
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Battle"));
                procedureOwner.SetData<VarString>(Constant.ProcedureData.BattleLevel, this.m_BattleLevel);
                this.m_BattleLevel = "";
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_Form = (UGuiForm)ne.UIForm.Logic;
            if (m_Form.name.StartsWith("HallForm"))
            {
                HallForm m_HallForm = (HallForm)m_Form;
                m_HallForm.SetDiamondValue(9);
                m_HallForm.SetGoldValue(999);
                m_HallForm.SetUsername("Victor");
            }
            //GameObject hallForm = GameObject.Find("HallForm");

        }
    }
}
