using Module;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Teste_moduleSR
{
    public class Teste_moduleSR : IModulo
    {
        public string Name => "Teste_modulo";

        public string Description => "essa uma desc";

        public float Version => 1.0f;

        Thread task;
        public void Start()
        {
            task = new Thread(ExecutaTarefa);
            task.Start();
        }
        void ExecutaTarefa()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));


                using (StreamWriter sw = File.AppendText($@"C:\Users\xrafa\Desktop\{Name}.txt"))
                {
                    sw.WriteLine($"[{DateTime.Now}] - Rodando");
                }


            }
        }
        public void Stop()
        {
            task.Abort();
        }
    }
}
