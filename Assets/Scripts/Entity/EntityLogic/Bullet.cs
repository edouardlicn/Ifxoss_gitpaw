using UnityEngine;
using UnityGameFramework.Runtime;

namespace CatPaw
{
    /// <summary>
    /// 子弹类。
    /// </summary>
    public class Bullet : Entity
    {
        [SerializeField]
        private BulletData m_BulletData = null;

        public ImpactData GetImpactData()
        {
            return new ImpactData(m_BulletData.OwnerCamp, 0, m_BulletData.Attack, 0);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_BulletData = userData as BulletData;
            if (m_BulletData == null)
            {
                Log.Error("Bullet data is invalid.");
                return;
            }
            CachedTransform.localScale = Vector3.one * 0.1f;
            CachedTransform.transform.AddLocalPositionY(0.5f);
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            CachedTransform.Translate(Vector3.forward * m_BulletData.Speed * elapseSeconds, Space.Self);
        }
    }
}
