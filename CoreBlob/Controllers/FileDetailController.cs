using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreBlob.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net;


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
            return View(await _context.FileDetail.Include(b=>b.Blobs).ToListAsync());
        }

        // GET: FileDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetail = await _context.FileDetail.Include(b=>b.Blobs)
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

            var fileDetail = await _context.FileDetail.Include(b => b.Blobs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileDetail == null)
            {
                return NotFound();
            }
            return View(fileDetail);
        }

        //POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FileDetail detail)
        {   
            if (ModelState.IsValid)
            {
                try
                {
                    List<BlobSummary> Blobs = new List<BlobSummary>();

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
                            Blobs.Add(summary);
                            _context.Entry(summary).State = EntityState.Added;
                        }
                    }

                    detail.Modified = DateTime.Now;
                    detail.Blobs = Blobs;
                  //  _context.Entry(detail).State = EntityState.Modified;
                     _context.Update(detail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileDetailExists(detail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(detail);
        }

        // GET: FileDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileDetail = await _context.FileDetail.Include(b=>b.Blobs)
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                var fileDetail = await _context.FileDetail.Include(b => b.Blobs).FirstOrDefaultAsync(m => m.Id == id);
                _context.FileDetail.Remove(fileDetail);

                _context.BlobSummary.RemoveRange(fileDetail.Blobs);

                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails
                await _context.SaveChangesAsync();

                transaction.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult DeleteFile(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Guid guid = new Guid(id);
                BlobSummary fileDetail = _context.BlobSummary.Find(guid);
                if (fileDetail == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                _context.BlobSummary.Remove(fileDetail);
                _context.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", ex.Message });
            }
        }

        public IActionResult Download(String id, String fileName)
        {
            BlobSummary val = null;

            if (!string.IsNullOrEmpty(id))
            {
                val = _context.BlobSummary.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();
            }

            var content = new MemoryStream(val.Blob);
            return File(content, val.ContentType, fileName);


            //Response.Clear();
            //Response.Buffer = true;
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = val.ContentType;
            //Response.AppendHeader("Content-Disposition", "attachment; filename=" + d);
            //Response.BinaryWrite(val.Blob);
            //Response.Flush();
            //Response.End();
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
