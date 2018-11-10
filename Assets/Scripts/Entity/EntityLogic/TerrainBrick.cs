using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using UnityGameFramework.Runtime;
using GameFramework;

namespace CatPaw {
    public abstract class TerrainBrick : TerrainBase {
        [SerializeField]
        private TerrainBrickData m_TerrainBrickData = null;

        protected override void OnShow(object userData) {
            base.OnShow(userData);

            m_TerrainBrickData = userData as TerrainBrickData;
            if (m_TerrainBrickData == null) {
                throw new GameFrameworkException();
            }

        }

        protected override void OnHide(object userData) {
            m_TerrainBrickData = null;

            base.OnHide(userData);

        }


        void OnTriggerEnter(Collider other) {
            Charactor charactor = other.GetComponent<Charactor>();
            if (charactor != null) {
                OnCharactorEnter(charactor);
            }
        }

        void OnTriggerStay(Collider other) {
            Charactor charactor = other.GetComponent<Charactor>();
            if (charactor != null) {
                OnCharactorStay(charactor);
            }
        }

        void OnTriggerExit(Collider other) {
            Charactor charactor = other.GetComponent<Charactor>();
            if (charactor != null) {
                OnCharactorExit(charactor);
            }
        }

        protected virtual void OnCharactorEnter(Charactor charactor) {

        }

        protected virtual void OnCharactorStay(Charactor charactor) {

        }

        protected virtual void OnCharactorExit(Charactor charactor) {

        }

    }
}
