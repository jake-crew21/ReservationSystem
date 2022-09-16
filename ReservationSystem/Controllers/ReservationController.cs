using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Data;
using ReservationSystem.Models;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace ReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
              return _context.Reservation != null ? 
                          View(await _context.Reservation.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
        }
        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Contact == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Request(Reservation reservation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservation = new Reservation
            {
                Id = CheckUId(userId),
                NoOfTable = CheckTbl(reservation.NoOfPpl),
                EndTime = reservation.StartTime.AddHours(2),
                BookingStatus = Reservation.StatusEnum.Requested,
                SessionType = (Reservation.SessionEnum)CheckSession(reservation.StartTime)
            };
            _context.Add(reservation);
            return View();
        }
        public int CheckSession(TimeOnly startT)
        {
            TimeSpan ts = startT.ToTimeSpan();
            bool bt = startT.IsBetween(new TimeOnly(07,00), new TimeOnly(09,00));
            bool lt = startT.IsBetween(new TimeOnly(11, 00), new TimeOnly(15,00));
            bool dt = startT.IsBetween(new TimeOnly(18, 00), new TimeOnly(22,00));
            if (bt)
            {
                return 0;
            }
            else if (lt)
            {
                return 1;
            }
            else if (dt)
            {
                return 2;
            }
            return 0;
        }
        public string CheckUId (string id)
        {
            if (id != null)
            {
                return id;
            }
            return "";
        }
        public int CheckTbl (int ppl)
        {
            decimal num = ppl / 4;
            if (num <= 1)
            {
                return 1;
            }
            else
            {
                int fNum = (int)Math.Ceiling(num);
                return fNum;
            }
        }
        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Contact,NoOfPpl,NoOfTable,ResDate,StartTime,EndTime,Duration,Notes,Source,BookingStatus,SessionType,Area")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Contact,NoOfPpl,NoOfTable,ResDate,StartTime,EndTime,Duration,Notes,Source,BookingStatus,SessionType,Area")] Reservation reservation)
        {
            if (id != reservation.Contact)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Contact))
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
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Contact == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(string id)
        {
          return (_context.Reservation?.Any(e => e.Contact == id)).GetValueOrDefault();
        }
    }
}
