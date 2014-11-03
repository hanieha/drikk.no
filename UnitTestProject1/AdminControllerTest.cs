using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.BLL;
using WebApplication1.DAL;
using WebApplication1.Model;
using WebApplication1.Controllers;
using System.Web.Mvc;
//using MvcContrib.TestHelper;


namespace Enhetstest
{
    [TestClass]
    public class AdminControllerTest
    {

       
        
        
        
        //Registrer
        [TestMethod]
        public void Registrer_OK_Post()
        {
            var controller = new AdminController(new WebApplication1.BLL.AdminBLL(new AdminDAL()));

            var innAdmin = new Admin()
            {
                Aid = 1,
                Fornavn = "Ole",
                Etternavn = "Pedersen",
                Adresse = "Oleveien 1",
                Epost = "ole@gmail.com",
                Postnr = "2222",
                Poststed = "Oslo",
                Passord = "Hei"
            };

            var resultat = (RedirectToRouteResult)controller.Registrer();

            Assert.AreEqual(resultat.RouteName, "");
            Assert.AreEqual(resultat.RouteValues.Values.First(), "Liste");
        }
        [TestMethod]
        public void Registrer_feil_validering_Post()
        {
            var controller = new AdminController(new WebApplication1.BLL.AdminBLL(new AdminDAL()));
            var innAdmin = new Admin();
            controller.ViewData.ModelState.AddModelError("Fornavn", "Fornavn må oppgis");

            var resultat = (ViewResult)controller.Registrer();

            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewName, "");
        }
        [TestMethod]
        public void Registrer_feil_DB_Post()
        {
            var controller = new AdminController(new WebApplication1.BLL.AdminBLL(new AdminDAL()));
            var innAdmin = new Admin();
            innAdmin.Fornavn = "";

            var actionResult = (ViewResult)controller.Registrer();

            Assert.AreEqual(actionResult.ViewName, "");

        }

        //lister alle kunder
          [TestMethod]
        public void hentallekunder_view()
        {
            var controller = new AdminController(new WebApplication1.BLL.AdminBLL(new AdminDAL()));
            var listekunder = new List<Kunde>();

            var kunde = new Kunde()
            {
                 Kid = 1,
                Fornavn = "Ole",
                Etternavn = "Pedersen",
                Adresse = "Oleveien 1",
                Epost = "ole@gmail.com",
                Postnr = "2222",
                Poststed = "Oslo",
                Passord = "Hei"
                 }; 
            listekunder.Add(kunde);
            listekunder.Add(kunde);
            listekunder.Add(kunde);
            
            var resultat = (ViewResult)controller.hentAlleKunder();
            var resultatliste =(List<Kunde>)resultat.Model;
            
            Assert.AreEqual(resultat.ViewName, "");

            for ( var i = 0; i < resultatliste.Count; i++) 
            {
                Assert.AreEqual(listekunder[i].Kid, resultatliste[i].Kid);
                Assert.AreEqual(listekunder[i].Fornavn, resultatliste[i].Fornavn);
                Assert.AreEqual(listekunder[i].Etternavn, resultatliste[i].Etternavn);
                Assert.AreEqual(listekunder[i].Adresse, resultatliste[i].Adresse);
                Assert.AreEqual(listekunder[i].Epost, resultatliste[i].Epost);
                Assert.AreEqual(listekunder[i].Postnr, resultatliste[i].Postnr);
                Assert.AreEqual(listekunder[i].Poststed, resultatliste[i].Poststed);
                Assert.AreEqual(listekunder[i].Passord, resultatliste[i].Passord);
            
            }

            }


          
             // Viser detaljer om en kunde
          


            //Henter informasjonen til en Kunde for endring


            //Endrer informasjonen til en Kunde

            // Henter en Kunde fra databasen for å slette

            // sletter en Kunde fra databasen

        
     /*   //AdminSide
        [TestMethod]
        public void Admin_Session()
        { // Arrange 
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController();
            SessionMock.InitializeController(controller);
            // setningen under må være etter InitializeController 
            controller.Session["LoggetInn"] = "null";
            // Act 
            var result = (ViewResult)controller.AdminSide();
            //
            Assert.AreEqual("", result.ViewName);
        }*/


    /*    //AdminSideLoggetinn
        [TestMethod]
        public void Admin_LoggetInn()
        { // Arrange 
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController();
            SessionMock.InitializeController(controller);
            // setningen under må være etter InitializeController 
            controller.Session["InnLoggetAdmin"] = true;
            // Act 
            var result = (ViewResult)controller.AdminSide();
            //
            Assert.AreEqual("", result.ViewName);
        } */

        // Sjekker om admin finnes i databasen
        


    }
}
