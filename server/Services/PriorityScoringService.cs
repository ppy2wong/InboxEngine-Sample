using InboxEngine.Exceptions;
using InboxEngine.Models;
using System.Net;
using Microsoft.Net.Http;

namespace InboxEngine.Services;

/// <summary>
/// TODO: Implement the priority scoring logic according to the requirements:
/// - VIP Status: +50 points if IsVIP is true
/// - Urgency Keywords: +30 points if Subject contains "Urgent", "ASAP", or "Error" (case-insensitive)
/// - Time Decay: +1 point for every hour passed since ReceivedAt
/// - Spam Filter: -20 points if Body contains "Unsubscribe" or "Newsletter"
/// - Clamping: Final score must be between 0 and 100 (inclusive)
/// </summary>
public class PriorityScoringService : IPriorityScoringService
{
    private DateTime todaysDate;

    // Calculate priority score logic
    public int CalculatePriorityScore(Email email)
    {
        // Start with a base score of 0
        int startingScore = 0;
        int finalScore;

        const int PRIORITY_SCORE_VIP = 50;
        const int PRIORITY_SCORE_URGENT = 30;
        const int PRIORITY_SCORE_SPAM = -20;
        const int PRIORITY_SCORE_PER_HOUR_ELAPSED = 1;

        const int HOURS_IN_A_DAY = 24;

        // Difference between today and email time
        TimeSpan timeSpanFromEmailReceivedToToday = this.todaysDate.Subtract(email.ReceivedAt);
        int hours = timeSpanFromEmailReceivedToToday.Days * HOURS_IN_A_DAY + timeSpanFromEmailReceivedToToday.Hours;
        if(hours < 0) //  stop computing if difference between today and email time is negative (i.e. email time is in future)
        {
            return -1;
        }

        // For every hour elapsed, add an hour
        startingScore += hours * PRIORITY_SCORE_PER_HOUR_ELAPSED;

        // Add the appropriate amount of points for VIP
        if(email.IsVIP)
        {
            startingScore += PRIORITY_SCORE_VIP;
        }

        // Add the appropriate amount of points for urgent emails
        // (with subject containing "Urgent", "ASAP" and "Error" on a case insensitive basis
        if (email.Subject.Contains("Urgent", StringComparison.CurrentCultureIgnoreCase) ||
            email.Subject.Contains("ASAP", StringComparison.CurrentCultureIgnoreCase) ||
            email.Subject.Contains("Error", StringComparison.CurrentCultureIgnoreCase) )
        {
            startingScore += PRIORITY_SCORE_URGENT;
        }

        // Subtract the appropriate amount of points for spam emails
        // (with body containing "Unsubscribe" and "Newsletter" on a case insensitive basis
        if (email.Body.Contains("Unsubscribe", StringComparison.CurrentCultureIgnoreCase) ||
            email.Body.Contains("Newsletter", StringComparison.CurrentCultureIgnoreCase) )
        {
            startingScore += PRIORITY_SCORE_SPAM;
        }

        // Clamping of scores: Score must be at least 0 and not exceed 100
        finalScore = Math.Min(Math.Max(0, startingScore), 100);

        return finalScore;


    }

    // Set today's time--called from UI
    public void setTodaysTime(DateTime today)
    {
        this.todaysDate = today.ToUniversalTime();
    }

    // Get today's time--today's time needs to be called to compute priority scores
    public DateTime getTodaysTime()
    {
        return this.todaysDate;
    }

}
