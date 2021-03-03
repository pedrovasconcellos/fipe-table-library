using System;
using System.Collections.Generic;
using System.Text;

namespace Vasconcellos.FipeTable.Types.Entities
{
    public class FipeReference
    {
        public FipeReference() { }

        public FipeReference(int fipeReferenceId, string dateString) 
        {
            this.Id = fipeReferenceId;
            this.DateString = dateString;
        }

        public int Id { get; set; }
        public string DateString { get; set; }
    }
}
