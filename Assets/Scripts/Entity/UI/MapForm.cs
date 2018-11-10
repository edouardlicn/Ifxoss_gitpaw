using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CatPaw
{
    public class MapForm : UGuiForm
    {
        [SerializeField]

        private ProcedureHall m_ProcedureHall = null;

        /// <summary>
        /// 退回按钮
        /// </summary>
        public void OnButtonBack()
        {
            m_ProcedureHall.ToHall();
        }

        /// <summary>
        /// 选择关卡按钮
        /// </summary>
        /// <param name="pointName"></param>
        public void OnButtonPoint(string pointName)
        {
            m_ProcedureHall.ToBattleLevel(pointName);
        }



#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureHall = (ProcedureHall)userData;
            if (m_ProcedureHall == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

            //m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            m_ProcedureHall = null;

            base.OnClose(userData);
        }
    }
}
