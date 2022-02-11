using Networking;
using Networking.Client;

namespace Project.Units.Client
{
    public class ClientUnit : UnitBase
    {
        public ClientPlayer player { get; private set; }
        public bool isMine { get; private set; }
        public ClientUnitStateReceiver unitStateReceiver { get; private set; }
        
        public void Initialize(ClientPlayer ownerPlayer)
        {
            isMine = ownerPlayer == NetInfo.minePlayer;
            player = ownerPlayer;
            DontDestroyOnLoad(gameObject);
            
#if UNITY_EDITOR
            name = isMine ? "MineUnit" : $"ClientUnit [ID {ownerPlayer.playerId}]: {ownerPlayer.nickname}";
#endif
        }

        public override void AddUnitComponents()
        {
            base.AddUnitComponents();
            unitStateReceiver = AddUnitComponent<ClientUnitStateReceiver>();
            
            if (isMine)
            {
                AddUnitComponent<ClientMineUnitStateSender>();
            }
        }
        
        
        
    }
}

