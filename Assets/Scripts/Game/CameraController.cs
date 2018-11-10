using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityGameFramework.Runtime;
using GameFramework;
using GameFramework.Event;

namespace CatPaw {

    public sealed class CameraController {

        Transform m_Follow;
        Camera m_MainCamera;
        Vector3 m_Offset = new Vector3(0, 15f, -15f);
        float m_Speed = 2f;

        public void Init(Transform follow) {
            m_Follow = follow;
            m_MainCamera = Camera.main;

        }

        public void Release() {
            m_Follow = null;
            m_MainCamera = null;
        }

        public void Update(float elapseSeconds, float realElapseSeconds) {

            if (m_Follow == null) {
                return;
            }

            m_MainCamera.transform.position = Vector3.Lerp(m_MainCamera.transform.position,
            m_Follow.position + m_Offset, m_Speed * elapseSeconds);

        }

    }

}