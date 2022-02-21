using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSPApplication.Data;
using MSPApplication.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.Api.Controllers
{
	public class AnswersController : Controller
	{
		private readonly AppDbContext _context;

		public AnswersController(AppDbContext context)
		{
			_context = context;
		}

		// GET: Answers
		public async Task<IActionResult> Index()
		{
			var appDbContext = _context.Answers.Include(a => a.Survey);
			return View(await appDbContext.ToListAsync());
		}

		// GET: Answers/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var answer = await _context.Answers
				.Include(a => a.Survey)
				.FirstOrDefaultAsync(m => m.AnswerId == id);
			if (answer == null)
			{
				return NotFound();
			}
			return View(answer);
		}

		// GET: Answers/Create
		public IActionResult Create()
		{
			ViewData["SurveyId"] = new SelectList(_context.Surveys, "SurveyId", "SurveyId");
			return View();
		}

		// POST: Answers/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("AnswerId,Response,Rating,SurveyId")] Answer answer)
		{
			if (ModelState.IsValid)
			{
				_context.Add(answer);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["SurveyId"] = new SelectList(_context.Surveys, "SurveyId", "SurveyId", answer.SurveyId);
			return View(answer);
		}

		// GET: Answers/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var answer = await _context.Answers.FindAsync(id);
			if (answer == null)
			{
				return NotFound();
			}
			ViewData["SurveyId"] = new SelectList(_context.Surveys, "SurveyId", "SurveyId", answer.SurveyId);
			return View(answer);
		}

		// POST: Answers/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("AnswerId,Response,Rating,SurveyId")] Answer answer)
		{
			if (id != answer.AnswerId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(answer);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AnswerExists(answer.AnswerId))
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
			ViewData["SurveyId"] = new SelectList(_context.Surveys, "SurveyId", "SurveyId", answer.SurveyId);
			return View(answer);
		}

		// GET: Answers/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var answer = await _context.Answers
				.Include(a => a.Survey)
				.FirstOrDefaultAsync(m => m.AnswerId == id);
			if (answer == null)
			{
				return NotFound();
			}

			return View(answer);
		}

		// POST: Answers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var answer = await _context.Answers.FindAsync(id);
			_context.Answers.Remove(answer);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool AnswerExists(int id)
		{
			return _context.Answers.Any(e => e.AnswerId == id);

		}
	}
}
