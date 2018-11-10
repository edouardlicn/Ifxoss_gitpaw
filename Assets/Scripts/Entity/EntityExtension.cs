using GameFramework;
using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace CatPaw
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        static DRTerrain GetDRTerrainByMapUnit(string mapUnit)
        {
            var arr = GameEntry.DataTable.GetDataTable<DRTerrain>().GetAllDataRows();
            foreach(var dr in arr)
            {
                if(dr.MapUnit == mapUnit)
                {
                    return dr;
                }
            }
            throw new GameFrameworkException("Not found mapunit '" + mapUnit + "' from datatable<" + typeof(DRTerrain).Name + ">.");
        }

        public static void ShowCharactor(this EntityComponent entityComponent, string charactorKey, CampType campType) {

            DRCharactor_Base[] arr = GameEntry.DataTable.GetDataTable<DRCharactor_Base>().GetAllDataRows();
            DRCharactor_Base drCB = null;
            foreach(var dr in arr) {
                if(dr.Key == charactorKey) {
                    drCB = dr;
                    break;
                }
            }
            if(drCB == null) {
                throw new GameFrameworkException("Not found charactor key[" + charactorKey + "] from datatable<" + typeof(DRCharactor_Base).Name + ">.");
            }

            int entityId = entityComponent.GenerateSerialId();
            string assetName = AssetUtility.GetCharactorAsset(charactorKey);

            CharactorData data = new CharactorData(entityId, drCB.Id, campType, charactorKey);
            entityComponent.ShowEntity(entityId, typeof(Charactor), assetName, "Charactor", Constant.AssetPriority.CharactorAsset, data);
        }

        public static void ShowWeapon(this EntityComponent entityComponent, string weaponType, int ownerId) {

            DRCharactor_Weapon[] arr = GameEntry.DataTable.GetDataTable<DRCharactor_Weapon>().GetAllDataRows();
            DRCharactor_Weapon drCW = null;
            foreach(var dr in arr) {
                if(dr.WeaponType == weaponType) {
                    drCW = dr;
                    break;
                }
            }
            if(drCW == null) {
                throw new GameFrameworkException("Not found weaponType[" + weaponType + "] from datatable<" + typeof(DRCharactor_Weapon).Name + ">.");
            }

            int entityId = entityComponent.GenerateSerialId();
            string assetName = AssetUtility.GetWeaponAsset(weaponType);
            WeaponData data = new WeaponData(entityId, drCW.Id, ownerId);
            entityComponent.ShowEntity(entityId, typeof(Weapon), assetName, "Weapon", Constant.AssetPriority.WeaponAsset, data);

        }

        public static void ShowBullet(this EntityComponent entityComponent, BulletData data)
        {
            int entityId = entityComponent.GenerateSerialId();
            string assetName = AssetUtility.GetWeaponAsset("球体");
            entityComponent.ShowEntity(entityId, typeof(Bullet), assetName, "Bullet", Constant.AssetPriority.BulletAsset, data);
        }

        public static void ShowTerrainBrick<T>(this EntityComponent entityComponent, string mapUnit, int x, int y, int z) where T : TerrainBrick
        {
            var drTerrain = GetDRTerrainByMapUnit(mapUnit);
            var entityId = entityComponent.GenerateSerialId();
            entityComponent.ShowEntity(entityId, typeof(T), AssetUtility.GetTerrainAsset(drTerrain.AssetName), 
            "TerrainBrick", Constant.AssetPriority.TerrainBrickAsset, new TerrainBrickData(entityId, drTerrain.Id, x, y, z));
        }

        public static void ShowTerrainCube<T>(this EntityComponent entityComponent, string mapUnit, int x, int y, int z) where T : TerrainCube
        {
            var drTerrain = GetDRTerrainByMapUnit(mapUnit);
            var entityId = entityComponent.GenerateSerialId();
            entityComponent.ShowEntity(entityId, typeof(T), AssetUtility.GetTerrainAsset(drTerrain.AssetName), 
            "TerrainCube", Constant.AssetPriority.TerrainCubeAsset, new TerrainCubeData(entityId, drTerrain.Id, x, y, z));
        }

        public static void ShowEffect(this EntityComponent entityComponent, EffectData data)
        {
            entityComponent.ShowEntity(typeof(Effect), "Effect", Constant.AssetPriority.EffectAsset, data);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity<{1}> id '{0}' from data table.", data.TypeId.ToString(), logicType.Name);
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
