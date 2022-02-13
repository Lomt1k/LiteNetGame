
namespace Networking
{
    public abstract class BasePlayer
    {
        public int playerId { get; protected set; }
        public virtual int ping { get; protected set; }
        public string nickname { get; protected set; }

        public virtual void OnDisconnect()
        {
        }
        
    }
}
