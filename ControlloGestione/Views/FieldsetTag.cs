using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControlloGestione.Helpers
{
    public class FieldsetTag : IDisposable
    {
        private bool disposed;
        private readonly TextWriter writer;

        public FieldsetTag(HtmlHelper helper, string title, string color = null)
        {
            this.writer = helper.ViewContext.Writer;
            if (string.IsNullOrEmpty(color))
            {
                writer.WriteLine("<fieldset>");
            }
            else
            {
                writer.WriteLine(string.Format("<fieldset style=\"color:{0}\">", color));
            }
            writer.WriteLine(string.Format("<legend>{0}</legend>", title));
        }

        public void Dispose()
        {
            if (disposed) return;

            disposed = true;

            writer.WriteLine("</fieldset>");
        }
    }

    public static class FieldsetTagExtensions
    {
        public static FieldsetTag BeginFieldset(this HtmlHelper self, string title, string color)
        {
            return new FieldsetTag(self, title, color);
        }

        public static FieldsetTag BeginFieldset(this HtmlHelper self, string title)
        {
            return new FieldsetTag(self, title);
        }
    }
}