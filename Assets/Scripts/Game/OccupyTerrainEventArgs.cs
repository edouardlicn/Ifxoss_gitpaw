using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace CatPaw {

    public sealed class OccupyTerrainEventArgs : GameEventArgs {

        public static readonly int EventId = typeof(OccupyTerrainEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public CampType From {
            get;
            private set;
        }

        public CampType To {
            get;
            private set;

        }

        public override void Clear() {
            To = default(CampType);
            From = default(CampType);
        }



        public OccupyTerrainEventArgs Fill(CampType from, CampType to)
        {
            From = from;
            To = to;

            return this;
        }
    }
}