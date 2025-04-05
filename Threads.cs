using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

public class Grade
{
    public string StudentName {  get; set; }
    public string Subject {  get; set; }
    public int Score {  get; set; }

}

internal class Programm01CurrentThread
{
    private static Object locker = new Object();
    private static double value = 45;

    private static bool isTookCos = false; // флаг для того, чтобы чередовать функции
    static async Task Main(string[] args)
    {
        //задание 1.1

        new Thread(() => printRange(1, 12)).Start();
        new Thread(() => printRange(20, 25)).Start();

        //задание 1.2

        var firstThread = new Thread(() => printRange(1, 100));
        firstThread.Start();
        firstThread.Join();

        var secondThread = new Thread(() => printRange(1, 100));
        secondThread.Start();
        secondThread.Join();


        //задание 1.3

        new Thread(() => calculateCos()).Start();
        new Thread(() => calculateArcCos()).Start();


        // задание 1.5

        // Имитация данных о студентах и их оценках
        List<Grade> grades = new List<Grade>
            {
            new Grade { StudentName = "Вася", Subject = "Математика", Score = 90 },
            new Grade { StudentName = "Вася", Subject = "Физика", Score = 85 },
            new Grade { StudentName = "Петя", Subject = "Математика", Score = 75 },
            new Grade { StudentName = "Петя", Subject = "Физика", Score = 80 },
            new Grade { StudentName = "Коля", Subject = "Математика", Score = 95 },
            new Grade { StudentName = "Коля", Subject = "Физика", Score = 90 }
        };

        // Список студентов
        List<string> students = new List<string> { "Вася", "Петя", "Коля" };

        // Замер времени выполнения
        var watch = System.Diagnostics.Stopwatch.StartNew();

        // Последовательно вычисляем средний балл для каждого студента
        foreach (var student in students)
        {
            double averageScore = CalculateAverageScore(grades, student);
            Console.WriteLine($"Для студента {student} средняя оценка: {averageScore}");
        }

        watch.Stop();

        Console.WriteLine($"Все вычисления завершены. Время выполнения: {watch.ElapsedMilliseconds} мс");

        watch = System.Diagnostics.Stopwatch.StartNew();

        var tasks = students.Select(s => CalculateAverageScoreAsync(grades, s));

        var results = await Task.WhenAll(tasks);

        for (int i = 0; i < students.Count; ++i)
        {
            Console.WriteLine($"Для студента {students[i]} средняя оценка: {results[i]}");
        }

        watch.Stop();

        Console.WriteLine($"Все вычисления завершены. Время выполнения: {watch.ElapsedMilliseconds} мс");


        // задание 1.6

        watch = System.Diagnostics.Stopwatch.StartNew();

        Parallel.ForEach(students, student =>
        {
            var value = CalculateAverageScore(grades, student);
            Console.WriteLine($"Для студента {student} средняя оценка: {value}");
        });

        Console.WriteLine($"Все вычисления завершены. Время выполнения: {watch.ElapsedMilliseconds} мс");
    }

    static async Task<double> CalculateAverageScoreAsync(List<Grade> grades, string student)
    {
        await Task.Delay(100);

        var studGrades = grades.Where(g => g.StudentName == student).ToList();

        return studGrades.Sum(g => g.Score) / studGrades.Count;
    }

    static double CalculateAverageScore(List<Grade> grades, string student)
    {
        var studGrades = grades.Where(g =>  g.StudentName == student).ToList();

        return studGrades.Sum(g => g.Score) / studGrades.Count;
    }

    static void printRange(int first, int second)
    {
        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} начал работу");

        for (int i = first; i <= second; i++)
        {
            Console.WriteLine($"Поток { Thread.CurrentThread.ManagedThreadId }: { i }");
        }

        Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId} завершил работу");
    }

    static void calculateCos()
    {
        for (int i = 0; i < 10; i++)
        {
            lock (locker)
            {

                if (isTookCos)
                {
                    Monitor.Wait(locker);
                }

                var newValue = Math.Cos(value);

                Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: Значение косинуса от {value}: {newValue}");
                value = newValue;

                isTookCos = true;

                Monitor.PulseAll(locker);
            }
        }
    }
    static void calculateArcCos()
    {
        for (int i = 0; i < 10; i++)
        {
            lock (locker)
            {

                if (!isTookCos)
                {
                    Monitor.Wait(locker);
                }

                var newValue = Math.Acos(value);

                Console.WriteLine($"Поток {Thread.CurrentThread.ManagedThreadId}: Значение аркосинуса от {value}: {newValue}");
                value = newValue;

                isTookCos = false;

                Monitor.PulseAll(locker);
            }
        }
    }
}