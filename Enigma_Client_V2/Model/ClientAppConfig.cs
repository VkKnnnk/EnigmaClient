using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Enigma_Client_V2.Model
{
    public static class ClientAppConfig
    {
        public static List<double> timesWhenUpdateTariffs { get; set; }
        public static Computer current_PC { get; set; }
        public static int personalDiscount { get; set; }
        public static Window current_window { get; set; }
        public static bool demoStart { get; set; }
        public static string lastError { get; set; }
    }
}
