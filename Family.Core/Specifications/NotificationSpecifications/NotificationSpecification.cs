using Family.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Specifications.NotificationSpecifications
{
    public class NotificationSpecification : BaseSpecification<Notifications>
    {
        public NotificationSpecification() : base()
        {
            AddInclude(x => x.Person);
        }

        public NotificationSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Person);
        }

        public NotificationSpecification(int personId, bool forPerson)
            : base(x => x.PersonId == personId.ToString())
        {
            AddInclude(x => x.Person);
        }

    }
}
