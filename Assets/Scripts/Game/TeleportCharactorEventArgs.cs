using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace CatPaw {
    public sealed class TeleportCharactorEventArgs : GameEventArgs {

        public static readonly int EventId = typeof(TeleportCharactorEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int CharactorId {
            get;
            private set;
        }

        public int FromTerrainBrickId {
            get;
            private set;
        }

        public override void Clear() {
            CharactorId = default(int);
            FromTerrainBrickId = default(int);
        }

         public TeleportCharactorEventArgs Fill(int charactorId, int fromTerrainBrickId)
        {
            CharactorId = charactorId;
            FromTerrainBrickId = fromTerrainBrickId;

            return this;
        }

    }

}

