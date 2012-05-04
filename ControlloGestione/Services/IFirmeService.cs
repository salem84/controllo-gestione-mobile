using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlloGestione.Services
{
    public interface IFirmeService
    {
        bool Autentica(string username, string password);

        string ReadCtrlGestionePage();

        string Firma();
    }
}
