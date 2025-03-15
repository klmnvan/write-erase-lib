namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            // Проверка на некорректные входные данные
            if (beginWorkingTime >= endWorkingTime)
                return new[] { "Начало рабочего дня должно быть раньше его окончания." };

            if (consultationTime <= 0)
                return new[] { "Минимальное время должно быть больше нуля." };

            if (startTimes == null || durations == null || startTimes.Length != durations.Length)
                return new[] { "Ошибка: массивы startTimes и durations должны быть не null и одинаковой длины." };

            // Создаем список занятых интервалов
            var busyIntervals = new List<(TimeSpan Start, TimeSpan End)>();
            for (int i = 0; i < startTimes.Length; i++)
            {
                busyIntervals.Add((startTimes[i], startTimes[i].Add(TimeSpan.FromMinutes(durations[i]))));
            }

            // Сортируем занятые интервалы по времени начала
            busyIntervals.Sort((a, b) => a.Start.CompareTo(b.Start));

            // Генерация всех возможных интервалов в рабочее время
            var allIntervals = new List<string>();
            var currentTime = beginWorkingTime;

            while (currentTime.Add(TimeSpan.FromMinutes(consultationTime)) <= endWorkingTime)
            {
                var intervalEnd = currentTime.Add(TimeSpan.FromMinutes(consultationTime));

                // Проверяем, не пересекается ли текущий интервал с занятыми
                bool isFree = true;
                foreach (var busy in busyIntervals)
                {
                    if (currentTime < busy.End && intervalEnd > busy.Start)
                    {
                        isFree = false;
                        break;
                    }
                }

                if (isFree)
                {
                    allIntervals.Add($"{currentTime:hh\\:mm}-{intervalEnd:hh\\:mm}");
                }

                currentTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime));
            }

            return allIntervals.ToArray();
        }
    }
}
