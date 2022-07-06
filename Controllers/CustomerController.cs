using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext _context;

        public CustomerController ()
        {
            _context = new ApplicationDbContext (); 
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customer
        public ActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();
            
            return View(customers);
        }
        
        public ActionResult Details(int id)
        {
            var customer= _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c=>c.Id==id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }
        public ActionResult New() 
        {

            var membershipTypes = _context.MembershipTypes.ToList();

            var viewModel= new CustomerFormViewModel
                { MembershipTypes = membershipTypes };

            return View("CustomerForm",viewModel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {

                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()   

                };
                return View("CustomerForm",viewModel); 
            }

            if (customer.Id==0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id==customer.Id);

                customerInDb.Name= customer.Name;
                customerInDb.BirthDay = customer.BirthDay;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;  



                // this is microsoft way to update objects in DB but it has secuirty issues.
              //  TryUpdateModel(customerInDb);

            }

          //  _context.Customers.Add(customer);
            _context.SaveChanges();


            return RedirectToAction("Index","Customer");
        }
        private IEnumerable<Customer> GetCustomer()
        {
            var customer1 = new Customer() { Id = 1, Name = "Mohamed Ibrahim" };
            var customer2 = new Customer() { Id = 2, Name = "Mai Gomaa" };

            var customerList = new List<Customer>();
            customerList.Add(customer1);
            customerList.Add(customer2);
            return customerList;
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer==null)
            {
                return HttpNotFound();
            }

            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            return View("CustomerForm", viewModel);  
        }

    }
}