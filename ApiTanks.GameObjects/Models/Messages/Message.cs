using Newtonsoft.Json.Linq;

namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class Message
    {
        public virtual JToken GetMessageAsJToken ()
        {
            return JToken.FromObject(this);
        }
    }

}