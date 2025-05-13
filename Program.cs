using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

interface IGameObject
{
    void Draw();
    void Update();
}

class Snake : IGameObject
{
    private List<int[]> body;
    private int direction;
    private int width;
    private int height;

    public Snake(int startX, int startY, int width, int height)
    {
        body = new List<int[]> { new int[] { startX, startY } };
        direction = 1;
        this.width = width;
        this.height = height;
    }

    public void Draw()
    {
        foreach (var segment in body)
        {
            Console.SetCursorPosition(segment[0], segment[1]);
            Console.Write("O");
        }
    }

    public void Update() // положение змеи
    {
        int headX = body.First()[0];
        int headY = body.First()[1];

        switch (direction)
        {
            case 0: headY--; break;
            case 1: headX++; break;
            case 2: headY++; break;
            case 3: headX--; break;
        }

        body.Insert(0, new int[] { headX, headY });
    }

    public void ChangeDirection(int newDirection)
    {
        if (Math.Abs(direction - newDirection) != 2)
            direction = newDirection;
    }

    public void Grow() //Проверка на сигмента (тела) червяка 
    { }

    public bool IsCollision()
    {
        int headX = body.First()[0];
        int headY = body.First()[1];

        // Проверка на столкновение со стенами
        if (headX < 0 || headX >= width + 1 || headY < 0 || headY >= height + 1)
            return true;



        // Проверка на столкновение с собой
        return body.Skip(1).Any(segment => segment[0] == headX && segment[1] == headY);
    }


    public void RemoveTail()
    {
        body.RemoveAt(body.Count - 1);
    }

    public int[] GetHeadPosition()
    {
        return body.First();
    }
}

class Food : IGameObject
{
    private int x;
    private int y;
    private int width;
    private int height;

    public Food(int width, int height)
    {
        this.width = width;
        this.height = height;
        Generate();
    }

    public void Draw()
    {
        Console.SetCursorPosition(x, y);
        Console.Write("F");
    }

    public void Update()
    { }

    public void Generate()
    {
        Random random = new Random();
        x = random.Next(0, width);
        y = random.Next(0, height);
    }

    public int[] GetPosition()
    {
        return new int[] { x, y };
    }
}

class Program
{
    static int width = 19;
    static int height = 9;
    static bool gameOver = false;
    static Snake snake;
    static Food food;
    static int score = 0;

    static void Main()
    {
        Console.CursorVisible = false;
        snake = new Snake(width / 2, height / 2, width, height);
        food = new Food(width, height);

        while (!gameOver)
        {
            Draw();
            Input();
            Logic();
            Thread.Sleep(100);
        }

        Console.SetCursorPosition(0, height + 1);
        Console.WriteLine("Game Over! Your score: " + score);
    }
    static void Draw()
    {
        Console.Clear();
    
        for (int i = 0; i < width + 1; i++)
        {
            Console.SetCursorPosition(i, 0); // Верхняя граница
            Console.Write("░");
        }
        for (int i = 0; i < height + 1; i++)
        {
            Console.SetCursorPosition(0, i); // Левая граница
            Console.Write("░");
        }


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.SetCursorPosition(x + 1, y + 1);
                Console.Write("░");
            }
        }

    snake.Draw();
    food.Draw();
}


    static void Input()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    snake.ChangeDirection(0);
                    break;
                case ConsoleKey.RightArrow:
                    snake.ChangeDirection(1);
                    break;
                case ConsoleKey.DownArrow:
                    snake.ChangeDirection(2);
                    break;
                case ConsoleKey.LeftArrow:
                    snake.ChangeDirection(3);
                    break;
            }
        }
    }

    static void Logic()
    {

        snake.Update();


        if (snake.IsCollision())
        {
            gameOver = true;
        }

        int[] headPosition = snake.GetHeadPosition();
        int headX = headPosition[0];
        int headY = headPosition[1];

    
        if (headX == food.GetPosition()[0] && headY == food.GetPosition()[1])
        {
            score++;
            snake.Grow();
            food.Generate();
        }
        else
        {
            snake.RemoveTail();
        }
    }

} 
