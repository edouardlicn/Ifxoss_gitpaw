using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CatPaw
{
    /// <summary>
    /// 战斗中的 UI
    /// </summary>
    public class BattleForm : UGuiForm
    {
        [SerializeField]

        private ProcedureBattle m_ProcedureBattle = null;


        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="ss"></param>
        public void SetTime(int mi, int ss)
        {
            Transform transform = this.gameObject.transform.Find("Mask/Background/TimerBG/Timer");
            Text text = transform.gameObject.GetComponent<Text>();
            text.text = mi.ToString().PadLeft(2, '0') + ":" + ss.ToString().PadLeft(2, '0');
        }


        #region 打开关闭

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

            //m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
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
        #endregion

    }
}
