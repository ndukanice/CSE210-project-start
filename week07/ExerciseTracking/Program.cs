using System;
using System.Collections.Generic;

// Base class
abstract class Activity
{
    protected DateTime _date;
    protected int _duration; // in minutes
    protected const bool UseMiles = true; // switch to false for kilometers

    public Activity(DateTime date, int duration)
    {
        _date = date;
        _duration = duration;
    }

    public abstract double GetDistance();
    public virtual double GetSpeed() => Math.Round((GetDistance() / _duration) * 60, 1);
    public virtual double GetPace() => Math.Round(_duration / GetDistance(), 2);

    public virtual string GetSummary()
    {
        string unit = UseMiles ? "miles" : "km";
        return $"{_date:dd MMM yyyy} {this.GetType().Name} ({_duration} min): " +
               $"Distance {GetDistance()} {unit}, " +
               $"Speed {GetSpeed()} {(UseMiles ? "mph" : "kph")}, " +
               $"Pace: {GetPace()} min per {unit}";
    }
}

// Derived class: Running
class Running : Activity
{
    private double _distance; // already stored

    public Running(DateTime date, int duration, double distance)
        : base(date, duration)
    {
        _distance = distance;
    }

    public override double GetDistance() => Math.Round(_distance, 2);
}

// Derived class: Cycling
class Cycling : Activity
{
    private double _speed;

    public Cycling(DateTime date, int duration, double speed)
        : base(date, duration)
    {
        _speed = speed;
    }

    public override double GetDistance() => Math.Round((_speed * _duration) / 60, 2);
    public override double GetSpeed() => _speed;
    public override double GetPace() => Math.Round(60 / _speed, 2);
}

// Derived class: Swimming
class Swimming : Activity
{
    private int _laps;
    private string _strokeType; // extra field just for fun

    public Swimming(DateTime date, int duration, int laps, string stroke = "Freestyle")
        : base(date, duration)
    {
        _laps = laps;
        _strokeType = stroke;
    }

    public override double GetDistance()
    {
        double distanceKm = _laps * 50 / 1000.0;
        return UseMiles ? Math.Round(distanceKm * 0.62, 2) : Math.Round(distanceKm, 2);
    }

    public override string GetSummary()
    {
        return base.GetSummary() + $" | Stroke: {_strokeType}";
    }
}

// Program execution
class Program
{
    static void Main()
    {
        var activities = new List<Activity>
        {
            new Running(new DateTime(2025, 6, 18), 30, 4.8),
            new Cycling(new DateTime(2025, 6, 17), 45, 15.0),
            new Swimming(new DateTime(2025, 6, 16), 25, 30, "Butterfly")
        };

        Console.WriteLine("=== Exercise Activity Summary ===\n");
        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
