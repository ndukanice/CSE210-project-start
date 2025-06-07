/*
Mindfulness Program

Steps taken to exceed the requirements:
- Used object-oriented design: Each activity is a separate class inheriting from a base Activity class.
- Added session summary after each activity (duration, items listed, etc.).
- Allowed user to repeat an activity without returning to the main menu.
- Added input validation for menu and duration.
- Added a simple animation spinner for "Get ready..." and between prompts.
- Used more prompts and questions for variety.
*/

using System;
using System.Collections.Generic;
using System.Threading;

abstract class Activity
{
    public string Name { get; }
    public string Description { get; }
    public Activity(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public virtual void DisplayStart()
    {
        Console.Clear();
        Console.WriteLine($"{Name} Activity");
        Console.WriteLine(Description);
        Console.WriteLine();
    }

    public void ShowSpinner(int seconds)
    {
        string[] spinner = { "|", "/", "-", "\\" };
        for (int i = 0; i < seconds * 4; i++)
        {
            Console.Write($"\r{spinner[i % 4]}");
            Thread.Sleep(250);
        }
        Console.Write("\r ");
    }

    public int GetDuration()
    {
        int duration;
        while (true)
        {
            Console.Write("How many seconds would you like for this session? ");
            if (int.TryParse(Console.ReadLine(), out duration) && duration > 0)
                break;
            Console.WriteLine("Please enter a valid positive number.");
        }
        return duration;
    }

    public abstract void Run();
}

class BreathingActivity : Activity
{
    public BreathingActivity() : base(
        "Breathing",
        "This activity will help you relax by guiding you through slow breathing. Clear your mind and focus on your breathing.")
    { }

    public override void Run()
    {
        DisplayStart();
        int duration = GetDuration();
        Console.WriteLine("Get ready...");
        ShowSpinner(3);

        int elapsed = 0;
        while (elapsed < duration)
        {
            Console.Write("\nBreathe in... ");
            Countdown(4);
            Console.Write("Now breathe out... ");
            Countdown(6);
            elapsed += 10;
        }

        Console.WriteLine("\nWell done! You have completed the Breathing Activity.");
        Console.WriteLine($"Session duration: {duration} seconds.");
        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }

    private void Countdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i + " ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

class ReflectionActivity : Activity
{
    private List<string> prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless.",
        "Recall a moment you overcame a fear.",
        "Remember a time you achieved a personal goal."
    };

    private List<string> questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity() : base(
        "Reflection",
        "This activity will help you reflect on times in your life when you have shown strength and resilience.")
    { }

    public override void Run()
    {
        DisplayStart();
        int duration = GetDuration();
        Console.WriteLine("Get ready...");
        ShowSpinner(3);

        Random rand = new Random();
        string prompt = prompts[rand.Next(prompts.Count)];
        Console.WriteLine($"\nPrompt: {prompt}");
        Console.WriteLine("When you have something in mind, press Enter to continue.");
        Console.ReadLine();

        int elapsed = 0;
        int questionCount = 0;
        while (elapsed < duration)
        {
            string question = questions[rand.Next(questions.Count)];
            Console.WriteLine($"> {question}");
            ShowSpinner(5);
            elapsed += 5;
            questionCount++;
        }

        Console.WriteLine($"\nYou reflected on {questionCount} questions.");
        Console.WriteLine("Well done! You have completed the Reflection Activity.");
        Console.WriteLine($"Session duration: {duration} seconds.");
        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }
}

class ListingActivity : Activity
{
    private List<string> prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?",
        "What are things that make you smile?"
    };

    public ListingActivity() : base(
        "Listing",
        "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    { }

    public override void Run()
    {
        DisplayStart();
        int duration = GetDuration();
        Console.WriteLine("Get ready...");
        ShowSpinner(3);

        Random rand = new Random();
        string prompt = prompts[rand.Next(prompts.Count)];
        Console.WriteLine($"\nPrompt: {prompt}");
        Console.WriteLine("Start listing items. Press Enter after each item.");

        int count = 0;
        DateTime endTime = DateTime.Now.AddSeconds(duration);
        while (DateTime.Now < endTime)
        {
            if (Console.KeyAvailable)
            {
                Console.ReadLine();
                count++;
            }
            else
            {
                Thread.Sleep(100);
            }
        }

        Console.WriteLine($"\nYou listed {count} items!");
        Console.WriteLine("Well done! You have completed the Listing Activity.");
        Console.WriteLine($"Session duration: {duration} seconds.");
        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Activity> activities = new List<Activity>
        {
            new BreathingActivity(),
            new ReflectionActivity(),
            new ListingActivity()
        };

        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("Menu Options:");
            for (int i = 0; i < activities.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. Start {activities[i].Name} Activity");
            }
            Console.WriteLine($"  {activities.Count + 1}. Quit");
            Console.Write("Select a choice from the menu: ");

            string input = Console.ReadLine();
            int choice;
            if (int.TryParse(input, out choice) && choice >= 1 && choice <= activities.Count + 1)
            {
                if (choice == activities.Count + 1)
                {
                    running = false;
                    Console.WriteLine("Goodbye!");
                }
                else
                {
                    bool repeat = true;
                    while (repeat)
                    {
                        activities[choice - 1].Run();
                        Console.Write("Would you like to repeat this activity? (y/n): ");
                        string again = Console.ReadLine().ToLower();
                        repeat = again == "y";
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Press Enter to try again.");
                Console.ReadLine();
            }
        }
    }
}