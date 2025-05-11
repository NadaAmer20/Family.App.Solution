using Family.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Specifications.ClanSpecifications
{
    public class ClanWithBranchesSpecification : BaseSpecification<Clan>
    {
        public ClanWithBranchesSpecification() : base()
        {
            AddInclude(x => x.Branches);
            AddInclude("Branches.Persons");
        }

        public ClanWithBranchesSpecification(int clanId)
            : base(c => c.Id == clanId)
        {
            AddInclude(x => x.Branches);
            AddInclude("Branches.Persons");
        }

    }
}
