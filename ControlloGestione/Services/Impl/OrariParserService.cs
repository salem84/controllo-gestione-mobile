using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ControlloGestione.Models;
using ControlloGestione.Utility;

namespace ControlloGestione.Services.Impl
{
    public class OrariParserService : IOrariParserService
    {
        public CtrlGestionePage ReadHours(string rawHtml)
        {
            var modelPage = new CtrlGestionePage();

            //<span id="objFormSigns_lblH1" class="labelSign">16.34</span>
            Regex regexHour = new Regex("<span id=\"objFormSigns_lblH\\d\"[^>]*>(.*?)</span>", RegexOptions.IgnoreCase);

            Match m = regexHour.Match(rawHtml);
            while (m.Success)
            {
                string s_ora = m.Groups[1].Value;

                DateTime ora;
                if (DateTime.TryParse(s_ora, out ora))
                {
                    modelPage.Orari.Add(ora);
                }
                m = m.NextMatch();
            }

            Regex regexOreDovute = new Regex("<span id=\"objHoursInThisWeek_lblWeekHDovute\"[^>]*>(.*?)</span>", RegexOptions.IgnoreCase);
            Match m2 = regexOreDovute.Match(rawHtml);
            if (m2.Success)
            {
                string s_tspan = m2.Groups[1].Value;
                TimeSpan tspan = s_tspan.ToTimeSpan();

                modelPage.RiepilogoSettimana.OreDaContratto = tspan;

            }

            Regex regexOreLavorate = new Regex("<span id=\"objHoursInThisWeek_lblWeekHWorked\"[^>]*>(.*?)</span>", RegexOptions.IgnoreCase);
            Match m3 = regexOreLavorate.Match(rawHtml);
            if (m3.Success)
            {
                string s_tspan = m3.Groups[1].Value;
                TimeSpan tspan = s_tspan.ToTimeSpan();

                modelPage.RiepilogoSettimana.OreLavorate = tspan;
            }


            Regex regexOreDaLavorare = new Regex("<span id=\"objHoursInThisWeek_lblWeekDaLavorare\"[^>]*><font[^>]*>(.*?)</font></span>", RegexOptions.IgnoreCase);
            Match m4 = regexOreDaLavorare.Match(rawHtml);
            if (m4.Success)
            {
                string s_tspan = m4.Groups[1].Value;
                TimeSpan tspan = s_tspan.ToTimeSpan();

                modelPage.RiepilogoSettimana.OreDaLavorare = tspan;
            }

            Regex regexOreAggiuntive = new Regex("<span id=\"objHoursInThisWeek_lblWeekLavorateInPiu\"[^>]*><font[^>]*>(.*?)</font></span>", RegexOptions.IgnoreCase);
            Match m5 = regexOreAggiuntive.Match(rawHtml);
            if (m5.Success)
            {
                string s_tspan = m5.Groups[1].Value;
                TimeSpan tspan = s_tspan.ToTimeSpan();

                modelPage.RiepilogoSettimana.OreAggiuntive = tspan;
            }

            return modelPage;
        }
    }
}