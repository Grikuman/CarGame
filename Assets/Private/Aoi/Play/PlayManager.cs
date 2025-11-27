using Fusion;
using NetWork;
using System.Linq;
using UnityEngine;

namespace Aoi
{
    public class PlayManager : MonoBehaviour
    {
        GameLauncher m_gameLauncher;
        [SerializeField] NetworkPrefabRef m_car;

        private void Start()
        {
            m_gameLauncher = GameLauncher.Instance;
            m_gameLauncher.OnPlayerJoined += SpownPlayer;
        }

        private void SpownPlayer(NetworkRunner runner,PlayerRef user)
        {
            if(runner.LocalPlayer == user)
            {
                runner.Spawn(m_car, new Vector3(runner.ActivePlayers.Count() * 10, 10, 0));
            }
        }
    } 
}
