using Family.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Specifications.PersonSpecifications
{
    public class PersonWithDetailsSpecification : BaseSpecification<Person>
    {
        public PersonWithDetailsSpecification()
    : base()
        {
            AddInclude(x => x.Clan);
            AddInclude(x => x.Branch);
            AddInclude(x => x.Notifications);
        }
        public PersonWithDetailsSpecification(List<int> branchIds)
    : base(x => branchIds.Contains(x.BranchId))
        {
            AddInclude(x => x.Notifications);
            AddInclude(x => x.Clan);
            AddInclude(x => x.Branch);
        }

        public PersonWithDetailsSpecification(int personId)
            : base(x => x.Id == personId)
        {
            AddInclude(x => x.Clan);
            AddInclude(x => x.Branch);
            AddInclude(x => x.Notifications);
        }

        public PersonWithDetailsSpecification(int branchId, bool filterByBranch)
            : base(x => x.BranchId == branchId)
        {
            AddInclude(x => x.Notifications);
            AddInclude(x => x.Clan);
            AddInclude(x => x.Branch);
        }

    }
}
