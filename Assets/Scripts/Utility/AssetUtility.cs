namespace CatPaw
{
    public static class AssetUtility
    {
        public static string GetWeaponAsset(string assetName) {

            return string.Format("Assets/Entities/Weapon/{0}.prefab", assetName);
        }

        public static string GetCharactorAsset(string assetName) {
            return string.Format("Assets/Entities/Charactors/{0}.prefab", assetName);
        }

        public static string GetTerrainAsset(string assetName)
        {
            return string.Format("Assets/Entities/Terrain/{0}.prefab", assetName);
        }

        public static string GetTerrainDataAsset(string assetName)
        {
            return string.Format("Assets/Datas/Terrain/{0}.txt", assetName);
        }

        public static string GetConfigAsset(string assetName)
        {
            return string.Format("Assets/Datas/Config/{0}.txt", assetName);
        }

        public static string GetDataTableAsset(string assetName)
        {
            return string.Format("Assets/Datas/DataTable/{0}.txt", assetName);
        }

        public static string GetDictionaryAsset(string assetName)
        {
            return string.Format("Assets/Datas/Localization/{0}/Dictionaries/{1}.xml", GameEntry.Localization.Language.ToString(), assetName);
        }

        public static string GetFontAsset(string assetName)
        {
            return string.Format("Assets/Datas/Localization/{0}/Fonts/{1}.ttf", GameEntry.Localization.Language.ToString(), assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return string.Format("Assets/Scenes/{0}.unity", assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return string.Format("Assets/Audios/Music/{0}.mp3", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return string.Format("Assets/Audios/Sound/{0}.wav", assetName);
        }
        public static string GetUISoundAsset(string assetName)
        {
            return string.Format("Assets/Audios/Sound/UISound/{0}.wav", assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return string.Format("Assets/Entities/{0}.prefab", assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return string.Format("Assets/Entities/UI/UIForm/{0}.prefab", assetName);
        }


    }
}
