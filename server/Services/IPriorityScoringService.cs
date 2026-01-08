using InboxEngine.Models;
using InboxEngine.Exceptions;

namespace InboxEngine.Services;

public interface IPriorityScoringService
{
    int CalculatePriorityScore(Email email);

    void setTodaysTime(DateTime today);

    DateTime getTodaysTime();

}
