using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Models
{
    public class TeamModel
    {
        public int TeamId { get; set; }
        public string Name { get; set; }

        public TeamModel(Team team)
        {
            this.TeamId = team.TeamId;
            this.Name = team.Name;
        }
    }
}
