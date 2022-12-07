using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Models;

namespace ReservationSystem.Controllers
{
    public class TableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TableController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Table
        public async Task<IActionResult> Index()
        {
            return View(await _context.Table.ToListAsync());
        }

        //GET: Table (by area)
        public async Task<IActionResult> IndexArea(Table.AreaEnum area)
        {
            var filter = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();
            return View(filter);
        }

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

        // GET: Table/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Table/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Table table)
        {
            //var tableInArea = await _context.Table.AsQueryable().Where(a => a.Area == table.Area).ToListAsync();
            //int tbNum = tableInArea.Count() + 1;
            //if (table.Area == Table.AreaEnum.Main)
            //{
            //    table.TableNum = "M" + tbNum.ToString();
            //    if (table.Seats == null)
            //    {
            //        table.Seats = 4;
            //    }
            //}
            //else if (table.Area == Table.AreaEnum.Balcony)
            //{
            //    table.TableNum = "B" + tbNum.ToString();
            //    if (table.Seats == null)
            //    {
            //        table.Seats = 2;
            //    }
            //}
            //else if (table.Area == Table.AreaEnum.Outside)
            //{
            //    table.TableNum = "O" + tbNum.ToString();
            //    if (table.Seats == null)
            //    {
            //        table.Seats = 2;
            //    }
            //}

            //ModelState.Clear();
            //TryValidateModel(table);
            
            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        //POST: Table/Create (QuickAdd)
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickAdd(Table.AreaEnum area)
        {
            Table table = null;
            table.Area = area;
            var tableInArea = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();
            int tbNum = tableInArea.Count() + 1;
            if (table.Area == Table.AreaEnum.Main)
            {
                table.TableNum = "M" + tbNum.ToString();
                if (table.Seats == null)
                {
                    table.Seats = 4;
                }
            }
            else if (table.Area == Table.AreaEnum.Balcony)
            {
                table.TableNum = "B" + tbNum.ToString();
                if (table.Seats == null)
                {
                    table.Seats = 2;
                }
            }
            else if (table.Area == Table.AreaEnum.Outside)
            {
                table.TableNum = "O" + tbNum.ToString();
                if (table.Seats == null)
                {
                    table.Seats = 2;
                }
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

        //POST: Table/Delete (QuickRemove)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickRemove(Table.AreaEnum area)
        {
            var tableInArea = await _context.Table.AsQueryable().Where(a => a.Area == area).ToListAsync();
            Table table = tableInArea.LastOrDefault();
            if (table != null)
            {
                //_context.Table.Remove(table);
                return RedirectToAction(nameof(Index));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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

        // POST: Table/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Table table)
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

        // GET: Table/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
    }
}
