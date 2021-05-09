using Microsoft.AspNetCore.Mvc;
using S5_DatabaseFirstEF.Web.Models;
using S5_DatabaseFirstEF.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S5_DatabaseFirstEF.Web.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Listado()
        {
            var listadoCliente = await CustomerRepository.ListadoAsync();
            return PartialView(listadoCliente);
        }

        [HttpPost]
        public async Task<IActionResult> Grabar(
            int idCliente,
            string nombres,
            string apellidos,
            string pais,
            string ciudad,
            string telefono) {

            var customer = new Customer()
            {
                FirstName = nombres,
                LastName = apellidos,
                City = ciudad,
                Country = pais,
                Phone = telefono
            };
            var exito = true;
            if (idCliente == -1)
                exito = await CustomerRepository.Insertar(customer);
            else {
                customer.Id = idCliente;
                exito = await CustomerRepository.Actualizar(customer);
            }

            return Json(exito);
        }

    }
}
