using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RafaServico
{
    public interface Plugin
    {
        string Name { get; }
        string Description { get; }
        void start();
        void stop();
    }
}
