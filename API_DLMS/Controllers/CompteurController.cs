using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_DLMS.Controllers
{
    public class CompteurController : Controller
    {

        // GET: CompteurController/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: CompteurController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompteurController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompteurController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompteurController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
