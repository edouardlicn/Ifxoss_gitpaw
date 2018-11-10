using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using GameFramework;
using UnityGameFramework.Runtime;

namespace CatPaw {

    public class Weapon : Entity {


        [SerializeField]
        WeaponData m_Charactor_WeaponData;

        [SerializeField]
        Charactor m_Owner;

        [Range(0f, 10f)]
        [SerializeField]
        float m_CoolDownMax;

        float m_CoolDown;

        protected override void OnShow(object userData) {
            base.OnShow(userData);

            m_Charactor_WeaponData = userData as WeaponData;
            if (m_Charactor_WeaponData == null) {
                return;
            }
            m_CoolDown = 0f;
            m_CoolDownMax = 0.3f;

            GameEntry.Entity.AttachEntity(Entity, m_Charactor_WeaponData.OwnerId);

            //暂时隐藏
            gameObject.SetActive(false);
        }

        protected override void OnHide(object userData) {
            m_Owner = null;
            m_Charactor_WeaponData = null;

            base.OnHide(userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData) {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            CachedTransform.localPosition = Vector3.zero;

            m_Owner = parentEntity as Charactor;

            if(m_Owner == null) {
                throw new GameFrameworkException("Not found weapon owner[" + m_Charactor_WeaponData.OwnerId + "].");
            }

            m_Owner.SetupWeapon(this);

        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData) {
            base.OnAttached(childEntity, parentTransform, userData);

            /// TEST.
            /// 

            Log.Warning(name + "..OnAttached");

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds) {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if(m_CoolDown > 0f) {
                m_CoolDown -= elapseSeconds;
                
            }
            
        }

        public bool CanFire() {
            return m_CoolDown <= 0f;
        }

        public void StartFire() {

            //Log.Warning(name + "..Fire");
            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), 1, m_Charactor_WeaponData.OwnerId, m_Owner.OwnerType, 5, 20)

            {
                Position = CachedTransform.position,
                Rotation = CachedTransform.rotation
            });

            m_CoolDown = m_CoolDownMax;
        }

    }
}