using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMVVM
{
    public class Personne
    {
        public string Nom { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return Nom + ", " + Age + " ans.";
        }
    }
}
