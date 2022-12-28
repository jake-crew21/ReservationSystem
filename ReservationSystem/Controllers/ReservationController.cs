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
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;

namespace ReservationSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ReservationController()
        {
        }

        /// <summary>
        /// Get all reservations
        /// </summary>
        /// <returns>All reservations from database</returns>
        // GET: Reservation
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> Index()
        {
              return _context.Reservation != null ? 
                          View(await _context.Reservation.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
        }
        /// <summary>
        /// Get all reservations made by user
        /// </summary>
        /// <returns>All reservations made by user from database</returns>
        //GET: Reservation    --only made by the "User"
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserIndex()
        {
            var userId = User.Identity.GetUserId();
            var user = await _context.Users.FindAsync(userId);

            var filtered = await _context.Reservation.AsQueryable().Where(r => r.Id == userId).ToListAsync();
            return View(filtered);
        }
        /// <summary>
        /// Displays individual reservation
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Displays selected reservation</returns>
        // GET: Reservation/Details/5
        [Authorize(Roles = "Admin, Staff, User")]
        public async Task<IActionResult> Details(string bookingId)
        {
            if (bookingId == null || _context.Reservation == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservation
                .FindAsync(bookingId);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }
        /// <summary>
        /// Empty model to create new reservation for "User"
        /// </summary>
        /// <returns>Emtpy reservation Model</returns>
        // GET: Reservation/Create      --User Create
        [Authorize(Roles = "User")]
        public IActionResult ReservationRequest()
        {
            return View();
        }
        /// <summary>
        /// User reservation Create, Status is set to "requested"
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns>Adds reservation with "requested" status to database</returns>
        // POST: Reservation/Create     --User
        [HttpPost]
        [Authorize(Roles = "User")]
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
            if (this.User.IsInRole("User"))
            {
                reservation.Source = "Online";
            }
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserIndex));
        }
        /// <summary>
        /// Empty model to create new reservation
        /// </summary>
        /// <returns>Emtpy reservation Model</returns>
        // GET: Reservation/Create      --Staff/Manager
        [Authorize(Roles = "Admin, Staff")]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Creates new reservation entry
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns>Adds reservation to database</returns>
        // POST: Reservation/Create     --Staff/Manager
        [HttpPost]
        [Authorize(Roles = "Admin, Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
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
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Index));
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
        /// <summary>
        /// Displays selected reservation with editable fields
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Editable reservation</returns>
        // GET: Reservation/Edit/5
        [Authorize(Roles = "Admin, Staff, User")]
        public async Task<IActionResult> Edit(int bookingId)
        {
            if (bookingId == null || _context.Reservation == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservation
                .FindAsync(bookingId);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }
        /// <summary>
        /// Changes selected reservation status to "Approved"
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Changes selected reservation status to "Approved"</returns>
        //PUT: Reservation/Approve
        [Authorize(Roles = "Admin, Staff, User")]
        public async Task<IActionResult> Approve(int bookingId)
        {
            if (bookingId == null || _context.Reservation == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservation
                .FindAsync(bookingId);
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
                if (!ReservationExists(reservation.BookingId))
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
        /// Updates displayed reservation
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="reservation"></param>
        /// <returns>Updates reservation, redirects to index</returns>
        // POST: Reservation/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Staff, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int bookingId, Reservation reservation)
        {
            if (bookingId != reservation.BookingId)
            {
                return NotFound();
            }
            
            try
            {
                reservation.StartTime = TimeOnly.Parse(reservation.DateTime.ToShortTimeString());
                reservation.ResDate = DateOnly.Parse(reservation.DateTime.ToShortDateString());
                reservation.NoOfTable = CheckTbl(reservation.NoOfPpl);
                reservation.EndTime = reservation.StartTime.AddHours(2);
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(reservation.BookingId))
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
        /// Displayes selected reservation with ability to delete
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>Displayes selected reservation</returns>
        // GET: Reservation/Delete/5
        [Authorize(Roles = "Admin, Staff, User")]
        public async Task<IActionResult> Delete(int bookingId)
        {
            if (bookingId == null || _context.Reservation == null)
            {
                return NotFound();
            }
            var reservation = await _context.Reservation
                .FindAsync(bookingId);
            if (reservation != null)
            {
                return View(reservation);
            }
            return NotFound();
        }
        /// <summary>
        /// Removes selected reservation to database
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>once reservation is removed, redirected to index</returns>
        // DELETE: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, Staff, User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int bookingId)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(bookingId);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool ReservationExists(int id)
        {
          return (_context.Reservation?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
    }
}
