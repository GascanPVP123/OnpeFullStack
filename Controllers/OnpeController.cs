using Microsoft.AspNetCore.Mvc;
using OnpeFullStack.DAO;
using System.Data;

namespace OnpeFullStack.Controllers
{
    public class OnpeController : Controller
    {
        private readonly OnpeDAO _dao;

        public OnpeController(OnpeDAO dao)
        {
            _dao = dao;
        }

        public IActionResult ResumenGeneral()
        {
            var dt = _dao.GetResultadosPresidenciales(); 
            return View(dt);
        }

        public IActionResult ResultadosPresidenciales()
        {
            var dt = _dao.GetResultadosPresidenciales(); 
            return View(dt);
        }
    }
}