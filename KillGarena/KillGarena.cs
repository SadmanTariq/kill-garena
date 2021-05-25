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
            eventLog1.WriteEntry("Started.", EventLogEntryType.Information);

            Timer timer = new Timer();
            timer.Interval = 30000;  // 30 seconds
            timer.Elapsed += OnTimer;
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            eventLog1.WriteEntry("Timer", EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stopped.");
        }
    }
}
