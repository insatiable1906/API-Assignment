using test2.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using test2.Models;
using System;
using Microsoft.AspNetCore.Mvc;

namespace test2.Controllers.Tests
{
    [TestClass()]
    public class LabResultsControllerTests
    {
        [TestMethod()]
        public void GetAllReportsTest_Valid()
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
                EnteredTime ="1/1/2021"
            };

            //Act
            var returnLabId = (labController.Post(lab).Result as OkObjectResult).Value;

            //Assert
            Assert.IsNotNull(returnLabId);
        }

        [TestMethod()]
        public void GetReportTest_Valid()
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
            var returnLab = labController.GetReport(Guid.Parse(returnLabId.ToString()));

            //Assert
            Assert.IsNotNull(returnLab);
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
            var returnLab = labController.GetReport(Guid.Parse(returnLabId.ToString()));

            //Assert
            var check = (returnLab.Result as OkObjectResult).Value;
            var check2 = (LabResults)check;

            //Assert
            Assert.IsNotNull(check);
        }

        [TestMethod()]
        public void PutTest_Valid()
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
            var labController = new LabResultsController(cache);
            var id = (mockController.Post(patient).Result as OkObjectResult).Value;
            var lab = new LabResults()
            {
                LabType = "TEST",
                Result = "TESTING",
                TestTime = "1/1/2021",
                PatientID = Guid.Parse(id.ToString()),
                EnteredTime = "1/1/2021"
            };
            var returnLabId = (labController.Post(lab).Result as OkObjectResult).Value;

            var updateLab = new LabResults()
            {
                LabType = "TEST",
                Result = "TESTING2",
                TestTime = "1/1/2021",
                PatientID = Guid.Parse(id.ToString()),
                EnteredTime = "1/1/2021",
                LabID = Guid.Parse(returnLabId.ToString())
            };

            //Act
            var updateLabId = ((labController.Put(updateLab)).Result as OkObjectResult).Value;
            var returnLab = ((labController.GetReport(Guid.Parse(returnLabId.ToString()))).Result as OkObjectResult).Value as LabResults;

            //Assert
            Assert.IsTrue(((LabResults)returnLab).Result == "TESTING2");
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
            var bLabReportDeleted = ((labController.Delete(Guid.Parse(returnLabId.ToString()))).Result as OkObjectResult).Value;
            var returnLab = labController.GetReport(Guid.Parse(returnLabId.ToString()));

            var check = (returnLab.Result as OkObjectResult).Value;
            var check2 = (LabResults)check;
            //Assert
            Assert.IsTrue(Convert.ToBoolean(bLabReportDeleted));
            Assert.IsNull(check2);
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