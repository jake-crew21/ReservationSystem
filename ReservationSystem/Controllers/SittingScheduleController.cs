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
        public async Task<IActionResult> Index()
        {
              return View(await _context.SittingSchedule.ToListAsync());
        }

        // GET: SittingSchedule/Details/5
        public async Task<IActionResult> Details(int id, DateOnly sd)
        {
            if ((id == null && sd == null) || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule
                .FirstOrDefaultAsync(m => ((int)m.SessionType) == id && m.StartDate == sd);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EndDate,StartTime,EndTime,SessionType,DayOfWeek")] SittingSchedule sittingSchedule)
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
        public async Task<IActionResult> Edit(int id, DateOnly sd)
        {
            if ((id == null && sd == null) || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule.FindAsync(id, sd);
            if (sittingSchedule == null)
            {
                return NotFound();
            }
            return View(sittingSchedule);
        }

        // POST: SittingSchedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int sessionTypeId, DateOnly sd, [Bind("StartDate,EndDate,StartTime,EndTime,SessionType,DayOfWeek")] SittingSchedule sittingSchedule)
        {
            if (sessionTypeId != ((int)sittingSchedule.SessionType) && sd != sittingSchedule.StartDate)
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
                    if (!SittingScheduleExists(((int)sittingSchedule.SessionType), sittingSchedule.StartDate))
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
        public async Task<IActionResult> Delete(int sessionTypeId, DateOnly sd)
        {
            if ((sessionTypeId == null && sd == null) || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule
                .FirstOrDefaultAsync(m => ((int)m.SessionType) == sessionTypeId && m.StartDate == sd);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            return View(sittingSchedule);
        }

        // POST: SittingSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, DateOnly sd)
        {
            if (_context.SittingSchedule == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SittingSchedule'  is null.");
            }
            var sittingSchedule = await _context.SittingSchedule.FindAsync(id, sd);
            if (sittingSchedule != null)
            {
                _context.SittingSchedule.Remove(sittingSchedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingScheduleExists(int sessionTypeId, DateOnly sd)
        {
          return _context.SittingSchedule.Any(e => ((int)e.SessionType) == sessionTypeId && e.StartDate == sd);
        }
    }
}
