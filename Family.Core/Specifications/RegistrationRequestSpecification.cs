using Family.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Specifications
{
    public class RegistrationRequestSpecification : BaseSpecification<RegistrationRequest>
    {
        public RegistrationRequestSpecification(Expression<Func<RegistrationRequest, bool>> criteria)
            : base(criteria)
        {
        }
    }
}
