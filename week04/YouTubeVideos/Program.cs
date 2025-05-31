using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Create a list to store videos
        List<Video> videos = new List<Video>();

        // Create sample videos
        Video video1 = new Video("Intro to C#", "John Doe", 300);
        video1.AddComment(new Comment("Alice", "Great explanation!"));
        video1.AddComment(new Comment("Bob", "Very helpful, thanks!"));
        video1.AddComment(new Comment("Charlie", "Nice video!"));

        Video video2 = new Video("Design Patterns", "Jane Smith", 450);
        video2.AddComment(new Comment("David", "Awesome tutorial!"));
        video2.AddComment(new Comment("Eva", "I learned a lot."));
        video2.AddComment(new Comment("Frank", "Very well structured."));

        // Add videos to the list
        videos.Add(video1);
        videos.Add(video2);

        // Display video details and comments
        foreach (Video video in videos)
        {
            Console.WriteLine($"\nTitle: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.Length} seconds");
            Console.WriteLine($"Number of Comments: {video.GetCommentCount()}");
            Console.WriteLine("Comments:");
            video.DisplayComments();
        }
    }
}

// Video class
class Video
{
    public string Title { get; }
    public string Author { get; }
    public int Length { get; }
    private List<Comment> Comments;

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        Comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return Comments.Count;
    }

    public void DisplayComments()
    {
        foreach (var comment in Comments)
        {
            Console.WriteLine($" - {comment.Commenter}: {comment.Text}");
        }
    }
}

// Comment class
class Comment
{
    public string Commenter { get; }
    public string Text { get; }

    public Comment(string commenter, string text)
    {
        Commenter = commenter;
        Text = text;
    }
}