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
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMesages()
        {
            var tests = unitOfWork.TestRepository.GetTestsMessages();
            TempData.Add("saved", "Your information has been updated!");
            return PartialView("_messagesList", tests);

        }

        public ActionResult Delete(string ID)
        {
            int id = int.Parse(ID);

            unitOfWork.TestRepository.Delete(id);
            unitOfWork.Save();

            TempData.Add("saved", "Your information has been saved!");
            return View("Index");
        }

        public ActionResult Edit(string ID)
        {
            return View("Edit", unitOfWork.TestRepository.GetByID(int.Parse(ID)));
        }

        [HttpPost]
        public ActionResult Edit(Models.DevTest test)
        {

            if (ModelState.IsValid)
            {
                unitOfWork.TestRepository.Update(test);
                unitOfWork.Save();

                if (TempData.ContainsKey("saved"))
                    TempData.Remove("saved");

                TempData.Add("saved", "Your information has been saved!");
                return View("Edit");
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
                unitOfWork.TestRepository.Insert(test);
                unitOfWork.Save();

                TempData.Add("saved", "Your information has been saved!");
                return View("Index");
            }

            return View("Create");
        }
    }
}