using Microsoft.AspNetCore.Mvc;
using InterviewBackend.Models;
using System.Runtime.InteropServices;

[ApiController]
[Route("api/question")]
public class QuestionController : ControllerBase
{
    // private static readonly List<Question> questions = new()
    // {
    //     new Question { Id = 1, Text = "Capital of India?", Options = new List<string>{"Delhi", "Mumbai", "Kolkata", "Chennai"}, CorrectOption = 0 },
    //     new Question { Id = 2, Text = "5 + 5 = ?", Options = new List<string>{"8", "9", "10", "11"}, CorrectOption = 2 }
    // };

    // [HttpGet]
    // public IActionResult GetQuestions() => Ok(questions);

    [HttpPost("submit")]
    public IActionResult SubmitAnswers([FromBody] Dictionary<int, int> answers)
    {
        // int score = 0;
        // foreach (var q in questions)
        // {
        //     if (answers.TryGetValue(q.Id, out int selected) && selected == q.CorrectOption)
        //         score++;
        // }
         int score2 = 0;
        foreach (var q in answers)
        {
            if (answers.TryGetValue(q.Value, out int selected) && selected == q.Value)
                score2++;

            Console.WriteLine(score2);
           
        
        }

        return Ok(new { score2 });
    }
}
