using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tainicom.Aether.Elementary.Radiums
{
    public interface IRadium<T>:IAether where T: struct
    {
        T Value { get; set; }
    }
}
