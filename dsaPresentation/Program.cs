using System;
using System.Diagnostics;

public class Timing
{
    TimeSpan startingTime;
    TimeSpan duration;
    public Timing()
    {
        startingTime = new TimeSpan(0);
        duration = new TimeSpan(0);
    }
    public void StopTime()
    {
        duration = Process.GetCurrentProcess().Threads[0].
        UserProcessorTime.
        Subtract(startingTime);
    }
    public void startTime()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        startingTime = Process.GetCurrentProcess().Threads[0].
        UserProcessorTime;
    }
    public TimeSpan Result()
    {
        return duration;
    }
}

class BubbleSortElement
{
    public BubbleSortElement(int index, int value)
    {
        this.index = index;
        this.value = value;
    }
    public int index;
    public int value;
}


class Program
{
    static void BubbleSort(int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }

    static void BubbleSortTestStability(BubbleSortElement[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = 0; j < arr.Length - i - 1; j++)
            {
                if (arr[j].value > arr[j + 1].value)
                {
                    BubbleSortElement temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        int times = 100;
        Random rand = new Random();
        double totalTime = 0;

        for (int timeIndex = 1; timeIndex <= times; timeIndex++)
        {
            Console.WriteLine("\nTime: " + timeIndex + "\n");

            Console.WriteLine("--------------------------------------");
            int[] numbers = new int[5];

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = rand.Next(1, 10);
            }



            Console.Write("initial array: ");
            foreach (int num in numbers)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();

            Timing tObj = new Timing();
            tObj.startTime();

            BubbleSort(numbers);

            tObj.StopTime();

            Console.Write("result: ");
            foreach (int num in numbers)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();

            Console.WriteLine("time count: " + tObj.Result().TotalMilliseconds);

            totalTime += tObj.Result().TotalMilliseconds;
            Console.WriteLine("totalTime: " + totalTime);
            Console.WriteLine("--------------------------------------");
        }

        double average = totalTime / times;

        Console.WriteLine("\naverage: " + average + "\n");

        Console.WriteLine("========================================");


        bool isStable = true;

        int stabilityTestTimes = 2000;
        BubbleSortElement[] stabilityPreviousValue = [
                new BubbleSortElement(0, 3),
                new BubbleSortElement(1, 1),
                new BubbleSortElement(2, 5),
                new BubbleSortElement(3, 5),
                new BubbleSortElement(4, 2),
            ];

        BubbleSortTestStability(stabilityPreviousValue);

        for (int stabilityTestTimeIndex = 1; stabilityTestTimeIndex <= stabilityTestTimes; stabilityTestTimeIndex++)
        {
            BubbleSortElement[] stabilityTest = [
                new BubbleSortElement(0, 3),
                new BubbleSortElement(1, 1),
                new BubbleSortElement(2, 5),
                new BubbleSortElement(3, 5),
                new BubbleSortElement(4, 2),
            ];

            int[] expectation = [1, 4, 0, 2, 3];

            BubbleSortTestStability(stabilityTest);

            for (int index = 0; index < stabilityTest.Length - 1; index++)
            {
                if (expectation[index] != stabilityTest[index].index)
                {
                    Console.WriteLine("expectation[index]: " + expectation[index]);
                    Console.WriteLine("stabilityTest[index].value: " + stabilityTest[index].value);
                    isStable = false;
                    break;
                }
            }

            // for (int index = 0; index < stabilityTest.Length; index++)
            // {
            //     Console.WriteLine("value: " + stabilityTest[index].value + "; index: " + stabilityTest[index].index);
            // }

            for (int index = 0; index < stabilityTest.Length - 1; index++)
            {
                if (stabilityPreviousValue[index].index != stabilityTest[index].index)
                {
                    Console.WriteLine("stabilityPreviousValue[index].index: " + stabilityPreviousValue[index].index);
                    Console.WriteLine("stabilityTest[index].value: " + stabilityTest[index].value);
                    isStable = false;
                    break;
                }
            }
        }

        Console.WriteLine();

        Console.WriteLine("is bubble sort stable: " + isStable);
    }
}