using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ControlloGestione.ViewModel
{
    public class CtrlGestioneInfoVM
    {
        [DataType(DataType.Time)]
        public DateTime? Mattina1 { get; set; }

        [DataType(DataType.Time)]
        public DateTime? Mattina2 { get; set; }

        [DataType(DataType.Time)]
        public DateTime? Pomeriggio1 { get; set; }

        [DataType(DataType.Time)]
        public DateTime? Pomeriggio2 { get; set; }

        #region GIORNO

        public TimeSpan OreGiornaliere
        {
            get
            {
                // Ho firmato la mattina e Mattina2 contiene o l'ora attuale o l'ora di pranzo
                if (Mattina1.HasValue && Mattina2.HasValue)
                {
                    TimeSpan oreMattina = Mattina2.Value - Mattina1.Value;

                    // Ho firmato la pausa pranzo e ho l'ora attuale in Pomeriggio2
                    if (Pomeriggio1.HasValue && Pomeriggio2.HasValue)
                    {
                        TimeSpan orePomeriggio = Pomeriggio2.Value - Pomeriggio1.Value;

                        TimeSpan oreTotali = oreMattina + orePomeriggio;
                        return oreTotali;
                    }

                    return oreMattina;
                }
                else
                {
                    var diff = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0) - new TimeSpan(Mattina1.Value.Hour, Mattina1.Value.Minute, 0);
                    return diff;
                }
            }
        }

        public bool OreMinimeGiornalieriRaggiunto
        {
            get
            {
                if (TimeSpan.Compare(OreGiornaliere, new TimeSpan(7, 42, 0)) >= 0)
                    return true;
                else
                    return false;
            }
        }

        public DateTime? UltimaFirma
        {
            get
            {
                return Pomeriggio2 ?? Pomeriggio1 ?? Mattina2 ?? Mattina1;
            }
        }

        #endregion

        #region SETTIMANA

        public bool OreMinimeSettimanaliRaggiunto { get; set; }

        public TimeSpan OreDaFare { get; set; }
        public TimeSpan OreDaFareEffettive
        {
            get
            {
                return OreDaFare - OreGiornaliere;
            }
        }
        public TimeSpan OreLavorate { get; set; }
        public TimeSpan OreStraordinario { get; set; }

        #endregion
    }
}