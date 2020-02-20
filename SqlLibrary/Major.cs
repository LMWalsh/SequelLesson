using System;
using System.Collections.Generic;
using System.Text;

namespace SqlLibrary {
    public class Major {

        public int Id { get; set; }
        public string Description { get; set; }
        public int MinSat { get; set; }

        public override string ToString() {
            return $"{Id}|{Description}|{MinSat}";
        }
    }
}
