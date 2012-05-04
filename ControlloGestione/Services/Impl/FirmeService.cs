using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ControlloGestione.Services;

namespace ControlloGestione.Services.Impl
{
    public class FirmeService : IFirmeService
    {
        private CookieContainer _container;
        private string ViewState;
        private string EventValidation;

        public string Domain { get; set; }
        public string Site { get; set; }

        public FirmeService()
        {
            Domain = ConfigurationManager.AppSettings["Domain"];
            Site = ConfigurationManager.AppSettings["Site"];
        }

        public bool Autentica(string username, string password)
        {
            try
            {
                var logonUrl = string.Format("http://{0}/CookieAuth.dll?Logon", Site);
                _container = new CookieContainer();

                //prima richiesta
                var request = (HttpWebRequest)WebRequest.Create(logonUrl);
                request.CookieContainer = _container;
                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";

                var stream = request.GetRequestStream();

                string completeUsername = string.Format("{0}%5C{1}", Domain, username);
                var postData = string.Format("curl=Z2F&flags=0&forcedownlevel=0&formdir=3&username={0}&password={1}&SubmitCreds=Accedi", completeUsername, password);
                using (var sw = new StreamWriter(stream))
                {
                    sw.Write(postData);
                }

                var rs = request.GetResponse() as HttpWebResponse;
                foreach (Cookie c in rs.Cookies)
                    _container.Add(c);

                return true;
            }
            catch(Exception fault)
            {
                throw new Exception("Errore Autenticazione", fault);
            }
        }

        public string ReadCtrlGestionePage()
        {
            var url = string.Format("http://{0}/ctrlgestione", Site);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = _container;

            var response = request.GetResponse().GetResponseStream();
            Encoding enc = Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new StreamReader(response, enc);

            string txt = loResponseStream.ReadToEnd();

            Regex r = new Regex("<input[\\t ]+type=\"hidden\"[\\t ]+name=\"__VIEWSTATE\"[\\t ]+id=\"__VIEWSTATE\" value=\"(?<viewstate>[^\"]+)\"");
            Match m = r.Match(txt);
            if (m.Success)
            { 
                ViewState = m.Groups["viewstate"].Value; 
            }

            r = new Regex("<input[\\t ]+type=\"hidden\"[\\t ]+name=\"__EVENTVALIDATION\"[\\t ]+id=\"__EVENTVALIDATION\" value=\"(?<eventvalidation>[^\"]+)\"");
            m = r.Match(txt);
            if (m.Success)
            {
                EventValidation = m.Groups["eventvalidation"].Value;
            }

            return txt;
        }

        public string Firma()
        {
            var url = string.Format("http://{0}/ctrlgestione/InsActivity.aspx", Site);

            // Verifico di aver caricato le informazioni sul viewstate
            if (ViewState == null || EventValidation == null)
            {
                ReadCtrlGestionePage();
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = _container;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //string postCtrl = "txtNumeroActivityForm=&objFormSigns%3AbtnSign=Firma&_ctl0%3AlistSubProject=0&_ctl0%3AtxtHelpDeskActivity=&_ctl0%3AtxtRFTActivity=&_ctl0%3AlistTypesActivity=1&_ctl0%3AlistTypesSide=1&_ctl0%3AtxtDescriptionActivity=&_ctl0%3AtxtBeginHourActivity=15.40&_ctl0%3AtxtEndHourActivity=15.40&_ctl0%3AhiddenIDActivity=&_ctl0%3AhiddenIDEmployee=";
            string postCtrl = "objFormSigns%3AbtnSign=Firma";
            string postData = string.Format("__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION={1}&{2}", HttpUtility.UrlEncode(ViewState), HttpUtility.UrlEncode(EventValidation), postCtrl);
            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(postData);
            }

            var response = request.GetResponse().GetResponseStream();

            Encoding enc = Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new StreamReader(response, enc);

            string txt = loResponseStream.ReadToEnd();
            return txt;
        }



        /*private static string ReadResponse(Stream response)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            string tempString = null;
            int count = 0;

            do
            {
                count = response.Read(buf, 0, buf.Length);

                if (count != 0)
                {
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    sb.Append(tempString);
                }
            }
            while (count > 0);

            return sb.ToString();
        }*/
    }
}