/*
Eternal Quest Program — Exceeding Requirements!

Enhancements:git
1. Added "ProgressGoal" for long-term goals with partial progress.
2. Added "NegativeGoal" to track and penalize bad habits.
3. Gamification: Users level up based on score, with fun titles.
4. Menu system to guide user interaction cleanly.
5. All classes organized in one file using inheritance & polymorphism.

Author: Emmanuel nduka Eze 
Date: June 2025
*/

using System;
using System.Collections.Generic;
using System.IO;

// BASE CLASS
abstract class Goal {
    public string Name { get; set; }
    public string Description { get; set; }
    protected int Points;

    public Goal(string name, string description, int points) {
        Name = name;
        Description = description;
        Points = points;
    }

    public abstract int RecordEvent();
    public abstract bool IsComplete();
    public abstract string GetStatus();
    public abstract string Serialize();
}

// SIMPLE GOAL
class SimpleGoal : Goal {
    private bool _completed = false;

    public SimpleGoal(string name, string description, int points)
        : base(name, description, points) {}

    public override int RecordEvent() {
        if (!_completed) {
            _completed = true;
            return Points;
        }
        return 0;
    }

    public override bool IsComplete() => _completed;
    public override string GetStatus() => _completed ? "[X]" : "[ ]";
    public override string Serialize() => $"Simple|{Name}|{Description}|{Points}|{_completed}";
}

// ETERNAL GOAL
class EternalGoal : Goal {
    public EternalGoal(string name, string description, int points)
        : base(name, description, points) {}

    public override int RecordEvent() => Points;
    public override bool IsComplete() => false;
    public override string GetStatus() => "[~]";
    public override string Serialize() => $"Eternal|{Name}|{Description}|{Points}";
}

// CHECKLIST GOAL
class ChecklistGoal : Goal {
    private int _targetCount;
    private int _currentCount;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonus)
        : base(name, description, points) {
        _targetCount = targetCount;
        _currentCount = 0;
        _bonus = bonus;
    }

    public override int RecordEvent() {
        _currentCount++;
        if (_currentCount == _targetCount)
            return Points + _bonus;
        return Points;
    }

    public override bool IsComplete() => _currentCount >= _targetCount;
    public override string GetStatus() => IsComplete() ? "[X]" : $"[{_currentCount}/{_targetCount}]";
    public override string Serialize() => $"Checklist|{Name}|{Description}|{Points}|{_targetCount}|{_currentCount}|{_bonus}";
}

// PROGRESS GOAL (BONUS)
class ProgressGoal : Goal {
    private int _target;
    private int _progress;

    public ProgressGoal(string name, string description, int points, int target)
        : base(name, description, points) {
        _target = target;
        _progress = 0;
    }

    public override int RecordEvent() {
        if (_progress < _target) {
            _progress++;
            return Points;
        }
        return 0;
    }

    public override bool IsComplete() => _progress >= _target;
    public override string GetStatus() => $"{_progress}/{_target}";
    public override string Serialize() => $"Progress|{Name}|{Description}|{Points}|{_progress}|{_target}";
}

// NEGATIVE GOAL (BONUS)
class NegativeGoal : Goal {
    public NegativeGoal(string name, string description, int penalty)
        : base(name, description, -penalty) {}

    public override int RecordEvent() => Points; // Negative points
    public override bool IsComplete() => false;
    public override string GetStatus() => "[!]";
    public override string Serialize() => $"Negative|{Name}|{Description}|{Points}";
}

// PLAYER (tracks level)
class Player {
    public int Score { get; private set; } = 0;

    public void AddPoints(int points) {
        Score += points;
    }

    public string GetTitle() {
        return Score switch {
            < 100 => "Newbie",
            < 500 => "Adventurer",
            < 1000 => "Level 1 Hero",
            < 2000 => "Level 2 Apprentice",
            _ => "Ultimate Goal Master"
        };
    }
}

// MAIN PROGRAM
class Program {
    static List<Goal> goals = new();
    static Player player = new();

    static void Main() {
        bool running = true;
        while (running) {
            Console.Clear();
            Console.WriteLine($"Score: {player.Score} ({player.GetTitle()})");
            Console.WriteLine("\nEternal Quest Menu:");
            Console.WriteLine("1. Create Goal");
            Console.WriteLine("2. Record Event");
            Console.WriteLine("3. Show Goals");
            Console.WriteLine("4. Save / Load");
            Console.WriteLine("5. Exit");
            Console.Write("> ");

            switch (Console.ReadLine()) {
                case "1": CreateGoal(); break;
                case "2": RecordEvent(); break;
                case "3": ShowGoals(); break;
                case "4": SaveLoad(); break;
                case "5": running = false; break;
            }
        }
    }

    static void CreateGoal() {
        Console.WriteLine("Choose goal type: 1) Simple 2) Eternal 3) Checklist 4) Progress 5) Negative");
        string type = Console.ReadLine();
        Console.Write("Name: "); string name = Console.ReadLine();
        Console.Write("Description: "); string desc = Console.ReadLine();
        Console.Write("Points: "); int points = int.Parse(Console.ReadLine());

        switch (type) {
            case "1": goals.Add(new SimpleGoal(name, desc, points)); break;
            case "2": goals.Add(new EternalGoal(name, desc, points)); break;
            case "3":
                Console.Write("Target count: "); int count = int.Parse(Console.ReadLine());
                Console.Write("Bonus: "); int bonus = int.Parse(Console.ReadLine());
                goals.Add(new ChecklistGoal(name, desc, points, count, bonus)); break;
            case "4":
                Console.Write("Target progress: "); int target = int.Parse(Console.ReadLine());
                goals.Add(new ProgressGoal(name, desc, points, target)); break;
            case "5": goals.Add(new NegativeGoal(name, desc, points)); break;
        }
    }

    static void RecordEvent() {
        ShowGoals();
        Console.Write("Select goal #: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index <= goals.Count && index > 0) {
            int points = goals[index - 1].RecordEvent();
            player.AddPoints(points);
            Console.WriteLine(points < 0 ? $"Lost {-points} points!" : $"Gained {points} points!");
        }
        Console.ReadKey();
    }

    static void ShowGoals() {
        Console.WriteLine("\nYour Goals:");
        for (int i = 0; i < goals.Count; i++) {
            Console.WriteLine($"{i + 1}. {goals[i].GetStatus()} {goals[i].Name} — {goals[i].Description}");
        }
    }

    static void SaveLoad() {
        Console.Write("Save or Load? (s/l): ");
        if (Console.ReadLine().ToLower() == "s") {
            File.WriteAllLines("goals.txt", goals.ConvertAll(g => g.Serialize()));
            File.WriteAllText("score.txt", player.Score.ToString());
            Console.WriteLine("Game saved!");
        } else {
            goals.Clear();
            foreach (var line in File.ReadAllLines("goals.txt")) {
                var parts = line.Split('|');
                switch (parts[0]) {
                    case "Simple": goals.Add(new SimpleGoal(parts[1], parts[2], int.Parse(parts[3]))); break;
                    case "Eternal": goals.Add(new EternalGoal(parts[1], parts[2], int.Parse(parts[3]))); break;
                    case "Checklist": goals.Add(new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[6]))); break;
                    case "Progress": goals.Add(new ProgressGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[5]))); break;
                    case "Negative": goals.Add(new NegativeGoal(parts[1], parts[2], -int.Parse(parts[3]))); break;
                }
            }
            player = new Player();
            player.AddPoints(int.Parse(File.ReadAllText("score.txt")));
            Console.WriteLine("Game loaded!");
        }
        Console.ReadKey();
    }
}
