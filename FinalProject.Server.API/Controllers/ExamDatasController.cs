using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Server.API.Context;
using FinalProject.Server.API.Models;

namespace FinalProject.Server.API.Controllers
{
    //This class will handle all types of calls to the exam data table

    [Route("api/[controller]")]
    [ApiController]
    public class ExamDatasController : ControllerBase
    {
        private readonly UsersContext _context;

        public ExamDatasController(UsersContext context)
        {
            _context = context;
        }

        // GET: api/ExamDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamData>>> GetExamData()
        {
            return await _context.ExamData.ToListAsync();
        }

        // GET: api/ExamDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamData>> GetExamData(string id)
        {
            var examData = await _context.ExamData.FindAsync(id);

            if (examData == null)
            {
                return NotFound();
            }

            return examData;
        }

        // PUT: api/ExamDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExamData(string id, ExamData examData)
        {
            if (id != examData.Id)
            {
                return BadRequest();
            }

            _context.Entry(examData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ExamDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExamData>> PostExamData(ExamData examData)
        {
            _context.ExamData.Add(examData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExamData", new { id = examData.Id }, examData);
        }

        // DELETE: api/ExamDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamData(string id)
        {
            var examData = await _context.ExamData.FindAsync(id);
            if (examData == null)
            {
                return NotFound();
            }

            _context.ExamData.Remove(examData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExamDataExists(string id)
        {
            return _context.ExamData.Any(e => e.Id == id);
        }
    }
}
