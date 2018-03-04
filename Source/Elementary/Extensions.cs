using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tainicom.Aether.Elementary
{
    public static class Extensions
    {
        public static T GetComponent<T>(this IAether element) where T : class
        {
            T result = null;
            try { return (T)element; }
            catch (InvalidCastException ice) { }

            return result;
        }
    }
}
