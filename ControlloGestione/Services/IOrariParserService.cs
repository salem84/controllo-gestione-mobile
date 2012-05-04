using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlloGestione.Models;

namespace ControlloGestione.Services
{
    public interface IOrariParserService
    {
        CtrlGestionePage ReadHours(string rawHtml);
    }
}
