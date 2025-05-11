using Family.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Specifications.SliderSpecifications
{
    public class SliderItemSpecification : BaseSpecification<SliderItem>
    {
        public SliderItemSpecification() : base()
        {
        }

        public SliderItemSpecification(int id)
            : base(x => x.Id == id)
        {
        }
    }
}
