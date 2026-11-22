namespace SkipMaster.Domain;

// итог прогона - по нему потом сравниваем стратегии между собой
public sealed class SemesterResult
{
    public SemesterResult(int totalPleasure, int daysCompleted, bool exploded)
    {
        TotalPleasure = totalPleasure;
        DaysCompleted = daysCompleted;
        Exploded = exploded;
    }

    public int TotalPleasure { get; }

    public int DaysCompleted { get; }

    public bool Exploded { get; }

    public double AveragePerDay
    {
        get
        {
            if (DaysCompleted == 0)
            {
                return 0;
            }

            return TotalPleasure / (double)DaysCompleted;
        }
    }
}
