using System;
using System.Collections.Generic;
using System.IO;

class FirmDish
{
    private int ingredientsCount;
    private List<double> ingredientPrices;
    private List<string> ingredientNames;
    private double totalWeight;
    private static int dishCount = 0;

    //constructor
    public FirmDish()
    {
        ingredientsCount = 0;
        ingredientPrices = new List<double>();
        ingredientNames = new List<string>();
        totalWeight = 0;
        dishCount++;
    }

    //constructor with parameters
    public FirmDish(int count, List<double> prices, List<string> names, double weight)
    {
        ingredientsCount = count;
        ingredientPrices = new List<double>(prices);
        ingredientNames = new List<string>(names);
        totalWeight = weight;
        dishCount++;
    }

    //copy constructor
    public FirmDish(FirmDish other)
    {
        ingredientsCount = other.ingredientsCount;
        ingredientPrices = new List<double>(other.ingredientPrices);
        ingredientNames = new List<string>(other.ingredientNames);
        totalWeight = other.totalWeight;
        dishCount++;
    }

    //loads data from the file
    public FirmDish(string filename)
    {
        ingredientPrices = new List<double>();
        ingredientNames = new List<string>();

        
        var lines = File.ReadAllLines(filename);
        ingredientsCount = int.Parse(lines[0]);

        for (int i = 1; i <= ingredientsCount; i++)
        {
            var ingredientData = lines[i].Split(',');
            ingredientNames.Add(ingredientData[0]);
            ingredientPrices.Add(double.Parse(ingredientData[1]));
        }

        totalWeight = double.Parse(lines[ingredientsCount + 1]);
    
    }

    public void DisplayRecipe()
    {
        Console.WriteLine($"Dish Recipe:");
        Console.WriteLine($"Number of ingredients: {ingredientsCount}");
        Console.WriteLine($"Ingredients:");
        for (int i = 0; i < ingredientsCount; i++)
        {
            Console.WriteLine($"{ingredientNames[i]} - {ingredientPrices[i]} rub");
        }
        Console.WriteLine($"Total dish weight: {totalWeight} kg");
    }

    //values from the keyboard
    public void FillFromKeyboard()
    {
        Console.Write("Enter the number of ingredients: ");
        ingredientsCount = int.Parse(Console.ReadLine());

        ingredientPrices.Clear();
        ingredientNames.Clear();

        for (int i = 0; i < ingredientsCount; i++)
        {
            Console.Write($"Enter the name of ingredient {i + 1}: ");
            ingredientNames.Add(Console.ReadLine());

            double price;
            do
            {
                Console.Write($"Enter the price of ingredient {i + 1}: ");
            } while (!double.TryParse(Console.ReadLine(), out price) || price <= 0);
            ingredientPrices.Add(price);
        }

        double weight;
        do
        {
            Console.Write("Enter the total weight of the dish: ");
        } while (!double.TryParse(Console.ReadLine(), out weight) || weight <= 0);
        totalWeight = weight;
    }

    //random
    public void FillRandomValues()
    {
        Random rand = new Random();
        ingredientsCount = rand.Next(1, 6);
        ingredientPrices.Clear();
        ingredientNames.Clear();
        for (int i = 0; i < ingredientsCount; i++)
        {
            ingredientNames.Add("Ingredient" + (i + 1));ingredientPrices.Add(rand.NextDouble() * 100);
        }
        totalWeight = rand.NextDouble() * 10;
    }

    //most complex dish
    public static FirmDish GetMostComplexDish(List<FirmDish> menu)
    {
        FirmDish mostComplexDish = menu[0];
        foreach (var dish in menu)
        {
            if (dish.ingredientsCount > mostComplexDish.ingredientsCount)
            {
                mostComplexDish = dish;
            }
        }
        return mostComplexDish;
    }

    //the total cost of the dish
    public double CalculateTotalCost()
    {
        double totalCost = 0;
        foreach (var price in ingredientPrices)
        {
            totalCost += price;
        }
        return totalCost;
    }

    //client()
    public bool Client(double money, float stomachCapacity)
    {
        double totalCost = CalculateTotalCost();
        if (money >= totalCost && stomachCapacity >= totalWeight)
        {
            return true;
        }
        return false;
    }

    //number of dishes created
    public static int DishCount => dishCount;

    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine(ingredientsCount);
            for (int i = 0; i < ingredientsCount; i++)
            {
                writer.WriteLine($"{ingredientNames[i]},{ingredientPrices[i]}");
            }
            writer.WriteLine(totalWeight);
        }
        Console.WriteLine($"Dish data saved to {filename}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<FirmDish> menu = new List<FirmDish>();

        //adding dishes to the menu with different constructors
        FirmDish dish1 = new FirmDish();
        FirmDish dish2 = new FirmDish(3, new List<double> { 10, 20, 30 }, new List<string> { "onion", "potato", "beef" }, 5.5);
        FirmDish dish3 = new FirmDish(dish2);

        //adding dishes to the menu
        menu.Add(dish1);
        menu.Add(dish2);
        menu.Add(dish3);

        //the recipe of each dish
        foreach (var dish in menu)
        {
            dish.DisplayRecipe();
            Console.WriteLine();
        }

        //saving data of all dishes to text files
        foreach (var dish in menu)
        {
            string filename = $"{dish.GetType().Name}_{Guid.NewGuid()}.txt";
            dish.SaveToFile(filename);
        }

        //dishes with random values
        Console.WriteLine("Dishes with random values:");
        foreach (var dish in menu)
        {
            dish.FillRandomValues();
            dish.DisplayRecipe();
            Console.WriteLine();
            string filename = $"Dish_{Guid.NewGuid()}.txt";
            dish.SaveToFile(filename);
        }

        //last dish from the keyboard
        Console.WriteLine("Enter data for the last dish:");
        FirmDish userDish = new FirmDish();
        userDish.FillFromKeyboard();
        userDish.DisplayRecipe();
        userDish.SaveToFile($"Dish_{Guid.NewGuid()}.txt");

        //total cost of each dish
        Console.WriteLine("Total cost of each dish:");
        foreach (var dish in menu)
        {
            Console.WriteLine($"Dish cost: {dish.CalculateTotalCost()} rub");
        }

        //most complex dish
        FirmDish mostComplexDish = FirmDish.GetMostComplexDish(menu);
        Console.WriteLine("\nThe most complex dish:");
        mostComplexDish.DisplayRecipe();

        //total cost
        double totalCostOfDishes = 0;
        foreach (var dish in menu)
        {
            totalCostOfDishes += dish.CalculateTotalCost();
        }
        Console.WriteLine($"\nTotal cost of all dishes: {totalCostOfDishes} rub");

        //clientfunction
        double moneyInWallet = 50;
        float stomachCapacity = 5;
        foreach (var dish in menu)
        {
            bool canAfford = dish.Client(moneyInWallet, stomachCapacity);
            Console.WriteLine($"Can the client afford this dish? {(canAfford ? "Yes" : "No")}");
        }

        //the number of dishes
        Console.WriteLine($"\nNumber of dishes created: {FirmDish.DishCount}");
    }
}