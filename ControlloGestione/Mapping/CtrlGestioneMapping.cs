using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControlloGestione.Models;
using ControlloGestione.ViewModel;

namespace ControlloGestione.Mapping
{
    public static class CtrlGestioneMapping
    {
        public static CtrlGestioneInfoVM ConvertToCtrlGestioneInfoVM(this CtrlGestionePage model)
        {
            var vm = new CtrlGestioneInfoVM();
            int count = 1;
            foreach (var orario in model.Orari)
            {
                switch (count)
                {
                    case 1:
                        vm.Mattina1 = orario;
                        break;
                    case 2:
                        vm.Mattina2 = orario;
                        break;
                    case 3:
                        vm.Pomeriggio1 = orario;
                        break;
                    case 4:
                        vm.Pomeriggio2 = orario;
                        break;
                }
            }

            if (model.RiepilogoSettimana.OreLavorate > model.RiepilogoSettimana.OreDaContratto)
                vm.OreMinimeSettimanaliRaggiunto = true;
            else
                vm.OreMinimeSettimanaliRaggiunto = false;

            vm.OreLavorate = model.RiepilogoSettimana.OreLavorate;
            vm.OreStraordinario = model.RiepilogoSettimana.OreAggiuntive;
            vm.OreDaFare = model.RiepilogoSettimana.OreDaLavorare;

            return vm;
        }
    }
}