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
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace ReservationSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservation
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Index()
        {
              return _context.Reservation != null ? 
                          View(await _context.Reservation.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
        }

        //GET: Reservation    --only made by the "User"
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserIndex()
        {
            var userId = User.Identity.GetUserId();
            var user = await _context.Users.FindAsync(userId);

            var filtered = await _context.Reservation.AsQueryable().Where(r => r.Id == userId).ToListAsync();
            return View(filtered);
        }

        // GET: Reservation/Details/5
        [Authorize(Roles = "Admin, Staff, User")]
        public async Task<IActionResult> Details(string contact, DateTime date, DateTime time)
        {
            if ((contact == null || date == null || time == null) || _context.Reservation == null)
            {
                return NotFound();
            }
            DateOnly d = DateOnly.Parse(date.ToShortDateString());
            TimeOnly t = TimeOnly.Parse(time.ToShortTimeString());
            var reservation = await _context.Reservation
                .FindAsync(contact, d, t);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create      --User Create
        [Authorize(Roles = "User")]
        public IActionResult ReservationRequest()
        {
            return View();
        }

        // POST: Reservation/Create     --User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationRequest(Reservation reservation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservation.Id = CheckUId(userId);
            reservation.StartTime = TimeOnly.Parse(reservation.DateTime.ToShortTimeString());
            reservation.ResDate = DateOnly.Parse(reservation.DateTime.ToShortDateString());
            reservation.NoOfTable = CheckTbl(reservation.NoOfPpl);
            reservation.EndTime = reservation.StartTime.AddHours(2);
            reservation.BookingStatus = Reservation.StatusEnum.Requested;
            CheckSession(reservation.StartTime);
            if (this.User.IsInRole("User"))
            {
                reservation.Source = "Online";
            }
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserIndex));

            async void CheckSession(TimeOnly st)
            {
                var breakfast = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Breakfast);
                var lunch = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Lunch);
                var dinner = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Dinner);
                if (st.IsBetween(TimeOnly.FromTimeSpan(breakfast.StartTime), TimeOnly.FromTimeSpan(breakfast.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Breakfast;
                }
                else if (st.IsBetween(TimeOnly.FromTimeSpan(lunch.StartTime), TimeOnly.FromTimeSpan(lunch.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Lunch;
                }
                else if (st.IsBetween(TimeOnly.FromTimeSpan(dinner.StartTime), TimeOnly.FromTimeSpan(dinner.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Dinner;
                }
            }
        }

        // GET: Reservation/Create      --Staff/Manager
        [Authorize(Roles = "Admin, Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservation/Create     --Staff/Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    reservation.Id = CheckUId(userId);
                    reservation.StartTime = TimeOnly.Parse(reservation.DateTime.ToShortTimeString());
                    reservation.ResDate = DateOnly.Parse(reservation.DateTime.ToShortDateString());
                    reservation.NoOfTable = CheckTbl(reservation.NoOfPpl);
                    reservation.EndTime = reservation.StartTime.AddHours(2);
                    reservation.BookingStatus = Reservation.StatusEnum.Requested;
                    CheckSession(reservation.StartTime);
                    reservation.Source = "Online";
                    if (this.User.IsInRole("User"))
                    {
                        reservation.Source = "Online";
                    }
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return View(reservation);
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(Index);

            async void CheckSession(TimeOnly st)
            {
                var breakfast = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Breakfast);
                var lunch = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Lunch);
                var dinner = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Dinner);
                if (st.IsBetween(TimeOnly.FromTimeSpan(breakfast.StartTime), TimeOnly.FromTimeSpan(breakfast.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Breakfast;
                }
                else if (st.IsBetween(TimeOnly.FromTimeSpan(lunch.StartTime), TimeOnly.FromTimeSpan(lunch.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Lunch;
                }
                else if (st.IsBetween(TimeOnly.FromTimeSpan(dinner.StartTime), TimeOnly.FromTimeSpan(dinner.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Dinner;
                }
            }
        }
        public string CheckUId (string id)
        {
            if (id != null)
            {
                return id;
            }
            return id = "1";
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

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(string contact, DateTime date, DateTime time)
        {
            if ((contact == null || date == null || time == null) || _context.Reservation == null)
            {
                return NotFound();
            }
            DateOnly d = DateOnly.Parse(date.ToShortDateString());
            TimeOnly t = TimeOnly.Parse(time.ToShortTimeString());
            var reservation = await _context.Reservation
                .FindAsync(contact, d, t);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        //GET: Reservation/Approve
        public async Task<IActionResult> Approve(string contact, DateTime date, DateTime time)
        {
            if ((contact == null || date == null || time == null) || _context.Reservation == null)
            {
                return NotFound();
            }
            DateOnly d = DateOnly.Parse(date.ToShortDateString());
            TimeOnly t = TimeOnly.Parse(time.ToShortTimeString());
            var reservation = await _context.Reservation
                .FindAsync(contact, d, t);
            if (reservation == null)
            {
                return NotFound();
            }

            try
            {
                reservation.BookingStatus = Reservation.StatusEnum.Confirmed;
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

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string contact, Reservation reservation)
        {
            if (contact != reservation.Contact)
            {
                return NotFound();
            }
            
            try
            {
                reservation.StartTime = TimeOnly.Parse(reservation.DateTime.ToShortTimeString());
                reservation.ResDate = DateOnly.Parse(reservation.DateTime.ToShortDateString());
                reservation.NoOfTable = CheckTbl(reservation.NoOfPpl);
                reservation.EndTime = reservation.StartTime.AddHours(2);
                CheckSession(reservation.StartTime);
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

            async void CheckSession(TimeOnly st)
            {
                var breakfast = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Breakfast);
                var lunch = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Lunch);
                var dinner = await _context.SittingSchedule.FindAsync(SittingSchedule.SessionEnum.Dinner);
                if (st.IsBetween(TimeOnly.FromTimeSpan(breakfast.StartTime), TimeOnly.FromTimeSpan(breakfast.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Breakfast;
                }
                else if (st.IsBetween(TimeOnly.FromTimeSpan(lunch.StartTime), TimeOnly.FromTimeSpan(lunch.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Lunch;
                }
                else if (st.IsBetween(TimeOnly.FromTimeSpan(dinner.StartTime), TimeOnly.FromTimeSpan(dinner.EndTime)))
                {
                    reservation.SessionType = Reservation.SessionEnum.Dinner;
                }
            }
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(string contact, DateTime date, DateTime time)
        {
            if ((contact == null || date == null || time == null) || _context.Reservation == null)
            {
                return NotFound();
            }
            DateOnly d = DateOnly.Parse(date.ToShortDateString());
            TimeOnly t = TimeOnly.Parse(time.ToShortTimeString());
            var reservation = await _context.Reservation
                .FindAsync(contact, d, t);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string contact, DateTime date, DateTime time)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
            }
            DateOnly d = DateOnly.Parse(date.ToShortDateString());
            TimeOnly t = TimeOnly.Parse(time.ToShortTimeString());
            var reservation = await _context.Reservation.FindAsync(contact, d, t);
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
