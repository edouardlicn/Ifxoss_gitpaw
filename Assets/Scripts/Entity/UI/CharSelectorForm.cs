using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityGameFramework.Runtime;

namespace CatPaw
{
    /// <summary>
    /// 角色选择界面
    /// </summary>
    public class CharSelectorForm : UGuiForm
    {

        private ProcedureBattle m_ProcedureBattle = null;

        /// <summary>
        /// 确认
        /// </summary>
        public void OnConfirmButtonClick()
        {
            m_ProcedureBattle.OnCharSelectorConfirm();
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void OnCancelButtonClick()
        {
            m_ProcedureBattle.OnCharSelectorCancel();
            //GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }



#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureBattle = (ProcedureBattle)userData;
            if (m_ProcedureBattle == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            m_ProcedureBattle = null;

            base.OnClose(userData);
        }
    }
}
