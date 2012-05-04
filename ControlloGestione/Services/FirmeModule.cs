using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using ControlloGestione.Services.Impl;

namespace ControlloGestione.Services
{
    public class FirmeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new FirmeService()).As<IFirmeService>();
            builder.Register(c => new OrariParserService()).As<IOrariParserService>();
        }
    }
}