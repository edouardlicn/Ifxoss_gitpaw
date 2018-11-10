using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CatPaw {

    [Serializable]
    public class CharactorData : EntityData {

        [SerializeField]
        CampType m_OwnerCampType;

        [SerializeField]
        float m_MoveSpeed;

        [SerializeField]
        string m_Key;

        [SerializeField]
        int m_Armor;
        [SerializeField]
        string m_WeaponType;
        [SerializeField]
        int m_BulletMax;
        [SerializeField]
        int m_CoolDown;
        [SerializeField]
        int m_Damage;
        [SerializeField]
        string m_Logic;


        public CharactorData(int entityId, int typeId, CampType campType, string key) : base(entityId, typeId) {

            m_OwnerCampType = campType;
            m_MoveSpeed = 8f;
            m_Key = key;

            DRCharactor_Ability[] arr = GameEntry.DataTable.GetDataTable<DRCharactor_Ability>().GetAllDataRows();
            DRCharactor_Ability drCA = null;
            foreach (var dr in arr) {
                if (dr.Key == key) {
                    drCA = dr;
                    break;
                }
            }

            if (drCA == null) {
                return;
            }

            m_Armor = drCA.Armor;
            m_WeaponType = drCA.WeaponType;
            m_BulletMax = drCA.BulletMax;
            m_CoolDown = drCA.CoolDown;
            m_Damage = drCA.Damage;
            m_Logic = drCA.Logic;

        }

        public int Armor {
            get {
                return m_Armor;
            }
        }

        public string WeaponType {
            get {
                return m_WeaponType;
            }
        }

        public int BulletMax {
            get {
                return m_BulletMax;
            }
        }

        public int CoolDown {
            get {
                return m_CoolDown;
            }
        }

        public int Damage {
            get {
                return m_Damage;
            }
        }

        public string Logic {
            get {
                return m_Logic;
            }
        }

        public string Key {
            get {
                return m_Key;
            }
        }

        public CampType OwnerType {
            get {
                return m_OwnerCampType;
            }
        }

        public float MoveSpeed {
            get {
                return m_MoveSpeed;
            }
        }

    }
}