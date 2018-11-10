using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace CatPaw {

    [Serializable]
    public class WeaponData : EntityData {

        [SerializeField]
        int m_OwnerId;

        public WeaponData(int entityId, int typeId, int ownerId) : base(entityId, typeId) {
            m_OwnerId = ownerId;
        }

        public int OwnerId {
            get {
                return m_OwnerId;
            }
        }

    }

}