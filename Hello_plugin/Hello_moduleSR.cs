using Module;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace Hello_plugin
{
    public class Hello_moduleSR : IModulo
    {
        public string Name => "Hello_modulo";

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
