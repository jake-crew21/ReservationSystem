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
using ReservationSystem.ViewModels;

namespace ReservationSystem.Controllers
{
    public class SittingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SittingController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get all sittings
        /// </summary>
        /// <returns></returns>
        // GET: Sitting
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sitting.Include(s => s.Reservation).Include(s => s.Table);
            return View(await applicationDbContext.ToListAsync());
        }
        /// <summary>
        /// Displays selected sitting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Sitting/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sitting == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sitting
                .Include(s => s.Reservation)
                .Include(s => s.Table)
                .FirstOrDefaultAsync(m => m.TableId == id);
            if (sitting == null)
            {
                return NotFound();
            }

            return View(sitting);
        }
        /// <summary>
        /// displays empty sitting model
        /// </summary>
        /// <returns></returns>
        // GET: Sitting/Create
        public IActionResult Create()
        {
            var model = new SittingViewModel()
            {
                Reservation = _context.Reservation.ToList(),
                Table = _context.Table.ToList()
            };
            return View(model);
        }
        /// <summary>
        /// Displays all reservations with dropdown to select one table
        /// </summary>
        /// <returns></returns>
        // GET: Sitting/AssignTable
        public IActionResult AssignTable()
        {
            List<Table> table = _context.Table.ToList();
            List<Sitting> sitting = _context.Sitting.ToList();
            if (sitting != null)
            {
                foreach (var t in table)
                {
                    bool match = false;
                    foreach (var s in sitting)
                    {
                        if (t.Id == s.TableId)
                        {
                            table.Remove(t);
                            match = true;
                            break;
                        }
                    }
                    if (match)
                    {
                        break;
                    }
                }
            }

            var model = new SittingViewModel()
            {
                Table = table,
                Reservation = _context.Reservation.ToList()
            };
            return View(model);
        }
        /// <summary>
        /// Creates new sitting by adding selected reservation and table
        /// </summary>
        /// <param name="sittingVM"></param>
        /// <returns></returns>
        // POST: Sitting/AssignTable
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTable(SittingViewModel sittingVM)
        {
            var sitting = new Sitting
            {
                TableId = sittingVM.Sitting.TableId,
                Table = await _context.Table.FindAsync(sittingVM.Sitting.TableId),
                BookingId = sittingVM.Sitting.BookingId,
                Reservation = await _context.Reservation.FindAsync(sittingVM.Sitting.BookingId)
            };
            ModelState.Clear();
            TryValidateModel(sitting);
            if (ModelState.IsValid)
            {
                _context.Add(sitting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            sittingVM.Reservation = _context.Reservation.ToList();
            sittingVM.Table = _context.Table.ToList();
            return View(sitting);
        }
        /// <summary>
        /// adds new reservation with selected reservation adn table
        /// </summary>
        /// <param name="sittingVM"></param>
        /// <returns></returns>
        // POST: Sitting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SittingViewModel sittingVM)
        {
            var sitting = new Sitting
            {
                TableId = sittingVM.Sitting.TableId,
                Table = sittingVM.Sitting.Table,
                BookingId = sittingVM.Sitting.BookingId,
                Reservation = sittingVM.Sitting.Reservation
            };
            if (ModelState.IsValid)
            {
                _context.Add(sitting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            sittingVM.Reservation = _context.Reservation.ToList();
            sittingVM.Table = _context.Table.ToList();
            return View(sitting);
        }
        /// <summary>
        /// Display selected sitting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Sitting/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sitting == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sitting.FindAsync(id);
            if (sitting == null)
            {
                return NotFound();
            }
            return View(sitting);
        }
        /// <summary>
        /// Updates selected sitting
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sitting"></param>
        /// <returns></returns>
        // POST: Sitting/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sitting sitting)
        {
            if (id != sitting.TableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sitting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SittingExists(sitting.TableId))
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
            return View(sitting);
        }

        /// <summary>
        /// displays selected sitting
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        // GET: Sitting/Delete/5
        public async Task<IActionResult> Delete(int id, int bookingId)
        {
            if (id == null || _context.Sitting == null)
            {
                return NotFound();
            }

            var sitting = await _context.Sitting
                .Include(s => s.Reservation)
                .Include(s => s.Table)
                .FirstOrDefaultAsync(m => m.TableId == id);
            if (sitting != null)
            {
                return View(sitting);
            }

            return View(sitting);
        }
        /// <summary>
        /// removes selected sitting
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="bookingId"></param>
        /// <returns>Once sitting is Deleted, redirected to index</returns>
        // POST: Sitting/Unassign
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int tableId, int bookingId)
        {
            var s = await _context.Sitting.FindAsync(tableId, bookingId);

            if (s != null)
            {
                _context.Sitting.Remove(s);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingExists(int id)
        {
          return _context.Sitting.Any(e => e.TableId == id);
        }
    }
}
