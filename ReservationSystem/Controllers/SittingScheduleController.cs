using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Models;
using static ReservationSystem.Models.Reservation;

namespace ReservationSystem.Controllers
{
    public class SittingScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostEnvironment _hostEnvironment;

        public SittingScheduleController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _hostEnvironment = hostEnvironment;
        }
        /// <summary>
        /// Displays all SittingSchedules
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        // GET: SittingSchedule
        public async Task<IActionResult> Index(SittingSchedule.SessionEnum? session)
        {
            if (session == null)
            {
                return View(await _context.SittingSchedule.ToListAsync());
            }
            else
            {
                var filter = await _context.SittingSchedule.AsQueryable()
                .Where(s => s.SessionType == session).ToListAsync();
                return View(filter);
            }
        }
        /// <summary>
        /// Dislpayed selected SittingSchedule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: SittingSchedule/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if ((id == null) || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule
                .FirstOrDefaultAsync(m => ((int)m.SessionType) == id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            return View(sittingSchedule);
        }
        /// <summary>
        /// Displays empty SittingSchedule model
        /// </summary>
        /// <returns></returns>
        // GET: SittingSchedule/Create
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Adds new SittingSchedule model to database
        /// </summary>
        /// <param name="sittingSchedule"></param>
        /// <returns></returns>
        // POST: SittingSchedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SittingSchedule sittingSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sittingSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sittingSchedule);
        }
        //Displays selected SittingSchedule
        // GET: SittingSchedule/Edit/5
        public async Task<IActionResult> Edit(int sessionId)
        {
            if (sessionId == null || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule.FindAsync(sessionId);
            if (sittingSchedule == null)
            {
                return NotFound();
            }
            return View(sittingSchedule);
        }
        /// <summary>
        /// Updates selected Sittingschedule
        /// </summary>
        /// <param name="sittingSchedule"></param>
        /// <returns></returns>
        // POST: SittingSchedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SittingSchedule sittingSchedule)
        {
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sittingSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SittingScheduleExists(sittingSchedule.Id))
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
            return View(sittingSchedule);
        }
        /// <summary>
        /// Displayes selected SittingSchedule
        /// </summary>
        /// <param name="sessionTypeId"></param>
        /// <returns></returns>
        // GET: SittingSchedule/Delete/5
        public async Task<IActionResult> Delete(SittingSchedule.SessionEnum sessionTypeId)
        {
            if (sessionTypeId == null || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule
                .FirstOrDefaultAsync(m => m.SessionType == sessionTypeId);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            return View(sittingSchedule);
        }
        /// <summary>
        /// Removes selected SittingSchedule
        /// </summary>
        /// <param name="sessionTypeId"></param>
        /// <returns>Once SittingSchedule is Deleted, redirected to index</returns>
        // POST: SittingSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int sessionTypeId)
        {
            if (_context.SittingSchedule == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SittingSchedule'  is null.");
            }
            var sittingSchedule = await _context.SittingSchedule.FindAsync(sessionTypeId);
            if (sittingSchedule != null)
            {
                _context.SittingSchedule.Remove(sittingSchedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingScheduleExists(int sessionTypeId)
        {
          return _context.SittingSchedule.Any(e => ((int)e.SessionType) == sessionTypeId);
        }
        /// <summary>
        /// Uploads image to local directory
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        //POST: SittingController/ImageUpload
        //Handle image upload for sitting schedule area
        [HttpPost, ActionName("ImageUpload")]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            //Image save folder within wwwroot
            string imageFolder = "Images\\Sessions";

            //Create folder if it doesn't exist
            string dirPath = Path.Combine(_webHostEnvironment.WebRootPath, imageFolder);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //Create image file (handle the upload)
            string filePath = Path.Combine(dirPath, file.FileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
            }

            //Return 200 + image path
            return StatusCode(200, Path.Combine(imageFolder, file.FileName));
        }
    }
}
