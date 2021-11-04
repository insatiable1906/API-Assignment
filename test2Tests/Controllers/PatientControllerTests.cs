using Microsoft.VisualStudio.TestTools.UnitTesting;
using test2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework.Internal;
using Microsoft.Extensions.DependencyInjection;
using test2.Models;
using Microsoft.AspNetCore.Mvc;

namespace test2.Controllers.Tests
{
    [TestClass()]
    public class PatientControllerTests
    {
        [TestMethod()]
        public void GetAllPatientsTest_Valid()
        {
            //Arrage
            var cache = this.SetCache();
            var mockController = new PatientController(cache);
            var patient = new Patient()
            {
                FirstName = "TEST",
                LastName = "TESTING3",
                DOB = "1/1/2021",
                Gender = "MALE"
            };

            //Act
            var id = (mockController.Post(patient).Result as OkObjectResult).Value;
            var getPatient = mockController.GetAllPatients();// (Guid.Parse(id.ToString()));

            var check = (getPatient.Result as OkObjectResult).Value;
            var check2 = (List<Patient>)check;

            //Assert
            Assert.IsTrue(check2.Count() == 1);
        }

        [TestMethod()]
        public void GetPatientsPerLabResultsSearchTest_Valid()
        {
            //Arrage
            var cache = this.SetCache();
            var mockController = new PatientController(cache);
            var patient = new Patient()
            {
                FirstName = "TEST",
                LastName = "TESTING3",
                DOB = "1/1/2021",
                Gender = "MALE"
            };

            //Act
            var id = (mockController.Post(patient).Result as OkObjectResult).Value;

            var labController = new LabResultsController(cache);
            var lab = new LabResults()
            {
                LabType = "TEST",
                Result = "TESTING",
                TestTime = "1/1/2021",
                PatientID = Guid.Parse(id.ToString()),
                EnteredTime = "1/1/2021"
            };

            //Act
            var returnLabId = (labController.Post(lab).Result as OkObjectResult).Value;

            var returnPatients = mockController.GetPatientsPerLabResultsSearch(new LabSearch()
            {
                LabType = "TEST",
                FromDate = "1/1/2001",
                ToDate = "1/2/2021"
            });

            var check = (returnPatients.Result as OkObjectResult).Value;
            var check2 = (List<Patient>)check;

            //Assert
            Assert.IsTrue(check2.Count() == 1);
        }

        [TestMethod()]
        public void PostTest_Valid()
        {
            //Arrage
            var cache = this.SetCache();
            var mockController = new PatientController(cache);
            var patient = new Patient()
            {
                FirstName = "TEST",
                LastName = "TESTING3",
                DOB = "1/1/2021",
                Gender = "MALE"
            };

            //Act
            var id = (mockController.Post(patient).Result as OkObjectResult).Value;
            var getPatient = mockController.GetItem(Guid.Parse(id.ToString()));

            //Assert
            Assert.IsNotNull(getPatient);
        }

        [TestMethod()]
        public void PutTest_Valid()
        {
            //Arrage
            var cache = this.SetCache();
            var mockController = new PatientController(cache);
            var checkPatient = new Patient()
            {
                FirstName = "TEST",
                LastName = "TESTING1",
                DOB = "1/1/2021",
                Gender = "MALE"
            };
            var updatePatient = new Patient()
            {
                FirstName = "TEST",
                LastName = "TESTING2",
                DOB = "1/1/2021",
                Gender = "MALE"
            };

            //Act
            var id = (mockController.Post(checkPatient).Result as OkObjectResult).Value;
            var patient = mockController.GetItem(Guid.Parse(id.ToString()));
            var retrievedPatient = ((mockController.GetItem(Guid.Parse(id.ToString()))).Result as OkObjectResult);
            ((Patient)retrievedPatient.Value).LastName = "TESTING2";
            ((Patient)retrievedPatient.Value).PatientId = Guid.Parse(id.ToString());
            var updateID = ((mockController.Put((Patient)retrievedPatient.Value)).Result as OkObjectResult).Value;
            var updatedPatient = ((mockController.GetItem(Guid.Parse(updateID.ToString()))).Result as OkObjectResult).Value as Patient;

            ////Assert
            Assert.IsNotNull(patient);
            Assert.IsNotNull(updatedPatient);
            Assert.IsTrue(((Patient)updatedPatient).LastName == "TESTING2");
            Assert.IsTrue(Guid.Parse(id.ToString()) == Guid.Parse(updateID.ToString()));
        }

        [TestMethod()]
        public void DeleteTest_Valid()
        {
            //Arrage
            var cache = this.SetCache();
            var mockController = new PatientController(cache);
            var patient = new Patient()
            {
                FirstName = "TEST",
                LastName = "TESTING3",
                DOB = "1/1/2021",
                Gender = "MALE"
            };

            //Act
            var id = (mockController.Post(patient).Result as OkObjectResult).Value;
            var bPatientDeleted = ((mockController.Delete(Guid.Parse(id.ToString()))).Result as OkObjectResult).Value;
            var updatePatient = mockController.GetAllPatients();

            var check = (updatePatient.Result as OkObjectResult).Value;
            var check2 = (List<Patient>)check;

            //Assert
            Assert.IsTrue(Convert.ToBoolean(bPatientDeleted));
            Assert.IsTrue(check2.Count() == 0);
        }
        private IMemoryCache? SetCache()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<IMemoryCache>();
        }
    }
}