using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcProject.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyMvcProject.Controllers
{
    public class WorkServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // File save karne ke path ke liye

        public WorkServiceController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // ==========================================
        // Sabhi records ki list dekhne ke liye
        // ==========================================
        public async Task<IActionResult> List()
        {
            _context.Database.EnsureCreated();
            var records = await _context.WorkServiceForms.ToListAsync();
            return View(records);
        }

        // ==========================================
        // Naya form kholne ke liye (GET)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Designations = new SelectList(await _context.Designations.ToListAsync(), "Name", "Name");
            return View(new WorkServiceFormViewModel());
        }

        // ==========================================
        // Naya form save karne ke liye (POST)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkServiceFormViewModel model)
        {
            ModelState.Remove("Id"); 
            ModelState.Remove("EstimatedCost"); 
            ModelState.Remove("ProfileImage"); // File upload validation error se bachne ke liye

            if (ModelState.IsValid)
            {
                _context.Database.EnsureCreated();
                
                // Agar user ne form submit karte waqt image bhi select ki ho
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(fileStream);
                    }

                    model.ProfilePicPath = "/Images/" + uniqueFileName;
                }

                _context.WorkServiceForms.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Form Data Saved Successfully!";
                return RedirectToAction(nameof(List));
            }

            ViewBag.Designations = new SelectList(await _context.Designations.ToListAsync(), "Name", "Name", model.Designation);
            return View(model);
        }

        // ==========================================
        // Record edit karne ke liye (GET)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var record = await _context.WorkServiceForms.FindAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            ViewBag.Designations = new SelectList(await _context.Designations.ToListAsync(), "Name", "Name", record.Designation);
            return View("Create", record);
        }

        // ==========================================
        // Record update karne ke liye (POST)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkServiceFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Id");
            ModelState.Remove("EstimatedCost");
            ModelState.Remove("ProfileImage");

            if (ModelState.IsValid)
            {
                try
                {
                    // Agar purani image ke sath nayi image bhi select ki gayi hai
                    if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ProfileImage.CopyToAsync(fileStream);
                        }

                        model.ProfilePicPath = "/Images/" + uniqueFileName;
                    }
                    else
                    {
                        // Agar nayi image select nahi ki to purana path maintain rakhein
                        var existingRecord = await _context.WorkServiceForms.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                        if (existingRecord != null)
                        {
                            model.ProfilePicPath = existingRecord.ProfilePicPath;
                        }
                    }

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Record Successfully Updated!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.WorkServiceForms.Any(e => e.Id == model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }

            ViewBag.Designations = new SelectList(await _context.Designations.ToListAsync(), "Name", "Name", model.Designation);
            return View("Create", model);
        }

        // ==========================================
        // 🔥 AJAX Method: Profile Picture Instant Upload ke liye 🔥
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> UploadProfilePic(IFormFile ProfileImage)
        {
            try
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ProfileImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                    string relativePath = "/Images/" + uniqueFileName;
                    return Json(new { success = true, imgPath = relativePath });
                }

                return Json(new { success = false, message = "Please select a valid image." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ==========================================
        // Record delete karne ke liye
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.WorkServiceForms.FindAsync(id);
            if (record != null)
            {
                _context.WorkServiceForms.Remove(record);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Record Deleted Successfully";
            }
            return RedirectToAction(nameof(List));
        }

        // ==========================================
        // View Details Record
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workServiceForm = await _context.WorkServiceForms 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workServiceForm == null)
            {
                return NotFound();
            }

            return View(workServiceForm);
        }

        // ==========================================
        // Designations manage karne ka panel
        // ==========================================
        public async Task<IActionResult> ManageDesignations()
        {
            _context.Database.EnsureCreated();
            var designations = await _context.Designations.ToListAsync();
            return View(designations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDesignation(string designationName)
        {
            if (!string.IsNullOrWhiteSpace(designationName))
            {
                bool exists = await _context.Designations.AnyAsync(d => d.Name == designationName);
                if (!exists)
                {
                    _context.Designations.Add(new Designation { Name = designationName });
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Successfully Added!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Already Exists";
                }
            }
            return RedirectToAction(nameof(ManageDesignations));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var designation = await _context.Designations.FindAsync(id);
            if (designation != null)
            {
                _context.Designations.Remove(designation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Designation kamyabi se delete ho gaya!";
            }
            return RedirectToAction(nameof(ManageDesignations));
        }
    }
}