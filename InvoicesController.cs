using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationdbContext _context;

        public InvoicesController(ApplicationdbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gstinvoice.ToListAsync());
        }

        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(string gemInvoiceNo)
        {
            if (string.IsNullOrEmpty(gemInvoiceNo))
            {
                ModelState.AddModelError("gemInvoiceNo", "Please provide a Gem Invoice No.");
                return View();
            }

            var invoice = await _context.Gstinvoice
                .FirstOrDefaultAsync(i => i.GemInvoiceNo == gemInvoiceNo);

            if (invoice == null)
            {
                ViewBag.Message = "No invoice found with the provided Gem Invoice No.";
                return View();
            }

            return View("Details", invoice); // Reuse the Details view to display the result

         
        }

        public IActionResult SearchByDate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SearchByDate(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                ModelState.AddModelError("", "Please provide both start and end dates.");
                return View();
            }

            var invoices = await _context.Gstinvoice
                .Where(i => i.Date >= startDate && i.Date <= endDate)
                .ToListAsync();

            if (invoices.Count == 0)
            {
                ViewBag.Message = "No invoices found in the given date range.";
            }

           
            var Amount=invoices.Sum(i => i.Amount);
            var totalCGST = invoices.Sum(i => i.CGSTAmount);
            var totalSGST = invoices.Sum(i => i.SGSTAmount);
            var totalGST = totalCGST + totalSGST;
            var totalAmount= Amount + totalGST;

            ViewBag.Amount= Amount;
            ViewBag.TotalCGST = totalCGST;
            ViewBag.TotalSGST = totalSGST;
            ViewBag.TotalGST = totalGST;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.StartDate = startDate.Value.ToShortDateString();
            ViewBag.EndDate = endDate.Value.ToShortDateString();


            return View("SearchResults", invoices);
        }
        public IActionResult SearchResults()
        {
            return View();
        }


        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Gstinvoice
                .FirstOrDefaultAsync(m => m.Sno == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

       
        


        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sno,Date,GemInvoiceNo,CompanyName,Items,Qty,Rate,Amount,AmountInwords,CGST,SGST,CGSTAmount,SGSTAmount,TotalAmount,TotalAmountInWords")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                if (_context.Gstinvoice.Any(i => i.GemInvoiceNo == invoice.GemInvoiceNo))
                {
                    ModelState.AddModelError("GemInvoiceNo", "Duplicate Gem Invoice No");
                    return View(invoice);
                }

                // Calculate GST
                invoice.Amount = invoice.Qty * invoice.Rate;
                invoice.CGSTAmount = (invoice.CGST) * Convert.ToDecimal(invoice.Amount) / 100;
                invoice.SGSTAmount = (invoice.SGST) * Convert.ToDecimal(invoice.Amount) / 100;
                
                invoice.TotalAmount = Convert.ToDecimal(invoice.Amount) + invoice.CGSTAmount + invoice.SGSTAmount;
                
                
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Amount"] = invoice.Amount;
            ViewData["CGSTAmount"] = invoice.CGSTAmount;
            ViewData["SGSTAmount"] = invoice.SGSTAmount;
            ViewData["TotalAmount"] = invoice.TotalAmount;
            return View(invoice);
            
        }

        // GET: Invoices/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Gstinvoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Sno,Date,GemInvoiceNo,CompanyName,Items,Qty,Rate,Amount,AmountInwords,CGST,SGST,CGSTAmount,SGSTAmount,TotalAmount,TotalAmountInWords")] Invoice invoice)
        {
            if (id != invoice.Sno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Sno))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Gstinvoice
                .FirstOrDefaultAsync(m => m.Sno == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Gstinvoice.FindAsync(id);
            if (invoice != null)
            {
                _context.Gstinvoice.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Gstinvoice.Any(e => e.Sno == id);
        }
    }
     
}
