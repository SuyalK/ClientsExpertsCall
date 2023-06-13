using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsExpertsCall.Model
{
    public class Expert
    {
        public List<Call> calls { get; set; }

        public string currency { get; set; }

        public decimal hourlyRate { get; set; }

        public string name { get; set;}
    }
}
