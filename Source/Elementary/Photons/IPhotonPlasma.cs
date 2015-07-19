using System;
using System.Collections.Generic;
using System.Text;

namespace tainicom.Aether.Elementary.Photons
{
    public interface IPhotonPlasma
    {
        IList<IAether> VisibleParticles { get; }
    }
}
