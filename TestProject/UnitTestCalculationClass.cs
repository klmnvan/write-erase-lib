using SF2022User_NN_Lib;

namespace TestProject
{
    [TestClass]
    public class UnitTestCalculationClass
    {
        private readonly Calculations _calculations;

        public UnitTestCalculationClass()
        {
            _calculations = new Calculations();
        }

        // ���� 1: ��� ������� ����������, ���� ���� ��������
        [TestMethod]
        public void AvailablePeriods_NoBusyIntervals_ReturnsFullDay()
        {
            var startTimes = new TimeSpan[] { };
            var durations = new int[] { };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }

        // ���� 2: ���� ������� �������� � �������� ���
        [TestMethod]
        public void AvailablePeriods_OneBusyInterval_ReturnsCorrectFreeIntervals()
        {
            var startTimes = new[] { new TimeSpan(10, 0, 0) };
            var durations = new[] { 60 };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }

        // ���� 3: ������� �������� � ������ ���
        [TestMethod]
        public void AvailablePeriods_BusyIntervalAtStart_ReturnsCorrectFreeIntervals()
        {
            var startTimes = new[] { new TimeSpan(9, 0, 0) };
            var durations = new[] { 60 };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }

        // ���� 4: ������� �������� � ����� ���
        [TestMethod]
        public void AvailablePeriods_BusyIntervalAtEnd_ReturnsCorrectFreeIntervals()
        {
            var startTimes = new[] { new TimeSpan(16, 0, 0) };
            var durations = new[] { 60 };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00" }, result);
        }

        // ���� 5: ��������� ������� ���������� � �������������
        [TestMethod]
        public void AvailablePeriods_OverlappingBusyIntervals_ReturnsCorrectFreeIntervals()
        {
            var startTimes = new[] { new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0) };
            var durations = new[] { 60, 30 };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(
                new[] {
                    "09:00-09:30",
                    "09:30-10:00",
                    "11:00-11:30",
                    "11:30-12:00",
                    "12:00-12:30",
                    "12:30-13:00",
                    "13:00-13:30",
                    "13:30-14:00",
                    "14:00-14:30",
                    "14:30-15:00",
                    "15:00-15:30",
                    "15:30-16:00",
                    "16:00-16:30",
                    "16:30-17:00"
                }, result);
        }

        // ���� 6: ����������� ����� ������, ��� ��������� ��������� �����
        [TestMethod]
        public void AvailablePeriods_ConsultationTimeTooLong_ReturnsCorrectIntervals()
        {
            var startTimes = new[] { new TimeSpan(10, 0, 0) };
            var durations = new[] { 60 };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 120;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "11:00-13:00", "13:00-15:00", "15:00-17:00" }, result);
        }

        // ���� 7: ������������ ������� ������ (������ �������� ��� ����� �����)
        [TestMethod]
        public void AvailablePeriods_BeginWorkingTimeAfterEnd_ReturnsError()
        {
            var startTimes = new TimeSpan[] { };
            var durations = new int[] { };
            var beginWorkingTime = new TimeSpan(17, 0, 0);
            var endWorkingTime = new TimeSpan(9, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "������ �������� ��� ������ ���� ������ ��� ���������." }, result);
        }

        // ���� 8: ������������ ������� ������ (����������� ����� <= 0)
        [TestMethod]
        public void AvailablePeriods_InvalidConsultationTime_ReturnsError()
        {
            var startTimes = new TimeSpan[] { };
            var durations = new int[] { };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 0;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "����������� ����� ������ ���� ������ ����." }, result);
        }

        // ���� 9: ������������ ������� ������ (������� ������ �����)
        [TestMethod]
        public void AvailablePeriods_ArraysLengthMismatch_ReturnsError()
        {
            var startTimes = new[] { new TimeSpan(10, 0, 0) };
            var durations = new int[] { };
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "������: ������� startTimes � durations ������ ���� �� null � ���������� �����." }, result);
        }

        // ���� 10: ������� ��������� ��������� ���� ����
        [TestMethod]
        public void AvailablePeriods_FullDayBusy_ReturnsEmptyArray()
        {
            var startTimes = new[] { new TimeSpan(9, 0, 0) };
            var durations = new[] { 480 }; // 8 �����
            var beginWorkingTime = new TimeSpan(9, 0, 0);
            var endWorkingTime = new TimeSpan(17, 0, 0);
            var consultationTime = 30;

            var result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new string[] { }, result);
        }
    }
}