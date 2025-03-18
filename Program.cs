using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.IO;

class Sequence
{
    public List<int> numbers;

    public Sequence()
    {
        numbers = new List<int>();
    }
    public Sequence(List<int> nums)
    {
        numbers = new List<int>(nums);
    }
    public string GetTypeSequence()
    {
        if (numbers.Count == 1)
        {
            return "Послідовність складається з одного елемента, тип визначити неможливо.";
        }

        bool isAscending = true;
        bool isDescending = true;
        for (int i = 0; i < numbers.Count - 1; i++)
        {
            if (numbers[i] >= numbers[i + 1])
            {
                isAscending = false;
            }
            if (numbers[i] <= numbers[i + 1])
            {
                isDescending = false;
            }
        }

        string result;
        if (isAscending)
        {
            result = "Зростаюча";
        }
        else if (isDescending)
        {
            result = "Спадна";
        }
        else
        {
            result = "Ні зростаюча, ні спадна";
            return result;
        }

        bool isArithmetic = true;
        bool isGeometric = true;

        if (numbers.Count < 2)
        {
            isArithmetic = false;
            isGeometric = false;
        }
        else
        {
            int diff = numbers[1] - numbers[0];
            int ratio;
            if (numbers[0] != 0)
            {
                ratio = numbers[1] / numbers[0];
            }
            else
            {
                ratio = 0;
            }

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i + 1] - numbers[i] != diff)
                {
                    isArithmetic = false;
                }

                if (numbers[i] == 0 || numbers[i + 1] / numbers[i] != ratio)
                {
                    isGeometric = false;
                }
            }
        }

        if (isArithmetic)
        {
            if (isAscending)
            {
                result += ", арифметична зростаюча прогресія";
            }
            else
            {
                result += ", арифметична спадна прогресія";
            }
        }
        else if (isGeometric)
        {
            if (isAscending)
            {
                result += ", геометрична зростаюча прогресія";
            }
            else
            {
                result += ", геометрична спадна прогресія";
            }
        }
        return result;
    }

    public bool Contains(int element)
    {
        return numbers.Contains(element);
    }

    public bool Equals(Sequence other)
    {
        return numbers.SequenceEqual(other.numbers);
    }

    public int Max() => numbers.Max();
    public int Min() => numbers.Min();

    public List<int> LocalMaxima()
    {
        var maxima = new List<int>();
        for (int i = 1; i < numbers.Count - 1; i++)
        {
            if (numbers[i] >= numbers[i - 1] && numbers[i] >= numbers[i + 1])
            {
                if (!maxima.Contains(numbers[i]))
                {
                    maxima.Add(numbers[i]);
                }
            }
        }
        return maxima;
    }
    public List<int> LocalMinima()
    {
        var minima = new List<int>();
        for (int i = 1; i < numbers.Count - 1; i++)
        {
            if (numbers[i] <= numbers[i - 1] && numbers[i] <= numbers[i + 1])
            {
                if (!minima.Contains(numbers[i]))
                {
                    minima.Add(numbers[i]);
                }
            }
        }
        return minima;
    }
    public List<int> LongestIncreasingSubsequence()
    {
        List<int> result = new List<int>();
        List<int> temp = new List<int>();

        for (int i = 0; i < numbers.Count; i++)
        {
            if (temp.Count == 0 || numbers[i] > temp.Last())
            {
                temp.Add(numbers[i]);
            }
            else
            {
                if (temp.Count > result.Count)
                {
                    result = new List<int>(temp);
                }
                temp.Clear();
                temp.Add(numbers[i]);
            }
        }

        if (temp.Count > result.Count)
        {
            result = new List<int>(temp);
        }

        return result;
    }
    public List<int> LongestDecreasingSubsequence()
    {
        List<int> result = new List<int>();
        List<int> temp = new List<int>();

        for (int i = 0; i < numbers.Count; i++)
        {
            if (temp.Count == 0 || numbers[i] < temp.Last())
            {
                temp.Add(numbers[i]);
            }
            else
            {
                if (temp.Count > result.Count)
                {
                    result = new List<int>(temp);
                }
                temp.Clear();
                temp.Add(numbers[i]);
            }
        }

        if (temp.Count > result.Count)
        {
            result = new List<int>(temp);
        }

        return result;
    }

    public void Print()
    {
        Console.WriteLine(string.Join(", ", numbers));
    }

    public void SaveToJson(string filePath)
    {
        try
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Послідовність збережена у файл: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при збереженні файлу: {ex.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        do
        {
            int numberOfSequences = 0;
            while (true)
            {
                Console.Write("\nВи хочете ввести одну послідовність чи дві? (1/2): ");
                string choice = Console.ReadLine().Trim();
                if (choice == "1" || choice == "2")
                {
                    numberOfSequences = int.Parse(choice);
                    break;
                }
                else
                {
                    Console.WriteLine("Некоректний ввід. Спробуйте ще раз.");
                }
            }

            if (numberOfSequences == 1)
            {
                List<int> numbers;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nВведіть послідовність цілих чисел через пробіл:");
                        string input = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(input))
                        {
                            Console.WriteLine("Послідовність порожня. Спробуйте ще раз.");
                            continue;
                        }
                        numbers = input.Split().Select(num => int.Parse(num)).ToList();
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Некоректний ввід або послідовність порожня. Спробуйте ще раз.");
                    }
                }

                Sequence seq = new Sequence(numbers);

                Console.WriteLine("Тип послідовності: " + seq.GetTypeSequence());
                Console.WriteLine("Максимум: " + seq.Max());
                Console.WriteLine("Мінімум: " + seq.Min());

                int element;
                while (true)
                {
                    try
                    {
                        Console.Write("Введіть число для перевірки наявності: ");
                        element = int.Parse(Console.ReadLine());
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Некоректний ввід. Введіть число.");
                    }
                }
                Console.WriteLine("Число " + (seq.Contains(element) ? "є" : "немає") + " в послідовності");

                var maxima = seq.LocalMaxima();
                var minima = seq.LocalMinima();

                if (numbers.Count >= 3)
                {
                    Console.WriteLine("Локальні максимуми: " + (maxima.Any() ? string.Join(", ", maxima) : "відсутні"));
                    Console.WriteLine("Локальні мінімуми: " + (minima.Any() ? string.Join(", ", minima) : "відсутні"));
                }
                else
                {
                    Console.WriteLine("Послідовність занадто коротка для знаходження локальних максимумів/мінімумів.");
                }

                Console.WriteLine("Найбільша зростаюча підпослідовність: " + string.Join(", ", seq.LongestIncreasingSubsequence()));
                Console.WriteLine("Найбільша спадна підпослідовність: " + string.Join(", ", seq.LongestDecreasingSubsequence()));

                string saveFilePath = @"C:\Users\Polina\Desktop\Sequence.json";
                seq.SaveToJson(saveFilePath);
            }
            else if (numberOfSequences == 2)
            {
                List<int> numbers1;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nВведіть першу послідовність цілих чисел через пробіл:");
                        string input1 = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(input1))
                        {
                            Console.WriteLine("Послідовність порожня. Спробуйте ще раз.");
                            continue;
                        }
                        numbers1 = input1.Split().Select(num => int.Parse(num)).ToList();
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Некоректний ввід. Спробуйте ще раз.");
                    }
                }
                Sequence seq1 = new Sequence(numbers1);
                Console.WriteLine("\nПерша послідовність:");
                Console.WriteLine("Тип послідовності: " + seq1.GetTypeSequence());
                Console.WriteLine("Максимум: " + seq1.Max());
                Console.WriteLine("Мінімум: " + seq1.Min());

                var maxima1 = seq1.LocalMaxima();
                var minima1 = seq1.LocalMinima();

                if (numbers1.Count >= 3)
                {
                    Console.WriteLine("Локальні максимуми: " + (maxima1.Any() ? string.Join(", ", maxima1) : "відсутні"));
                    Console.WriteLine("Локальні мінімуми: " + (minima1.Any() ? string.Join(", ", minima1) : "відсутні"));
                }
                else
                {
                    Console.WriteLine("Послідовність занадто коротка для знаходження локальних максимумів/мінімумів.");
                }

                Console.WriteLine("Найбільша зростаюча підпослідовність: " + string.Join(", ", seq1.LongestIncreasingSubsequence()));
                Console.WriteLine("Найбільша спадна підпослідовність: " + string.Join(", ", seq1.LongestDecreasingSubsequence()));

                List<int> numbers2;
                while (true)
                {
                    try
                    {
                        Console.WriteLine("\nВведіть другу послідовність цілих чисел через пробіл:");
                        string input2 = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(input2))
                        {
                            Console.WriteLine("Послідовність порожня. Спробуйте ще раз.");
                            continue;
                        }
                        numbers2 = input2.Split().Select(num => int.Parse(num)).ToList();
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Некоректний ввід. Спробуйте ще раз.");
                    }
                }
                Sequence seq2 = new Sequence(numbers2);
                Console.WriteLine("\nДруга послідовність:");
                Console.WriteLine("Тип послідовності: " + seq2.GetTypeSequence());
                Console.WriteLine("Максимум: " + seq2.Max());
                Console.WriteLine("Мінімум: " + seq2.Min());

                var maxima2 = seq2.LocalMaxima();
                var minima2 = seq2.LocalMinima();

                if (numbers2.Count >= 3)
                {
                    Console.WriteLine("Локальні максимуми: " + (maxima2.Any() ? string.Join(", ", maxima2) : "відсутні"));
                    Console.WriteLine("Локальні мінімуми: " + (minima2.Any() ? string.Join(", ", minima2) : "відсутні"));
                }
                else
                {
                    Console.WriteLine("Послідовність занадто коротка для знаходження локальних максимумів/мінімумів.");
                }

                Console.WriteLine("Найбільша зростаюча підпослідовність: " + string.Join(", ", seq2.LongestIncreasingSubsequence()));
                Console.WriteLine("Найбільша спадна підпослідовність: " + string.Join(", ", seq2.LongestDecreasingSubsequence()));

                if (seq1.Equals(seq2))
                {
                    Console.WriteLine("Послідовності однакові");
                }
                else
                {
                    Console.WriteLine("Послідовності не однакові");
                }

                string saveFilePath1 = @"C:\Users\Polina\Desktop\Sequence1.json";
                string saveFilePath2 = @"C:\Users\Polina\Desktop\Sequence2.json";

                seq1.SaveToJson(saveFilePath1);
                seq2.SaveToJson(saveFilePath2);
            }

            string userInput;
            while (true)
            {
                Console.Write("Бажаєте повторити? (yes/no): ");
                userInput = Console.ReadLine().Trim().ToLower();
                if (userInput == "yes" || userInput == "no")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nПомилка: введіть 'yes' або 'no'.");
                }
            }

            if (userInput == "no")
            {
                break;
            }

        } while (true);
    }
}
