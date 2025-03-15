namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            if (beginWorkingTime >= endWorkingTime)
                return new[] { "Начало рабочего дня должно быть раньше его окончания." };

            if (consultationTime <= 0)
                return new[] { "Минимальное время должно быть больше нуля." };

            if (startTimes == null || durations == null || startTimes.Length != durations.Length)
                return new[] { "Ошибка: массивы startTimes и durations должны быть не null и одинаковой длины." };

            var busyIntervals = CreateBusyIntervals(startTimes, durations);
            busyIntervals.Sort((a, b) => a.Start.CompareTo(b.Start));

            return GenerateFreeIntervals(busyIntervals, beginWorkingTime, endWorkingTime, consultationTime);
        }

        private List<(TimeSpan Start, TimeSpan End)> CreateBusyIntervals(TimeSpan[] startTimes, int[] durations)
        {
            var intervals = new List<(TimeSpan Start, TimeSpan End)>();
            for (int i = 0; i < startTimes.Length; i++)
            {
                intervals.Add((startTimes[i], startTimes[i].Add(TimeSpan.FromMinutes(durations[i]))));
            }
            return intervals;
        }

        private string[] GenerateFreeIntervals(List<(TimeSpan Start, TimeSpan End)> busyIntervals, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            var freeIntervals = new List<string>();
            var currentTime = beginWorkingTime;

            while (currentTime.Add(TimeSpan.FromMinutes(consultationTime)) <= endWorkingTime)
            {
                var intervalEnd = currentTime.Add(TimeSpan.FromMinutes(consultationTime));

                if (!IsIntervalBusy(currentTime, intervalEnd, busyIntervals))
                {
                    freeIntervals.Add($"{currentTime:hh\\:mm}-{intervalEnd:hh\\:mm}");
                }

                currentTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime));
            }

            return freeIntervals.ToArray();
        }

        private bool IsIntervalBusy(TimeSpan start, TimeSpan end, List<(TimeSpan Start, TimeSpan End)> busyIntervals)
        {
            return busyIntervals.Any(busy => start < busy.End && end > busy.Start);
        }
    }
}
