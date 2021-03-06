﻿using Postal;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Http;

namespace Ait.Infrastructure.Web.Controllers
{
    /// <summary>
    /// https://security.google.com/settings/security/apppasswords
    /// 
    /// http://localhost:16807/Email/MultiPart?format=html
    /// </summary>
    [RoutePrefix("api/Email")]
    public class EmailController : ApiController
    {
        private InfraShell Shell
        {
            get { return Request.GetOwinContext().GetShell() ?? new InfraShell(); }
        }


        //public class EmailFormModel
        //{
        //    [Display(Name = "От кого"), EmailAddress]
        //    public string FromName { get; set; }

        //    [RegularExpression(Maybe2.Re.email, ErrorMessage = "Поле E-mail некорректо!")]
        //    [Display(Name = "email"), EmailAddress]
        //    public string FromEmail { get; set; }
        //    [Required]
        //    public string Message { get; set; }
        //}


        public class EmailMessage : Email
        {
            public EmailMessage() : base("ResetPassword.html")
            {
            }



            [Display(Name = "Куда"), EmailAddress]
            public string To { get; set; }

            [Display(Name = "От кого"), EmailAddress]
            public string From { get; set; }

            public string Subject { get; set; }

            [Required]
            public string Message { get; set; }

            public string PersonName { get; set; }
        }

        [HttpGet]
        public IHttpActionResult Test(string id)
        {
            return Ok();
        }


        [HttpGet]
        public IHttpActionResult Send(string id)
        {
            try
            {
                var shell = new InfraShell();



                EmailMessage email = new EmailMessage();

                var em = shell.EmailService;

                email.ViewData["From"] = "XXX@gmail.com";
                email.ViewData["To"] = "dundich_@mail.ru";
                email.ViewData["Subject"] = "тест";
                email.ViewData["PersonName"] = "Иванов И И";
                email.ViewData["Message"] = id;

                em.Send(email);
            }
            catch (Exception e)
            {
                //throw e;
                return BadRequest(e.Message);
            }
            return Ok();

        }



        //[HttpPost]
        //[Authorize]
        //[Route("api/Email/Send")]
        //[System.Web.Mvc.ValidateAntiForgeryToken]
        //public IHttpActionResult Send(EmailMessage model)
        //{

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {

        //        var email = model;
        //        email.To = "dundich1@mail.ru";
        //        email.Subject = "тест";
        //        email.PersonName = "Иванов И И";
        //        email.Send();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //        //return BadRequest(e.Message);
        //    }
        //    return Ok();
        //}
    }
}
