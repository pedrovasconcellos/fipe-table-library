using System;
using System.Collections.Generic;
using System.Text;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeReference
    {
        public FipeReference() { }

        public FipeReference(int fipeReferenceId, string dateString, DateTime referenceDate) 
        {
            this.Id = fipeReferenceId;
            this.DateString = dateString;
            this.ReferenceDate = referenceDate;
        }

        public int Id { get; set; }
        public string DateString { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}
