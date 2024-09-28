using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using OpenAISemanticKernelPoc.Models;
using OpenAISemanticKernelPoc.Services;

namespace OpenAISemanticKernelPoc.Controllers
{
  
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        [Authorize(Roles = "Task.Administrator")]
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        public IActionResult GetActionDetails(string certificateType)
        {
            // Assuming we have Entity Framework models for the tables
            var results = from cr in _context.CommodityImportRules
                        where cr.IsActive == 1
                                          && cr.Regulator == certificateType
                                          && cr.VarietyId == "0"
                                          && cr.Eppo == ""
                                          && (
                                              cr.Permanent == 1 ||
                                              (cr.Permanent == 0 && cr.StartDate == null && cr.EndDate == null) ||
                                              (cr.Permanent == 0 && cr.StartDate == null && cr.EndDate >= DateTime.UtcNow) ||
                                              (cr.Permanent == 0 && cr.StartDate <= DateTime.UtcNow && cr.EndDate == null) ||
                                              (cr.Permanent == 0 && cr.StartDate <= DateTime.UtcNow && cr.EndDate >= DateTime.UtcNow)
                                          )
                        // LEFT JOIN for Border Control Posts
                        join crbcp in _context.CommodityImportRuleBorderControlPosts
                            on cr.Id equals crbcp.RuleId into borderControlPostGroup
                        from borderControlPost in borderControlPostGroup.DefaultIfEmpty()

                            // LEFT JOIN for Purpose
                        join crp in _context.CommodityImportRulePurposes
                            on cr.Id equals crp.RuleId into purposeGroup
                        from crPurpose in purposeGroup.DefaultIfEmpty()
                        join cp in _context.CommodityImportPurposes
                            on crPurpose.PurposeId equals cp.Id into commodityPurposeGroup
                        from commodityPurpose in commodityPurposeGroup.DefaultIfEmpty()

                            // LEFT JOIN for Certified Use
                        join crcu in _context.CommodityImportRuleCertifiedUses
                            on cr.Id equals crcu.RuleId into certifiedUseGroup
                        from crCertifiedUse in certifiedUseGroup.DefaultIfEmpty()
                        join ccu in _context.CommodityImportCertifiedUses
                            on crCertifiedUse.CertifiedUseId equals ccu.Id into certifiedUseGroupFinal
                        from certifiedUse in certifiedUseGroupFinal.DefaultIfEmpty()

                        select new
                        {
                            cr.Id,
                            cr.CommodityId,
                            cr.Rate,
                            cr.Triggered,
                            cr.Total,
                            BorderControlPost = borderControlPost != null ? borderControlPost.BcpCode : null,
                            Purpose = commodityPurpose != null ? commodityPurpose.Purpose : null,
                            CertifiedUse = certifiedUse != null ? certifiedUse.CertifiedFor : null,
                            cr.RiskCategorisation,
                            cr.AllowMultipleInspections,

                            // CountriesFlat: Concatenating allowed countries
                            CountriesFlat = String.Join(",",
                                (from crcCountry in _context.CommodityImportRuleCountries
                                 where crcCountry.Type == 1 && crcCountry.RuleId == cr.Id
                                 join excludedCountry in _context.CommodityImportRuleExceptions
                                     on new { crcCountry.RuleId, crcCountry.CountryOrGroupId } equals
                                        new { excludedCountry.RuleId, excludedCountry.CountryOrGroupId }
                                        into excludedGroup
                                 from excluded in excludedGroup.DefaultIfEmpty()
                                 where excluded == null // exclude countries found in exceptions
                                 select crcCountry.CountryOrGroupId.ToString()).Distinct()),

                            // ExcludedCountriesFlat: Concatenating excluded countries
                            ExcludedCountriesFlat = String.Join(",",
                                (from excludedCountry in _context.CommodityImportRuleExceptions
                                 where excludedCountry.RuleId == cr.Id && excludedCountry.Type == 2
                                 select excludedCountry.CountryOrGroupId.ToString()).Distinct()),

                            // CountryGroupCountriesFlat: Concatenating countries in groups
                            CountryGroupCountriesFlat = String.Join(",",
                                (from crcCountryGroup in _context.CommodityImportRuleCountries
                                 where crcCountryGroup.Type == 2 && crcCountryGroup.RuleId == cr.Id
                                 join cgm in _context.CountryGroupsMappings
                                     on crcCountryGroup.CountryOrGroupId equals cgm.CountryGroupsId
                                 join excludedCountry in _context.CommodityImportRuleExceptions
                                     on cgm.CountryId equals excludedCountry.CountryOrGroupId into excludedGroup
                                 from excluded in excludedGroup.DefaultIfEmpty()
                                 where excluded == null // exclude countries found in exceptions
                                 select cgm.CountryId.ToString()).Distinct())
                        };

            return (IActionResult)results;

        }

        // GET: Students/Create
        //get1 check
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Age")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}

