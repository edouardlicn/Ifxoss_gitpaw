using GameFramework.DataTable;
using System.Collections.Generic;

namespace CatPaw
{
    public class DRTerrain : IDataRow
    {
        public int Id
        {
            get;
            protected set;
        }

        public string MapUnit
        {
            get;
            private set;
        }

        public bool CanMovePass {
            get;
            private set;
        }

        public string AssetName
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index]);
            index++;
            MapUnit = text[index];
            index++;
            CanMovePass = bool.Parse(text[index]);
            index++;
            index++;
            AssetName = text[index];
        }


        private void AvoidJIT()
        {
            new Dictionary<int, DRTerrain>();
        }
    }
}
