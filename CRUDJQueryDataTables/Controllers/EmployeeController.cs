using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUDJQueryDataTables.Models;

namespace CRUDJQueryDataTables.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetEmployees()
        {
            using (DbEntities dc = new DbEntities())
            {
                var employees = dc.Employees.OrderBy(a => a.fName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Save(int id)
        {
            using (DbEntities dc = new DbEntities())
            {
                var v = dc.Employees.Where(a => a.Id == id).FirstOrDefault();
                return View(v);
            }
        }
        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (DbEntities dc = new DbEntities())
                {
                    if (emp.Id > 0)
                    {
                        //Edit 
                        var v = dc.Employees.Where(a => a.Id == emp.Id).FirstOrDefault();
                        if (v != null)
                        {
                            v.fName = emp.fName;
                            v.lName = emp.lName;
                            v.Email = emp.Email;
                            v.City = emp.City;
                            v.Country = emp.Country;
                        }
                    }
                    else
                    {
                        //Save
                        dc.Employees.Add(emp);
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (DbEntities dc = new DbEntities())
            {
                var v = dc.Employees.Where(a => a.Id == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (DbEntities dc = new DbEntities())
            {
                var v = dc.Employees.Where(a => a.Id == id).FirstOrDefault();
                if (v != null)
                {
                    dc.Employees.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}