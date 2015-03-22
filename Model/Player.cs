using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Player
    {
        public string Name { get; set; }
        public int EventCount { get; set; }
        public bool IsInPm { get; set; }
        public bool IsInMelee { get; set; }
        public bool IsInS4 { get; set; }
        public bool IsInPmDoubles { get; set; }
        public bool IsInMeleeDoubles { get; set; }
        public bool IsInS4Doubles { get; set; }
        public string PmPartner { get; set; }
        public string MeleePartner { get; set; }
        public string S4Partner { get; set; }
    }
}
