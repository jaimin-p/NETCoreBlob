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
    public class FileDetailController : Controller
    {
        private readonly BlobContext _context;

        public FileDetailController(BlobContext context)
        {
            _context = context;
        }

        // GET: FileDetail
        public async Task<IActionResult> Index()
        {
            return View(await _context.FileDetail.ToListAsync());
        }

        // GET: FileDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetail = await _context.FileDetail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileDetail == null)
            {
                return NotFound();
            }

            return View(fileDetail);
        }

        // GET: FileDetail/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FileDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Summary,Modified")] FileDetail fileDetail)
        {
            if (ModelState.IsValid)
            {
                fileDetail.Modified = DateTime.Now;
                _context.Add(fileDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fileDetail);
        }

        // GET: FileDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetail = await _context.FileDetail.FindAsync(id);
            if (fileDetail == null)
            {
                return NotFound();
            }
            return View(fileDetail);
        }

        // POST: FileDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Summary,Modified")] FileDetail fileDetail)
        {
            if (id != fileDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    fileDetail.Modified = DateTime.Now;
                    _context.Update(fileDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileDetailExists(fileDetail.Id))
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
            return View(fileDetail);
        }

        // GET: FileDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetail = await _context.FileDetail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileDetail == null)
            {
                return NotFound();
            }

            return View(fileDetail);
        }

        // POST: FileDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fileDetail = await _context.FileDetail.FindAsync(id);
            _context.FileDetail.Remove(fileDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileDetailExists(int id)
        {
            return _context.FileDetail.Any(e => e.Id == id);
        }
    }
}
