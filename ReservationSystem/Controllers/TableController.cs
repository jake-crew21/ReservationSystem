using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReservationSystem.Data;
using ReservationSystem.Models;

namespace ReservationSystem.Controllers
{
    public class TableController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostEnvironment _hostEnvironment;

        public TableController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _hostEnvironment = hostEnvironment;
        }

        public TableController()
        {
        }

        /// <summary>
        /// Displays all Tables
        /// </summary>
        /// <returns></returns>
        // GET: Table
        public async Task<IActionResult> Index()
        {
            return View(await _context.Table.ToListAsync());
        }
        /// <summary>
        /// Display all tables with matching {area}
        /// </summary>
        /// <param name="area">Sitting Area</param>
        /// <returns></returns>
        //GET: Table (by area)
        public async Task<IActionResult> IndexArea(Models.Table.AreaEnum area)
        {
            var filter = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();
            return View(filter);
        }
        /// <summary>
        /// Displays selected Table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Table/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Table == null)
            {
                return NotFound();
            }

            var table = await _context.Table
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }
        /// <summary>
        /// Displays empty Table Model
        /// </summary>
        /// <returns></returns>
        // GET: Table/Create
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Add new Table Model to database
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        // POST: Table/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Table table)
        {
            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }
        /// <summary>
        /// Adds new Table Model to database with in {area}
        /// </summary>
        /// <param name="area">Sitting area</param>
        /// <param name="table">Table Model</param>
        /// <returns></returns>
        //POST: Table/Create (QuickAdd)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickAdd(Models.Table.AreaEnum area, Models.Table table)
        {
            table.Area = area;
            var tableInArea = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();
            int tbNum = tableInArea.Count() + 1;
            if (table.Area == Models.Table.AreaEnum.Main)
            {
                table.TableNum = "M" + tbNum.ToString();
                table.Seats = 4;
            }
            else if (table.Area == Models.Table.AreaEnum.Balcony)
            {
                table.TableNum = "B" + tbNum.ToString();
                table.Seats = 2;
            }
            else if (table.Area == Models.Table.AreaEnum.Outside)
            {
                table.TableNum = "O" + tbNum.ToString();
                table.Seats = 2;
            }

            ModelState.Clear();
            TryValidateModel(table);

            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }
        /// <summary>
        /// Reomves Table from slected {area} that is last in listing
        /// </summary>
        /// <param name="area">Sitting area</param>
        /// <returns></returns>
        //POST: Table/Delete (QuickRemove)
        [HttpPost, ActionName("QuickRemove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickRemove(Models.Table.AreaEnum area)
        {
            var tableInArea = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();
            Models.Table table = tableInArea.LastOrDefault();
            if (table != null)
            {
                _context.Table.Remove(table);
                return View(Index);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Change Image for all tables in selected {area}
        /// </summary>
        /// <param name="area">Sitting area</param>
        /// <returns></returns>
        // GET: Table/EditImage
        public async Task<IActionResult> EditImage(Models.Table.AreaEnum area)
        {
            var tableInArea = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();

            try
            {
                foreach (var table in tableInArea)
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (tableInArea == null)
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
        /// <summary>
        /// Displays selected Table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Table/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Table == null)
            {
                return NotFound();
            }

            var table = await _context.Table.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            return View(table);
        }
        /// <summary>
        /// Updates selected Table
        /// </summary>
        /// <param name="id">Table Id</param>
        /// <param name="table">Table Model</param>
        /// <returns></returns>
        // POST: Table/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Table table)
        {
            if (id != table.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.Id))
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
            return View(table);
        }
        /// <summary>
        /// Displays selected Table
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        // GET: Table/Delete/5
        public async Task<IActionResult> Delete(Models.Table.AreaEnum area)
        {
            var tableInArea = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();
            Models.Table table = tableInArea.LastOrDefault();

            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }
        /// <summary>
        /// Removes selected table
        /// </summary>
        /// <param name="id">Table Id</param>
        /// <returns>Once removed, redirected to index</returns>
        // POST: Table/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Table == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Table'  is null.");
            }
            var table = await _context.Table.FindAsync(id);
            if (table != null)
            {
                _context.Table.Remove(table);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(int id)
        {
          return _context.Table.Any(e => e.Id == id);
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
            string imageFolder = "Images/Area";

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
            return StatusCode(200, $"");
        }
    }
}
