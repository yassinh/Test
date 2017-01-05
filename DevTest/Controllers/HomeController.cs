using DevTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTest.Controllers
{
    public class HomeController : Controller
    {
        private ITestRepository testRepository;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMesages()
        {
            this.testRepository = new DAL.TestRepository(new DevTestContext());
            List<Models.DevTest> lst = testRepository.GetTestsMessages();
            return PartialView("_messagesList", lst);

        }

        public ActionResult Delete(string ID)
        {
            int id = int.Parse(ID);

            this.testRepository = new DAL.TestRepository(new DevTestContext());
            testRepository.DeleteTest(id);
            testRepository.Save();

            return View("Index");
        }

        public ActionResult Edit(string ID)
        {
            this.testRepository = new DAL.TestRepository(new DevTestContext());

            return View("Edit", testRepository.GetTestByID(int.Parse(ID)));
        }

        [HttpPost]
        public ActionResult Edit(Models.DevTest test)
        {

            if (ModelState.IsValid)
            {

                this.testRepository = new DAL.TestRepository(new DevTestContext());
                testRepository.UpdateTest(test);
                testRepository.Save();

                return RedirectToAction("Index");
            }

            return View("Edit");
        }

        public ActionResult Create()
        { 
            return View("Create");
        }

        [HttpPost]
        public ActionResult Create(Models.DevTest test)
        {

            if (ModelState.IsValid)
            {

                this.testRepository = new DAL.TestRepository(new DevTestContext());
                testRepository.InsertTest(test);
                testRepository.Save();

                return RedirectToAction("Index");
            }

            

            return View("Create");
        }
    }
}