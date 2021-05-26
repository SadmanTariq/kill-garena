using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KillGarena
{
    public partial class KillGarena : ServiceBase
    {
        private ServiceController GpServiceController;
        private bool Kill = false;

        public KillGarena()
        {
            InitializeComponent();

            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("KillGarena service"))
            {
                EventLog.CreateEventSource(
                    "KillGarena service", "KillGarena");
            }
            eventLog1.Source = "KillGarena service";
            eventLog1.Log = "KillGarena";
        }

        protected override void OnStart(string[] args)
        {
            GpServiceController = new ServiceController("GarenaPlatform");

            Log("Started.", EventLogEntryType.Information);

            Timer timer = new Timer();
            timer.Interval = 5000;  // 30 seconds
            //timer.Interval = 30000;  // 30 seconds
            timer.Elapsed += OnTimer;
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            eventLog1.WriteEntry("Timer", EventLogEntryType.Information);
            if (GpServiceController.Status == ServiceControllerStatus.Running)
            {
                Log("Garena service running.");
            }
            if (GarenaProcessRunning())
            {
                Log("Garena process running.");
            }
        }

        protected override void OnStop()
        {
            Log("Stopped.");
        }

        private bool GarenaProcessRunning()
        {
            string pname = "Garena";
            return Process.GetProcessesByName(pname).Length > 0;
        }

        internal void TestStartupAndStop(string[] args)
        {
            OnStart(args);
            Console.ReadLine();
            OnStop();
        }  

        private void Log(string message)
        {
            Log(message, EventLogEntryType.Information);
        }
        private void Log(string message, EventLogEntryType type)
        {
            eventLog1.WriteEntry(message, type);
            Debug.WriteLine(message);
        }
    }
}
