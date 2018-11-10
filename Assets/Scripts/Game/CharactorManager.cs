using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace CatPaw {

    public sealed class CharactorManager {

        Action<Charactor> m_SuccessCallback;
        Charactor m_Charactor;

        public void Load(string charactorKey, CampType campType, Action<Charactor> successCallback) {

            m_SuccessCallback = successCallback;
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, ShowCharactorSuccessCallback);
            GameEntry.Entity.ShowCharactor(charactorKey, campType);
        }

        public void Unload(Charactor charactor) {

            if (charactor == null) {
                return;
            }

            if (charactor.Weapon != null) {
                GameEntry.Entity.HideEntity(charactor.Weapon);
            }

            GameEntry.Entity.HideEntity(charactor);

        }


        void ShowCharactorSuccessCallback(object sender, GameEventArgs e) {

            ShowEntitySuccessEventArgs args = e as ShowEntitySuccessEventArgs;

            if (args == null) {
                return;
            }

            m_Charactor = args.Entity.Logic as Charactor;

            if (m_Charactor == null) {
                throw new GameFrameworkException("Show charactor entity failed.");
            }

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, ShowCharactorSuccessCallback);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, ShowWeaponSuccessCallback);
            GameEntry.Entity.ShowWeapon(m_Charactor.WeaponType, m_Charactor.Id);
        }

        void ShowWeaponSuccessCallback(object sender, GameEventArgs e) {

            ShowEntitySuccessEventArgs args = e as ShowEntitySuccessEventArgs;

            if (args == null) {
                return;
            }

            Weapon weapon = args.Entity.Logic as Weapon;

            if(weapon == null) {

                throw new GameFrameworkException("Show weapon entity failed.");
            }

            //GameEntry.Entity.AttachEntity(weapon.Id, m_Charactor.Id);

            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, ShowWeaponSuccessCallback);

            Charactor c = m_Charactor;
            m_Charactor = null;

            if (m_SuccessCallback != null) {
                var handler = m_SuccessCallback;
                m_SuccessCallback = null;
                handler(c);
            }

        }


    }




}