using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public interface IModulo
    {
        string Name { get; }
        string Description { get; }
        float Version { get; }
        void Start();
        void Stop();
    }
}
