using System;
using System.Collections.Generic;
using System.Text;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeReference
    {
        public FipeReference() { }

        public FipeReference(int fipeReferenceId, DateTime referenceDate) 
        {
            this.Id = fipeReferenceId;
            this.ReferenceDate = referenceDate;
        }

        public int Id { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
