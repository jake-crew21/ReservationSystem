using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public SittingScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        // GET: SittingSchedule/Create
        public IActionResult Create()
        {
            return View();
        }

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

        // GET: SittingSchedule/Edit/5
        public async Task<IActionResult> Edit(SittingSchedule.SessionEnum sessionTypeId)
        {
            if (sessionTypeId == null || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule.FindAsync(sessionTypeId);
            if (sittingSchedule == null)
            {
                return NotFound();
            }
            return View(sittingSchedule);
        }

        // POST: SittingSchedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SittingSchedule.SessionEnum sessionTypeId, SittingSchedule sittingSchedule)
        {
            if (sessionTypeId != sittingSchedule.SessionType)
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
                    if (!SittingScheduleExists((int)sittingSchedule.SessionType))
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
    }
}
