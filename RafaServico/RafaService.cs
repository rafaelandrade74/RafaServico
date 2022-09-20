using Module;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;

namespace RafaServico
{
    public partial class RafaService : ServiceBase
    {
        EventLog eventLog1 = new System.Diagnostics.EventLog();
        string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        Timer timer = new Timer();
        readonly List<IModulo> Modulos = new List<IModulo>();
        public RafaService()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("RafaService"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "RafaService", "AppLog");
            }
            eventLog1.Source = "RafaService";
            eventLog1.Log = "RafaService";
            timer.Interval = 10000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
        }
        private IEnumerable<Type> getDerivedTypesFor(Type baseType)
        {
            string pluginLocation = Path.GetFullPath(root);

            List<Type> types = new List<Type>();
            var arq = Directory.GetFiles(pluginLocation, "*moduleSR*.dll");
            eventLog1.WriteEntry($"Dlls encontradas: {arq.Length}", EventLogEntryType.Information);
            foreach (var modulo in arq)
            {
                eventLog1.WriteEntry($"Plugin encontrado: {modulo}", EventLogEntryType.Information);
                var assembly = Assembly.LoadFile(modulo);
                types.Add(assembly.GetTypes().Where(baseType.IsAssignableFrom).Where(t => baseType != t).FirstOrDefault());
                eventLog1.WriteEntry($"Foi adicionado {types.Count} plugin(s)", EventLogEntryType.Information);
            }

            return types;
        }

        public IModulo InstancePlugin(Type pluginIdentifier)
        {
            return (IModulo)Activator.CreateInstance(pluginIdentifier);
        }

        public void registerPlugins()
        {
            // Register plugins at application startup
            IEnumerable<Type> modulos = getDerivedTypesFor(typeof(IModulo));

            foreach (Type modulo in modulos)
            {
                Modulos.Add(InstancePlugin(modulo));
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                registerPlugins();
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(ex.Message);
            }

            eventLog1.WriteEntry($"In OnStart.", EventLogEntryType.Information);
            foreach (var modulo in Modulos)
            {
                if(modulo.Version >= 1.0)
                {
                    modulo.Start();
                }
                
                eventLog1.WriteEntry($"instance: {modulo.Name}", EventLogEntryType.Information);
            }
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            eventLog1.WriteEntry($"Timer run {e.SignalTime}.", EventLogEntryType.Warning, 1, 100, null);

        }

        protected override void OnStop()
        {
            foreach (var plgin in Modulos)
            {
                plgin.Stop();
            }
            timer.Stop();
        }
    }
}
