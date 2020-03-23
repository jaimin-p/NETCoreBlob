using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreBlob.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(FileDetail detail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<BlobSummary> fileDetails = new List<BlobSummary>();
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var file = Request.Form.Files[i];

                        if (file != null)
                        {
                            var fileName = Path.GetFileName(file.FileName);

                            BlobSummary fileDetail = new BlobSummary()
                            {
                                FileName = fileName,
                                Extension = Path.GetExtension(fileName),
                                Id = Guid.NewGuid(),
                                Blob = GenerateBlob(file),
                                ContentType = Request.Form.Files[i].ContentType
                            };
                            fileDetails.Add(fileDetail);
                        }
                    }

                    detail.Modified = DateTime.Now;
                    detail.Blobs = fileDetails;

                    _context.FileDetail.Add(detail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(Index));
            }
            return View(detail);

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

        //POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Summary")] FileDetail fileDetail)
        {
            if (id != fileDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    for (int i = 0; i < Request.Form.Files.Count; i++)
                    {
                        var file = Request.Form.Files[i];

                        if (file != null)
                        {
                            var fileName = Path.GetFileName(file.FileName);

                            BlobSummary summary = new BlobSummary()
                            {
                                FileName = fileName,
                                Extension = Path.GetExtension(fileName),
                                Id = Guid.NewGuid(),
                                Blob = GenerateBlob(file),
                                ContentType = Request.Form.Files[i].ContentType
                            };
                            _context.Entry(summary).State = EntityState.Added;
                        }
                    }

                    fileDetail.Modified = DateTime.Now;
                    _context.Entry(fileDetail).State = EntityState.Modified;
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

       
        private byte[] GenerateBlob(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private bool FileDetailExists(int id)
        {
            return _context.FileDetail.Any(e => e.Id == id);
        }
    }
}
