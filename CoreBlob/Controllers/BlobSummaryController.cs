using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreBlob.Models;

namespace CoreBlob.Controllers
{
    public class BlobSummaryController : Controller
    {
        private readonly BlobContext _context;

        public BlobSummaryController(BlobContext context)
        {
            _context = context;
        }

        // GET: BlobSummary
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlobSummary.ToListAsync());
        }

        // GET: BlobSummary/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blobSummary = await _context.BlobSummary
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blobSummary == null)
            {
                return NotFound();
            }

            return View(blobSummary);
        }

        // GET: BlobSummary/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlobSummary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FileName,Extension,ContentType,FileDetail_Id,Blob")] BlobSummary blobSummary)
        {
            if (ModelState.IsValid)
            {
                blobSummary.Id = Guid.NewGuid();
                _context.Add(blobSummary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blobSummary);
        }

        // GET: BlobSummary/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blobSummary = await _context.BlobSummary.FindAsync(id);
            if (blobSummary == null)
            {
                return NotFound();
            }
            return View(blobSummary);
        }

        // POST: BlobSummary/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FileName,Extension,ContentType,FileDetail_Id,Blob")] BlobSummary blobSummary)
        {
            if (id != blobSummary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blobSummary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlobSummaryExists(blobSummary.Id))
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
            return View(blobSummary);
        }

        // GET: BlobSummary/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blobSummary = await _context.BlobSummary
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blobSummary == null)
            {
                return NotFound();
            }

            return View(blobSummary);
        }

        // POST: BlobSummary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var blobSummary = await _context.BlobSummary.FindAsync(id);
            _context.BlobSummary.Remove(blobSummary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlobSummaryExists(Guid id)
        {
            return _context.BlobSummary.Any(e => e.Id == id);
        }
    }
}
