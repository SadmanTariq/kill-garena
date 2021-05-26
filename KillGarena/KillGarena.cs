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

            if (!EventLog.SourceExists("KillGarena service"))
            {
                EventLog.CreateEventSource(
                    "KillGarena service", "KillGarena");
            }

            eventLog1 = new EventLog
            {
                Source = "KillGarena service",
                Log = "KillGarena"
            };
        }

        protected override void OnStart(string[] args)
        {
            GpServiceController = new ServiceController("GarenaPlatform");

            Log("Started.", EventLogEntryType.Information);

            Timer timer = new Timer();
            timer.Interval = 30000;  // 30 seconds
            timer.Elapsed += OnTimer;
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs args)
        {
            GpServiceController.Refresh();

            bool oldKill = Kill;
            // Service running but not the process.
            Kill = (GpServiceController.Status == ServiceControllerStatus.Running) && !GarenaProcessRunning();

            // Only kill if conditions match in both last and current poll.
            if (oldKill && Kill)
            {
                try
                {
                    GpServiceController.Stop();
                    ServiceHelper.ChangeStartMode(GpServiceController, ServiceStartMode.Disabled);
                    Log("Stopped Garena Platform Service.");
                }
                catch (InvalidOperationException e)
                {
                    Log("An error occurred while trying to stop Garena service.\n" + e.Message, EventLogEntryType.Error);
                }
            }
        }

        protected override void OnStop()
        {
            GpServiceController.Close();
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
