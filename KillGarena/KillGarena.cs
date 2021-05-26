using System;
using System.Diagnostics;
using System.ServiceProcess;
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
        }

        protected override void OnStart(string[] args)
        {
            GpServiceController = new ServiceController("GarenaPlatform");

            Debug.WriteLine("Started.");

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
                    Debug.WriteLine("Stopped Garena Platform Service.");
                }
                catch (InvalidOperationException e)
                {
                    Debug.WriteLine("An error occurred while trying to stop Garena service.\n" + e.Message);
                }
            }
        }

        protected override void OnStop()
        {
            GpServiceController.Close();
            Debug.WriteLine("Stopped.");
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
    }
}
