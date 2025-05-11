using Family.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Specifications.PhotoSpecifications
{
    public class PhotoSpecification : BaseSpecification<Photo>
    {
        public PhotoSpecification() : base()
        {
        }

        public PhotoSpecification(int id)
            : base(x => x.Id == id)
        {
        }
    }
}
