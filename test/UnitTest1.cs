using System;
using System.Linq;
using pws.Shared.Domain;
using Xunit;

namespace test
{
    public class UnitTest1
    {
        [Fact]
        public void RecurEveryOtherDay()
        {
            var start = new DateTime(2020, 9, 1);
            var occursEveryTwoDays = new Recurrence().AddDaily(start, 2);
            var schedule = new Schedule(start, occursEveryTwoDays);

            var dates = schedule.Project(start, start.AddDays(8)).ToList();

            Assert.Equal(new []
            {
                new DateTime(2020, 9, 1),
                new DateTime(2020, 9, 3),
                new DateTime(2020, 9, 5),
                new DateTime(2020, 9, 7),
                new DateTime(2020, 9, 9),
            }, dates);
        }

        [Fact]
        public void RecursEveryMonday()
        {
            var start = new DateTime(2020, 9, 1);
            var occursEveryTwoDays = new Recurrence().AddWeekly(DayOfWeek.Monday);
            var schedule = new Schedule(start, occursEveryTwoDays);

            var dates = schedule.Project(start, start.AddDays(31)).ToList();

            Assert.Equal(new[]
            {
                new DateTime(2020, 9, 7),
                new DateTime(2020, 9, 14),
                new DateTime(2020, 9, 21),
                new DateTime(2020, 9, 28),
            }, dates);
        }

        [Fact]
        public void RecursMondayAndThurs()
        {
            var start = new DateTime(2020, 9, 1);
            var occursEveryTwoDays = new Recurrence()
                .AddWeekly(DayOfWeek.Monday)
                .AddWeekly(DayOfWeek.Thursday);
            var schedule = new Schedule(start, occursEveryTwoDays);

            var dates = schedule.Project(start, start.AddDays(8)).ToList();
            
            Assert.Equal(new[]
            {
                new DateTime(2020, 9, 3),
                new DateTime(2020, 9, 7)
            }, dates);
        }

        [Fact]
        public void RecursTuesdaysMorningAndThursdayEvening()
        {
            var start = new DateTime(2020, 9, 1);
            var occursEveryTwoDays = new Recurrence()
                .AddWeekly(DayOfWeek.Tuesday, new TimeSpan(9 ,0, 0))
                .AddWeekly(DayOfWeek.Thursday, new TimeSpan(21, 0, 0));
            var schedule = new Schedule(start, occursEveryTwoDays);

            var dates = schedule.Project(start, start.AddDays(3)).ToList();

            Assert.Equal(new[]
            {
                new DateTime(2020, 9, 1, 9, 0, 0),
                new DateTime(2020, 9, 3, 21, 0, 0)
            }, dates);
        }

        [Fact]
        public void RecursTwiceInOneDay()
        {
            var start = new DateTime(2020, 9, 1);
            var occursEveryTwoDays = new Recurrence()
                .AddDaily(start, 2, new TimeSpan(10, 0, 0))
                .AddDaily(start, 1, new TimeSpan(18, 30, 0));
            var schedule = new Schedule(start, occursEveryTwoDays);

            var dates = schedule.Project(start, start.AddDays(2)).ToList();

            Assert.Equal(new[]
            {
                new DateTime(2020, 9, 1, 10, 0, 0),
                new DateTime(2020, 9, 1, 18, 30, 0),
                new DateTime(2020, 9, 2, 18, 30, 0),
                new DateTime(2020, 9, 3, 10, 0, 0),
                new DateTime(2020, 9, 3, 18, 30, 0)
            }, dates);
        }
    }
}
