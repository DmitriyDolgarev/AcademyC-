using System.Collections.Generic;
using System.Threading;

internal class Programm01CurrentThread
{
    private static Object locker = new Object();
    private static double value = 45;

    private static bool isTookCos = false; // флаг для того, чтобы чередовать функции
    private static void Main(string[] args)
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