using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizAppAPI.Data;
using QuizAppAPI.Model.DTO.QandAsDTOs;
using QuizAppAPI.Model.DTO.Quiz;
using QuizAppAPI.Model.DTO.QuizDTOs;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizAppController : ControllerBase
    {
        private readonly QuizDbContext _quizDbContext;
        private readonly IMapper _quizzesMapper;   
        public QuizAppController(QuizDbContext quizDbContext, IMapper quizzesMapper)
        {
            _quizDbContext = quizDbContext;
            _quizzesMapper = quizzesMapper;
        }

        [HttpGet]
        //[Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<QuizInfoDTO>>> GetAllQuizzes()
        {
            var quizzes = await _quizDbContext.Quizzes.Include(q => q.Users).Include(q => q.QandAs).ToListAsync();
            var response = _quizzesMapper.Map<IEnumerable<QuizInfoDTO>>(quizzes);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizInfoDTO>> GetQuizById(int id)
        {
            var quiz = await _quizDbContext.Quizzes
            .Include(q => q.Users) // Include the related User entity
            .Include(q => q.QandAs) // Optionally include QandAs if needed
            .FirstOrDefaultAsync(q => q.QuizId == id); // Use FirstOrDefaultAsync instead of FindAsync

            if (quiz == null)
            {
                return NotFound();
            }
            var response = _quizzesMapper.Map<QuizInfoDTO>(quiz);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<AddUpQuizDTO>> CreateQuiz(AddUpQuizDTO quiz)
        {

            if (quiz == null || string.IsNullOrEmpty(quiz.QuizName) || string.IsNullOrEmpty(quiz.QuizTopic))
            {
                return BadRequest("Invalid user data.");
            }

            var addQuiz = new Quiz
            {
                UserId = quiz.UserId,
                QuizName = quiz.QuizName,
                QuizTopic = quiz.QuizTopic,
            };

            _quizDbContext.Quizzes.Add(addQuiz);
            await _quizDbContext.SaveChangesAsync();

            var addQuizResponse = _quizzesMapper.Map<AddUpQuizDTO>(addQuiz);

            return CreatedAtAction(nameof(GetQuizById), new { id = addQuiz.QuizId }, addQuizResponse);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] PatchQuizDTO updated_quiz)
        {

            var quiz = await _quizDbContext.Quizzes.Where(quiz => quiz.QuizId == id).FirstOrDefaultAsync();
            if (quiz == null)
            {
                return NotFound();
            }

            quiz.QuizName = updated_quiz.QuizName;
            quiz.QuizTopic= updated_quiz.QuizTopic;

            _quizDbContext.Update(quiz);
            await _quizDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _quizDbContext.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _quizDbContext.Quizzes.Remove(quiz);
            await _quizDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
