using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orochi.Protections.Mutations.Arithmetic.Utils
{
    public class Generator
    {
        private Random random;

        public Generator() => random = new Random(Guid.NewGuid().GetHashCode());

        public int Next() => random.Next(int.MaxValue);

        public int Next(int value) => random.Next(value);

        public int Next(int min, int max) => random.Next(min, max);
    }
}
