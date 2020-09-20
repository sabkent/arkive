using System;
using System.Collections.Generic;
using System.Linq;

namespace pws.Shared.Domain
{
    public class WorkSpec
    {
        public WorkSpec()
        {
            _schedules = new List<Schedule>();
        }
        private List<Schedule> _schedules;
        
        public string Title { get; set; }
    }

    public class Schedule
    {
        private Recurrence _recurrence;

        public Schedule(DateTime start, Recurrence recurrence = null)
        {
            Start = start;
            _recurrence = recurrence;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; } //or duration? 1 hour, 2 weeks

        public IEnumerable<DateTime> Project(DateTime beginning, DateTime end) =>
            _recurrence == null
                ? new List<DateTime> {Start}
                : _recurrence.Project(beginning, end);
    }

    //occurs every other monday and every thursday
    //list of frequency items which can be combined   

    public enum Frequency
    {
        Daily,
        Weekly, //nominated days
        Fortnightly,
        Monthly, //nominated day of month.. rules  if day falls on weekend or public holiday
        //every 14 weeks
        //every 3 months
    }

    public class Recurrence
    {
        private readonly List<IRecurrencePattern> _recurrencePatterns;

        public Recurrence()
        {
            _recurrencePatterns = new List<IRecurrencePattern>();
        }

        public Recurrence AddDaily(DateTime start, int every ,TimeSpan? time = null)
        {
            _recurrencePatterns.Add(new DailyRecurrencePattern(start, time){Every = every});
            return this;
        }

        public Recurrence AddWeekly(DayOfWeek dayOfWeek, TimeSpan? time = null)
        {
            _recurrencePatterns.Add(new WeeklyRecurrencePattern(dayOfWeek, time));
            return this;
        }

        public IEnumerable<DateTime> Project(DateTime start, DateTime end)
        {
            var current = start;
            while (current.Date <= end.Date)
            {
                foreach (var date in _recurrencePatterns.SelectMany(pattern => pattern.GetPattern(current)))
                {
                    yield return date;
                }

                current = current.AddDays(1);
            }
        }
    }

    public interface IRecurrencePattern
    {
        IEnumerable<DateTime> GetPattern(DateTime datetime);
    }

    public class DailyRecurrencePattern : IRecurrencePattern
    {
        public DailyRecurrencePattern(DateTime start, TimeSpan? time = null)
        {
            _start = start;
            _time = time ?? TimeSpan.Zero;
        }

        private DateTime _start;
        private TimeSpan _time;

        private bool Covers(DateTime datetime) =>
            _start.Date == datetime.Date || datetime.Subtract(_start).Days % Every == 0;

        public int Every { get; set; }

        public IEnumerable<DateTime> GetPattern(DateTime datetime)
        {
            if (Covers(datetime))
                yield return datetime.WithTime(_time);
        }
    }

    public class WeeklyRecurrencePattern : IRecurrencePattern
    {
        public WeeklyRecurrencePattern(DayOfWeek dayOfWeek, TimeSpan? time = null)
        {
            _dayOfWeek = dayOfWeek;
            _time = time ?? TimeSpan.Zero;
        }

        private TimeSpan _time;
        private DayOfWeek _dayOfWeek;

        private bool Covers(DateTime datetime) => 
            datetime.DayOfWeek == _dayOfWeek;

        public IEnumerable<DateTime> GetPattern(DateTime datetime)
        {
            if (Covers(datetime))
                yield return datetime.WithTime(_time);
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime WithTime(this DateTime datetime, TimeSpan time) =>
            new DateTime(datetime.Year, datetime.Month, datetime.Day, time.Hours, time.Minutes, time.Seconds);
    }
}
