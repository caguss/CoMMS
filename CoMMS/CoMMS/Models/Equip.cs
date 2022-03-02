using System;
using System.Collections.Generic;
using System.Text;

namespace CoMMS
{
    class Equip
    {
        private string statusString = "비가동";
        public string EquipName { get; set; }
        public string EquipCode { get; set; }
        public string Value { get; set; }
        public bool Status { get; set; }
        public string StatusString { get => statusString; set => statusString = value; }

        public string EquipColor { get; set; }
    }
}
