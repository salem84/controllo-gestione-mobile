using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlloGestione.Models
{
    public class CtrlGestionePage
    {
        public List<DateTime> Orari { get; set; }
        public Riepilogo RiepilogoSettimana { get; set; }
        

        public CtrlGestionePage()
        {
            Orari = new List<DateTime>();
            RiepilogoSettimana = new Riepilogo();
        }
    }

    public class Riepilogo
    {
        public TimeSpan OreDaContratto { get; set; }
        public TimeSpan OreLavorate { get; set; }
        public TimeSpan OreDaLavorare { get; set; }
        public TimeSpan OreAggiuntive { get; set; }
    }
}