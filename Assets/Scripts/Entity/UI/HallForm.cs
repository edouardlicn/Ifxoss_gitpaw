using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CatPaw
{
    public class HallForm : UGuiForm
    {
        [SerializeField]
    
        private ProcedureHall m_ProcedureHall = null;

        public void OnButtonGo()
        {
            m_ProcedureHall.ToWorldMap();
        }

        /// <summary>
        /// 设置金币值
        /// </summary>
        /// <param name="value"></param>
        public void SetGoldValue(int value)
        {
            Transform transform = this.gameObject.transform.Find("Mask/Background/Infomationbar/Volgold");
            Text text = transform.gameObject.GetComponent<Text>();
            text.text = value.ToString();
        }

        /// <summary>
        /// 设置钻石值
        /// </summary>
        /// <param name="value"></param>
        public void SetDiamondValue(int value)
        {
            Transform transform = this.gameObject.transform.Find("Mask/Background/Infomationbar/Voldiamond");
            Text text = transform.gameObject.GetComponent<Text>();
            text.text = value.ToString();
        }

        /// <summary>
        /// 设置玩家名
        /// </summary>
        /// <param name="name"></param>
        public void SetUsername(string name)
        {
            Transform transform = this.gameObject.transform.Find("Mask/Background/Infomationbar/Username");
            Text text = transform.gameObject.GetComponent<Text>();
            text.text = name;
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
