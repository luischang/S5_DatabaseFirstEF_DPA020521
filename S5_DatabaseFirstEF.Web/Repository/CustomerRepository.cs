using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using S5_DatabaseFirstEF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace S5_DatabaseFirstEF.Web.Repository
{
    public class CustomerRepository
    {

        public static IEnumerable<Customer> Listado()
        {
            using var data = new SalesContext();
            return data.Customer.OrderBy(x => x.LastName).ToList();
        }

        public static async Task<IEnumerable<Customer>> ListadoAsync()
        {
            //using var data = new SalesContext();
            //return await data.Customer.OrderBy(x => x.LastName).ToListAsync();

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .GetAsync("http://localhost:63292/api/Customer/GetCustomer");
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<IEnumerable<Customer>>(apiResponse);
            return customers;
        }

        public static IEnumerable<Customer> ListadoConSP()
        {
            using var data = new SalesContext();
            return data.Customer.FromSqlRaw("SP_SEL_CUSTOMER");
        }


        public static async Task<Customer> Obtener(int id)
        {
            //using var data = new SalesContext();
            //return await data.Customer.Where(x => x.Id == id).FirstOrDefaultAsync();
            using var httpClient = new HttpClient();
            using var response = await httpClient
                .GetAsync("http://localhost:63292/api/Customer/GetCustomerById/" + id);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(apiResponse);
            return customer;
        }

        public static async Task<bool> Insertar(Customer customer)
        {
            bool exito = true;

            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            using var response = await httpClient
                .PostAsync("http://localhost:63292/api/Customer/PostCustomer", data);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var customerResponse = JsonConvert.DeserializeObject<Customer>(apiResponse);
            if (customerResponse == null)
                exito = false;

            return exito;
            //bool exito = true;

            //try
            //{
            //    using var data = new SalesContext();
            //    data.Customer.Add(customer);
            //    await data.SaveChangesAsync();
            //}
            //catch (Exception)
            //{
            //    exito = false;
            //}
            //return exito;
        }

        public static async Task<bool> Actualizar(Customer customer)
        {
            bool exito = true;

            using var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await httpClient
                .PutAsync("http://localhost:63292/api/Customer/PutCustomer", data);

            string apiResponse = await response.Content.ReadAsStringAsync();
            var customerResponse = JsonConvert.DeserializeObject<Customer>(apiResponse);

            if (customerResponse == null)
                exito = false;

            return exito;

        //    bool exito = true;

        //    try
        //    {
        //        using var data = new SalesContext();
        //        var customerNow = await data.Customer.Where(x => x.Id == customer.Id).FirstOrDefaultAsync();

        //        customerNow.FirstName = customer.FirstName;
        //        customerNow.LastName = customer.LastName;
        //        customerNow.City = customer.City;
        //        customerNow.Country = customer.Country;
        //        customerNow.Phone = customer.Phone;

        //        await data.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        exito = false;
        //    }
        //    return exito;
        }

        public static async Task<bool> Eliminar(int id)
        {
            bool exito = true;

            using var httpClient = new HttpClient();

            using var response = await httpClient
                .DeleteAsync("http://localhost:63292/api/Customer/DeleteCustomer/" + id);

            string apiResponse = await response.Content.ReadAsStringAsync();

            if ((int)response.StatusCode == 400)
                exito = false;

            return exito;
            //bool exito = true;

            //try
            //{
            //    using var data = new SalesContext();
            //    var customerNow = await Obtener(id);

            //    data.Customer.Remove(customerNow);
            //    await data.SaveChangesAsync();

            //}
            //catch (Exception)
            //{
            //    exito = false;
            //}

            //return exito;
        }


    }
}
