using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace CatPaw {

    public class Charactor : Entity {

        [SerializeField]
        CharactorData m_Charactor_BasekData;

        [SerializeField]
        float m_MoveSpeed;

        [SerializeField]
        bool m_IsRunning;

        [SerializeField]
        Weapon m_Weapon;

        [SerializeField]
        int m_Bullet;

        [SerializeField]
        int m_CoolDown;

        [SerializeField]
        bool m_IsReverseMoveDirection;

        [SerializeField]
        float m_ReverseMoveDirectionDuraction;

        float m_ElapseSeconds;

        protected override void OnShow(object userData) {
            base.OnShow(userData);

            m_Charactor_BasekData = userData as CharactorData;
            if (m_Charactor_BasekData == null) {
                return;
            }

            m_MoveSpeed = m_Charactor_BasekData.MoveSpeed;
            m_IsRunning = false;
            m_Bullet = m_Charactor_BasekData.BulletMax;
            m_CoolDown = 0;
        }

        protected override void OnHide(object userData) {
            m_Weapon = null;
            m_Charactor_BasekData = null;

            base.OnHide(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds) {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
            if (m_ElapseSeconds >= 1f) {
                m_ElapseSeconds -= 1f;


                if (m_CoolDown > 0) {
                    m_CoolDown--;

                    if (m_CoolDown == 0) {
                        m_Bullet = m_Charactor_BasekData.BulletMax;
                    }

                }
            }

            if(m_ReverseMoveDirectionDuraction > 0f) {
                m_ReverseMoveDirectionDuraction -= elapseSeconds;
                if(m_ReverseMoveDirectionDuraction <= 0f) {
                    if (m_IsReverseMoveDirection) {
                        m_IsReverseMoveDirection = false;
                    }
                    m_ReverseMoveDirectionDuraction = 0f;
                }
            }

            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");

            bool isRunning = !Mathf.Approximately(hor, 0f) || !Mathf.Approximately(ver, 0f);

            if (isRunning) {

                if (m_IsReverseMoveDirection) {
                    hor *= -1f;
                    ver *= -1f;
                }

                Vector3 dir = new Vector3(hor, 0, ver).normalized;
                CachedTransform.forward = dir;

                CachedTransform.position += dir * m_MoveSpeed * elapseSeconds;

                CheckMoveCollision();
            }

            if (m_IsRunning != isRunning) {
                m_IsRunning = isRunning;
                if (CachedAnimator != null) {
                    CachedAnimator.SetBool("isRunning", m_IsRunning);
                }
            }

            if (m_Weapon == null) {
                //Debug.LogWarning("m_Weapon == null.");
                return;
            }

            if (m_CoolDown > 0) {
                return;
            }

            if (Input.GetKey(KeyCode.K) && m_Weapon.CanFire()) {


                m_Weapon.StartFire();
                m_Bullet--;

                if (m_Bullet <= 0) {

                    m_CoolDown = m_Charactor_BasekData.CoolDown;

                }
            }

        }

        public void Teleport(Vector3 position) {
            CachedTransform.position = position;

            CheckMoveCollision();
        }

        public void Die() {

            /// TODO.

        }

        public bool CanBorn() {

            /// TODO.
            /// 
            /// 每个猫有5格血
            /// 当HP值为零时角色死亡
            /// 3秒后在起始点复活
            /// 

            return true;
        }

        public void StartBorn(Vector3 position) {

            /// TODO.

            Teleport(position);

        }

        public void CheckMoveCollision() {

                if (CachedRigidbody != null) {
                    CachedRigidbody.MovePosition(CachedTransform.position);
                    CachedRigidbody.velocity = Vector3.zero;
                }
        }

        public void SetupWeapon(Weapon weapon) {
            m_Weapon = weapon;
        }

        public bool CanReverseMoveDirection() {
            return !m_IsReverseMoveDirection;
        }

        public void StartReverseMoveDirection(float duraction) {
            m_ReverseMoveDirectionDuraction = duraction;
            m_IsReverseMoveDirection = true;
        }

        public bool IsReverseMoveDirection {
            get {
                return m_IsReverseMoveDirection;
            }
        }

        public CampType OwnerType {
            get {
                return m_Charactor_BasekData.OwnerType;
            }
        }

        public string WeaponType {
            get {
                return m_Charactor_BasekData.WeaponType;
            }
        }

        public Weapon Weapon {
            get {
                return m_Weapon;
            }
        }

    }
}