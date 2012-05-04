﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlloGestione.Services;
using ControlloGestione.ViewModel;
using ControlloGestione.Mapping;

namespace ControlloGestione.Controllers
{
    public class HomeController : Controller
    {
        public User CurrentUser
        {
            get
            {
                return ControllerContext.HttpContext.Session["CurrentUser"] as User;
            }
            set
            {
                ControllerContext.HttpContext.Session["CurrentUser"] = value;
            }
        }

        private IFirmeService service;
        private IOrariParserService parser;

        public HomeController(IFirmeService service, IOrariParserService parser)
        {
            this.service = service;
            this.parser = parser;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Firma()
        {
            try
            {
                if (CurrentUser != null)
                {
                    service.Autentica(CurrentUser.Username, CurrentUser.Password);
                    string raw_page = service.Firma();

                    var pageModel = parser.ReadHours(raw_page);

                    var vm = pageModel.ConvertToCtrlGestioneInfoVM();

                    return RedirectToSuccess("Firma", "FIRMATO", "Firma effettuata correttamente alle "+ vm.UltimaFirma);
                }

                return RedirectToError("Firma", "Errore: Utente non valido");
            }
            catch (Exception ex)
            {
                return RedirectToError("Firma", ex.Message + " " + ex.StackTrace);
            }
        }

        public ActionResult Info()
        {
            if (CurrentUser != null)
            {
                service.Autentica(CurrentUser.Username, CurrentUser.Password);
                string raw_page = service.ReadCtrlGestionePage();

                var pageModel = parser.ReadHours(raw_page);
                var pageVM = pageModel.ConvertToCtrlGestioneInfoVM();

                return View(pageVM);
            }

            return View();
        }

        public ActionResult Configura()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Configura(User utente)
        {
            string msg; 
            try
            {
                bool result = service.Autentica(utente.Username, utente.Password);

                if (result)
                {
                    CurrentUser = utente;
                }

                msg = "OK";
            }
            catch (Exception fault)
            {
                string inner;
                if(fault.InnerException != null)
                    inner = fault.InnerException.Message;
                else
                    inner = "OK";

                msg = string.Format("{0} <br/> Risultato Servizio: {1}", fault.Message, inner);
            }


            return Content(msg);
        }

        

        /*public ActionResult GetRawCtrlPage()
        {
            if (CurrentUser != null)
            {
                FirmeService service = new FirmeService();
                service.Autentica(CurrentUser.Username, CurrentUser.Password);
                string raw_page = service.ReadCtrlGestionePage();
                
                return Content(raw_page);
            }

            //ViewBag.Message = "Errore: Utente non valido";
            return Content("ERR");
        }*/

        #region UTILITA'

        public ActionResult IsValidCache()
        {
            string msg = CurrentUser != null ? CurrentUser.Username : "ERR";
            return Content(msg);
        }


        public ActionResult RedirectToSuccess(string title, string message, string information = "")
        {
            ResultVM result = new ResultVM
            {
                Title = title,
                Message = message,
                Information = information,
                Type = ResultVM.TYPE_SUCCESS
            };

            return View("Result", result);
        }

        public ActionResult RedirectToError(string title, string message, string information = "")
        {
            ResultVM result = new ResultVM
            {
                Title = title,
                Message = message,
                Information = information,
                Type = ResultVM.TYPE_ERROR
            };

            return View("Result", result);
        }

        #endregion

    }
}