using System.Collections.Generic;

namespace ApiTanks.GameObjects.Models
{
    public class Team
    {
        public string Id {get; set;}
        public int Points {get; set;}
        public Dictionary<string, Tank> Tanks {get; set;} 
            = new Dictionary<string, Tank>();
        private int MaxTanks {get; set;} = 5;
        private string Secret {get; set;}

        public Team (string id)
        {
            Id = id;
        }

        public Tank SpawnTank (string tankName)
        {
            if (Tanks.Count > MaxTanks || Tanks.ContainsKey(tankName))
            {
                return null;
            }
            tankName = $"{Id}_{tankName}";
            var newTank = new Tank(this, tankName);
            Tanks.Add(tankName, newTank);
            return newTank;
        }
    }

}