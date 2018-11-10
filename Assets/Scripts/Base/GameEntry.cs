using UnityEngine;
using UnityEngine.SceneManagement;
namespace CatPaw
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
            //DontDestroyOnLoad(this);
        }
    }
}
