using InboxEngine.Exceptions;
using InboxEngine.Models;
using InboxEngine.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace InboxEngine.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InboxController : ControllerBase
{
    private readonly IPriorityScoringService _scoringService;
    private readonly ILogger<InboxController> _logger;

    public InboxController(IPriorityScoringService scoringService, ILogger<InboxController> logger)
    {
        _scoringService = scoringService;
        _logger = logger;
    }

    /// <summary>
    /// Set today's date in the Scoring Service.
    /// The mechanism to set today's date is to facilitate testing of calculating priority scores based on elapsed hours.
    /// </summary>
    /// <param name="todaysDate">Today's date - in YYYYMMDDThh:mi:ssZ format</param>
    /// <returns>
    /// 400 Bad Request - if Date string is null
    /// 200 OK - If parsed date is in valid format, set time; else, use today's time
    /// </returns>
    [HttpPost("setTodaysDate")]
    public IActionResult setTodaysDate([FromBody] string todaysDate)
    {
        if(todaysDate == null || todaysDate == "")
        {
            return BadRequest();
        }
        else
        {
            DateTime todaysDateResult;

            if (DateTime.TryParse(todaysDate, out todaysDateResult))
            {
                _scoringService.setTodaysTime(todaysDateResult);
            }
            else
            {
                _scoringService.setTodaysTime(DateTime.Now);
            }


            return Ok($"Date successfully set to {_scoringService.getTodaysTime()}");
        }
    }

    /// <summary>
    /// Get today's date
    /// </summary>
    /// <returns>
    /// 200 OK - Today's date
    /// </returns>
    [HttpGet("getTodaysDate")]
    public ActionResult<DateTime> getTodaysDate()
    {
        DateTime dateTime = _scoringService.getTodaysTime();
        return dateTime;
    }


    /// <summary>
    /// Get a list of emails and calculate their priority score in the PriorityScoringService,
    /// then returns the same list sorted by priority.
    /// </summary>
    /// <param name="emails">A list of emails in JSON format.</param>
    /// <returns>
    /// 400 Bad Request - if list of emails is empty, or list of emails contains emails with ReceivedAt date after Today's Date
    /// 200 OK - list of emails sorted by calculated priority score.
    /// </returns>
    [HttpPost("sort")]
    public IActionResult SortEmails([FromBody] List<Email> emails)
    {
        // TODO: Implement the endpoint logic:
        // 1. Validate the input (check for null or empty list)
        // 2. Calculate priority score for each email using _scoringService
        // 3. Sort emails by PriorityScore (highest first)
        // 4. Return the sorted list
        if(emails == null || emails.Count() == 0)
        {
            return BadRequest();
        }
        else
        {
            var emailsSortedByPriority = from email in emails
                                   let priority = _scoringService.CalculatePriorityScore(email)
                                   orderby priority descending
                                   select new
                                   {
                                       Subject = email.Subject,
                                       Sender = email.Sender,
                                       Body = email.Body,
                                       IsVIP = email.IsVIP,
                                       ReceivedAt = email.ReceivedAt,
                                       PriorityScore = priority
                                   };
            var emailsFromFuture = from email in emailsSortedByPriority where email.PriorityScore < 0 select email;
            if(emailsFromFuture.Count() > 0)
            {
                return BadRequest($"Emails that are later than today ({_scoringService.getTodaysTime()}) exist - please fix");

            }
            else
            { 
                return Ok(emailsSortedByPriority);
            }
        }

    }
}
