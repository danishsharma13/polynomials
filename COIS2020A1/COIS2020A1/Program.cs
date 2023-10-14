#nullable enable

using System;
using System.Collections.Generic;

public class Term : IComparable<Term>
{
    private double coefficient;
    private int exponent;

    public double Coefficient
    {
        get { return coefficient; }
        set
        {
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("Coefficient must be between 0 and 20.");
            }
            coefficient = value;
        }
    }

    public int Exponent
    {
        get { return exponent; }
        set
        {
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("Exponent must be between 0 and 20.");
            }
            exponent = value;
        }
    }

    public Term(double coefficient, int exponent)
    {
        this.Coefficient = coefficient;
        this.Exponent = exponent;
    }

    public double Evaluate(double x)
    {
        return Coefficient * Math.Pow(x, Exponent);
    }

    public int CompareTo(Term other)
    {
        return this.Exponent.CompareTo(other.Exponent);
    }

    public override string ToString()
    {
        string sign = (Coefficient >= 0) ? "+" : "-";
        return $"{sign} {Math.Abs(Coefficient)}x^{Exponent}";
    }
}

public class Node<T>
{
    public T Item { get; set; }
    public Node<T>? Next { get; set; }

    public Node(T item, Node<T>? next)
    {
        this.Item = item;
        this.Next = next;
    }
}

public class Polynomial : ICloneable
{
    private Node<Term>? front;

    public Polynomial()
    {
        // Initialize an empty linked list for the polynomial.
        front = null;
    }

    public void AddTerm(Term t)
    {
        if (front == null)
        {
            // If the polynomial is empty, add the term as the first element.
            front = new Node<Term>(t, null);
        }
        else
        {
            Node<Term> current = front;
            Node<Term>? previous = null;

            while (current != null && current.Item.Exponent > t.Exponent)
            {
                previous = current;
                current = current.Next;
            }

            if (current != null && current.Item.Exponent == t.Exponent)
            {
                // If a term with the same exponent exists, add the coefficients.
                current.Item.Coefficient += t.Coefficient;

                // If the addition results in a zero coefficient, remove the term.
                if (current.Item.Coefficient == 0)
                {
                    if (previous == null)
                    {
                        front = current.Next;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }
                }
            }
            else
            {
                // Insert the term in proper order.
                Node<Term> newTerm = new Node<Term>(t, current);

                if (previous == null)
                {
                    front = newTerm;
                }
                else
                {
                    previous.Next = newTerm;
                }
            }
        }
    }

    public static Polynomial operator +(Polynomial p, Polynomial q)
    {
        Polynomial result = new Polynomial();

        Node<Term>? currentP = p.front;
        Node<Term>? currentQ = q.front;

        while (currentP != null && currentQ != null)
        {
            if (currentP.Item.Exponent > currentQ.Item.Exponent)
            {
                result.AddTerm(new Term(currentP.Item.Coefficient, currentP.Item.Exponent));
                currentP = currentP.Next;
            }
            else if (currentP.Item.Exponent < currentQ.Item.Exponent)
            {
                result.AddTerm(new Term(currentQ.Item.Coefficient, currentQ.Item.Exponent));
                currentQ = currentQ.Next;
            }
            else
            {
                // Exponents are equal, add coefficients.
                result.AddTerm(new Term(currentP.Item.Coefficient + currentQ.Item.Coefficient, currentP.Item.Exponent));
                currentP = currentP.Next;
                currentQ = currentQ.Next;
            }
        }

        // Add remaining terms, if any.
        while (currentP != null)
        {
            result.AddTerm(new Term(currentP.Item.Coefficient, currentP.Item.Exponent));
            currentP = currentP.Next;
        }

        while (currentQ != null)
        {
            result.AddTerm(new Term(currentQ.Item.Coefficient, currentQ.Item.Exponent));
            currentQ = currentQ.Next;
        }

        return result;
    }

    public static Polynomial operator *(Polynomial p, Polynomial q)
    {
        Polynomial result = new Polynomial();

        Node<Term>? currentP = p.front;

        while (currentP != null)
        {
            Node<Term>? currentQ = q.front;

            while (currentQ != null)
            {
                double newCoefficient = currentP.Item.Coefficient * currentQ.Item.Coefficient;
                int newExponent = currentP.Item.Exponent + currentQ.Item.Exponent;

                result.AddTerm(new Term(newCoefficient, newExponent));

                currentQ = currentQ.Next;
            }

            currentP = currentP.Next;
        }

        return result;
    }

    public double Evaluate(double x)
    {
        double result = 0.0;
        Node<Term>? current = front;

        while (current != null)
        {
            result += current.Item.Evaluate(x);
            current = current.Next;
        }

        return result;
    }

    public object Clone()
    {
        Polynomial clone = new Polynomial();
        Node<Term>? current = front;

        while (current != null)
        {
            clone.AddTerm(new Term(current.Item.Coefficient, current.Item.Exponent));
            current = current.Next;
        }

        return clone;
    }

    public void Print()
    {
        Node<Term>? current = front;
        bool isFirstTerm = true;

        while (current != null)
        {
            if (isFirstTerm)
            {
                Console.Write(current.Item);
                isFirstTerm = false;
            }
            else
            {
                Console.Write(" " + current.Item);
            }

            current = current.Next;
        }

        Console.WriteLine(); // Add a new line after printing the polynomial.
    }
}

public class Polynomials
{
    private List<Polynomial> L;

    public Polynomials()
    {
        L = new List<Polynomial>();
    }

    public Polynomial Retrieve(int i)
    {
        if (i >= 0 && i < L.Count)
        {
            return L[i];
        }
        else
        {
            throw new ArgumentOutOfRangeException("Invalid index.");
        }
    }

    public void Insert(Polynomial p)
    {
        L.Add(p);
    }

    public void Delete(int i)
    {
        if (i >= 0 && i < L.Count)
        {
            L.RemoveAt(i);
        }
        else
        {
            throw new ArgumentOutOfRangeException("Invalid index.");
        }
    }

    public int Size()
    {
        return L.Count;
    }

    public void Print()
    {
        for (int i = 0; i < L.Count; i++)
        {
            Console.Write($"Polynomial {i + 1}: ");
            L[i].Print();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Polynomials collection = new Polynomials();

        while (true)
        {
            Console.WriteLine("Polynomial Manipulation Menu:");
            Console.WriteLine("1. Create and Insert Polynomial");
            Console.WriteLine("2. Add Polynomials");
            Console.WriteLine("3. Multiply Polynomials");
            Console.WriteLine("4. Delete Polynomial");
            Console.WriteLine("5. Evaluate Polynomial");
            Console.WriteLine("6. Clone Polynomial");
            Console.WriteLine("7. Print Polynomials");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        // Create and Insert Polynomial
                        Console.WriteLine("Enter polynomial as a space-separated list of terms (e.g., 4.5x^3 -2.1x^2 6.2x -9.7):");
                        string input = Console.ReadLine();
                        Polynomial polynomial = ParsePolynomial(input);
                        if (polynomial != null)
                        {
                            collection.Insert(polynomial);
                            Console.WriteLine("Polynomial inserted.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid polynomial input.");
                        }
                        break;

                    case 2:
                        // Add Polynomials
                        PrintPolynomials(collection);
                        int index1, index2;
                        if (TryGetValidIndices(collection, out index1, out index2))
                        {
                            Polynomial result = collection.Retrieve(index1) + collection.Retrieve(index2);
                            collection.Insert(result);
                            Console.WriteLine("Polynomials added.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid indices.");
                        }
                        break;

                    case 3:
                        // Multiply Polynomials
                        PrintPolynomials(collection);
                        if (TryGetValidIndices(collection, out index1, out index2))
                        {
                            Polynomial result = collection.Retrieve(index1) * collection.Retrieve(index2);
                            collection.Insert(result);
                            Console.WriteLine("Polynomials multiplied.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid indices.");
                        }
                        break;

                    case 4:
                        // Delete Polynomial
                        PrintPolynomials(collection);
                        if (TryGetValidIndex(collection, out int deleteIndex))
                        {
                            collection.Delete(deleteIndex);
                            Console.WriteLine("Polynomial deleted.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid index.");
                        }
                        break;

                    case 5:
                        // Evaluate Polynomial
                        PrintPolynomials(collection);
                        if (TryGetValidIndex(collection, out int evalIndex))
                        {
                            Console.Write("Enter the value of x: ");
                            if (double.TryParse(Console.ReadLine(), out double x))
                            {
                                double result = collection.Retrieve(evalIndex).Evaluate(x);
                                Console.WriteLine($"Result: {result}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid value for x.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid index.");
                        }
                        break;

                    case 6:
                        // Clone Polynomial
                        PrintPolynomials(collection);
                        if (TryGetValidIndex(collection, out int cloneIndex))
                        {
                            Polynomial clone = (Polynomial)collection.Retrieve(cloneIndex).Clone();
                            collection.Insert(clone);
                            Console.WriteLine("Polynomial cloned.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid index.");
                        }
                        break;

                    case 7:
                        // Print Polynomials
                        PrintPolynomials(collection);
                        break;

                    case 0:
                        // Exit
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter a valid number.");
            }
        }
    }

    static Term? ParseTerm(string termString)
    {
        // Remove any spaces and trim the input.
        termString = termString.Replace(" ", "").Trim();

        // Check if the termString is empty.
        if (string.IsNullOrEmpty(termString))
        {
            return null; // Return null for empty terms.
        }

        double coefficient = 1.0; // Default coefficient is 1.0 if not specified.
        int exponent = 0; // Default exponent is 0 if not specified.

        // Check if the termString starts with '+' or '-' and adjust the coefficient accordingly.
        if (termString[0] == '+')
        {
            termString = termString.Substring(1); // Remove the leading '+'
        }
        else if (termString[0] == '-')
        {
            coefficient = -1.0; // Set the coefficient to -1.0 for negative terms.
            termString = termString.Substring(1); // Remove the leading '-'
        }

        // Split the termString based on 'x^'.
        string[] parts = termString.Split("x^");

        // Check if there are valid parts.
        if (parts.Length > 0)
        {
            if (!double.TryParse(parts[0], out coefficient))
            {
                throw new FormatException("Invalid coefficient.");
            }

            if (parts.Length > 1)
            {
                if (!int.TryParse(parts[1], out exponent))
                {
                    throw new FormatException("Invalid exponent.");
                }
            }
        }

        return new Term(coefficient, exponent);
    }

    static Polynomial? ParsePolynomial(string input)
    {
        input = input.Trim();
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new FormatException("Empty polynomial.");
        }

        string[] termStrings = input.Split(' ');
        Polynomial polynomial = new Polynomial();

        foreach (string termString in termStrings)
        {
            try
            {
                Term? term = ParseTerm(termString);
                if (term != null)
                {
                    polynomial.AddTerm(term);
                }
            }
            catch (FormatException)
            {
                // Skip invalid terms.
            }
        }

        return polynomial;
    }


    static void PrintPolynomials(Polynomials collection)
    {
        Console.WriteLine("Polynomials in the collection:");
        for (int i = 0; i < collection.Size(); i++)
        {
            Console.Write($"Polynomial {i + 1}: ");
            collection.Retrieve(i).Print();
        }
    }

    static bool TryGetValidIndices(Polynomials collection, out int index1, out int index2)
    {
        index1 = index2 = -1;
        Console.Write("Enter index of the first polynomial: ");
        if (TryGetValidIndex(collection, out index1))
        {
            Console.Write("Enter index of the second polynomial: ");
            if (TryGetValidIndex(collection, out index2))
            {
                return true;
            }
        }
        return false;
    }

    static bool TryGetValidIndex(Polynomials collection, out int index)
    {
        if (int.TryParse(Console.ReadLine(), out index) && IsValidIndex(index, collection))
        {
            return true;
        }
        return false;
    }

    static bool IsValidIndex(int index, Polynomials collection)
    {
        return index >= 0 && index < collection.Size();
    }
}
