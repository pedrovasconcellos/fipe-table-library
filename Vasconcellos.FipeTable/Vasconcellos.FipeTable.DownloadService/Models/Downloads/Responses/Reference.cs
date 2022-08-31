
using System;
using System.Collections.Generic;

namespace Vasconcellos.FipeTable.DownloadService.Models.Responses
{
    public class Reference
    {
        public int Codigo { get; set; }
        public string Mes { get; set; }
        public DateTime ReferenceDate => GetReferenceDate();

        private DateTime GetReferenceDate()
        {
            try
            {
                var arrayDate = Mes.Split("/");
                var referenceDate =  new DateTime(
                    Convert.ToInt32(arrayDate[1]),
                    summaryDates.GetValueOrDefault(arrayDate[0]), 1).Date;
                return referenceDate;
            }
            catch
            {
                throw new ArgumentNullException("Reference date not found!");
            }
        }

        private static IReadOnlyDictionary<string, int> summaryDates = new Dictionary<string, int>()
        {
            { "janeiro", 1 },
            { "fevereiro", 2 },
            { "março", 3 },
            { "abril", 4 },
            { "maio", 5 },
            { "junho", 6 },
            { "julho", 7 },
            { "agosto", 8 },
            { "setembro", 9 },
            { "outubro", 10 },
            { "novembro", 11 },
            { "dezembro", 12 },
        };
    }
}
