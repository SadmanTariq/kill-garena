﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KillGarena
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)  
{
            if (Environment.UserInteractive)
            {
                KillGarena service1 = new KillGarena();
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new KillGarena()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
