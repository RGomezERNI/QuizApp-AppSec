using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizAppAPI.Data;
using QuizAppAPI.Model.DTO.QandAs;
using QuizAppAPI.Model.DTO.QandAsDTOs;
using QuizAppAPI.Model.DTO.Quiz;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QandAController : ControllerBase
    {
        private readonly QuizDbContext _quizDbContext;
        private readonly IMapper _QandAmapper;

        public QandAController(QuizDbContext quizDbContext, IMapper QandAmapper)
        {
            _quizDbContext = quizDbContext;
            _QandAmapper = QandAmapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QandAInfoDTO>>> GetAllQandA()
        {
            var quizItems = await _quizDbContext.QandAs.Include(q => q.Quizzes).ToListAsync();
            var response = _QandAmapper.Map<IEnumerable<QuizInfoDTO>>(quizItems);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QandAInfoDTO>> GetQandAById(int id)
        {
            var quizItems = await _quizDbContext.QandAs
            .Include(q => q.Quizzes) // Include the related User entity
            .FirstOrDefaultAsync(q => q.Id == id);

            if (quizItems == null)
            {
                return NotFound();
            }
            var response = _QandAmapper.Map<QuizInfoDTO>(quizItems);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<QandAInfoDTO>>> CreateQandA(List<AddUpQandADTO> qnaList)
        {
            var test = qnaList;
            if (qnaList == null || !qnaList.Any())
            {
                return BadRequest("Invalid user data.");
            }

            var createdItems = new List<QandA>();

            foreach (var qna in qnaList)
            {
                if (string.IsNullOrEmpty(qna.Question) || string.IsNullOrEmpty(qna.Answer))
                {
                    return BadRequest("Invalid question or answer.");
                }

                var addQuizItem = new QandA
                {
                    QuizId = qna.QuizId,
                    Question = qna.Question,
                    Answer = qna.Answer,
                };

                _quizDbContext.QandAs.Add(addQuizItem);
                createdItems.Add(addQuizItem);
            }

            await _quizDbContext.SaveChangesAsync();

            var addQuizResponses = createdItems.Select(item => _QandAmapper.Map<QandAInfoDTO>(item)).ToList();

            return CreatedAtAction(nameof(GetQandAById), new { id = createdItems.First().Id }, addQuizResponses);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQandA(int id, [FromBody] AddUpQandADTO updated_qna)
        {

            var qna = await _quizDbContext.QandAs.Where(qna => qna.Id == id).FirstOrDefaultAsync();
            if (qna == null)
            {
                return NotFound();
            }

            qna.Question = updated_qna.Question;
            qna.Answer = updated_qna.Answer;

            _quizDbContext.Update(qna);
            await _quizDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQandA(int id)
        {
            var qna = await _quizDbContext.QandAs.FindAsync(id);
            if (qna == null)
            {
                return NotFound();
            }

            _quizDbContext.QandAs.Remove(qna);
            await _quizDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
