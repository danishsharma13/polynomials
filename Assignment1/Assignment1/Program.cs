// Group members: Danish Sharma, Sami Ali, Tushar Dhiman
// Group members IDs: 0623392, 0791752, 0757538
// Assignment 1 Polynomials

// -----------------------------------------------------------------------------------------------------------------

// Class Term
// Summary: The class Term encapsulates the coefficient and exponent of a single polynomial term.
//          Exponents are limited in range from 0 to 20, inclusively
public class Term : IComparable
{
    // Data members
    private double coefficient;
    private int exponent;

    // Summary: Setter and Getters for data members with implementation
    //          of exception when exponent is out of range

    // Get and Set Coefficient
    public double Coefficient
    {
        get { return this.coefficient; }
        set { this.coefficient = value; }
    }

    // Get and Set Exponent
    public int Exponent
    {
        get { return this.exponent; }
        set
        {
            // If the value is not within the 0 to 20 range then throw exception
            //      otherwise set coefficient to value entered
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("The Exponent must be between 0 and 20!");
            }
            this.exponent = value;
        }
    }

    // Summary: 2-args constructor creates a term object with the given coefficient and exponent
    public Term(double coefficient, int exponent)
    {
        // Use setter methods to create a new Term object and populate it's data members
        this.Coefficient = coefficient;
        this.Exponent = exponent;
    }

    // Summary: Evaluate method will have value for x which will be used to get the double returning
    //          value after we power it by exponent and multiply it with coefficient
    public double Evaluate(double x)
    {
        return this.Coefficient * Math.Pow(x, this.Exponent);
    }
    
    // Summary: CompareTo method compares current object to another object
    //          and returns an integer based on the comparison
    public int CompareTo(Object obj)
    {
        // If obj is null or is not a Term object then throw exception
        if (obj == null || !(obj is Term))
        {
            throw new ArgumentException("Object cannot be compared!");
        }

        Term termObj = (Term) obj;

        // If exponent is equal to obj's exponent return 0
        // If exponent is greater than obj's exponent return 1
        // Else exponent is less than obj's exponent return -1
        if (this.Exponent == termObj.Exponent)
        {
            return 0;
        }
        else if (this.Exponent > termObj.Exponent)
        {
            return 1;
        }
        else 
        { 
            return -1; 
        }
    }

    // Summary: ToString method returns a string of the Term
    public override string ToString()
    {
        // If coefficient is 0 then print out empty string
        // If exponent is greater than 1 then we display exponent integer
        //      in string
        // If exponent is 1, then we only display coefficient and x
        // Else exponent is 0, then we only show coefficient
        if (this.Coefficient == 0)
        {
            return "0";
        }
        if (this.Exponent > 1)
        {
            return $"{this.Coefficient}x^{this.Exponent}";
        }
        else if (this.Exponent == 1)
        {
            return $"{this.Coefficient}x";
        }
        else
        {
            return $"{this.Coefficient}";
        }
    }
}

// -----------------------------------------------------------------------------------------------------------------

// Class Node<T> (Generic)
// Summary: The generic class Node contains an item<T> and a reference to the next Node.
public class Node<T>
{
    // Data members
    public T Item { get; set; }
    public Node<T>? Next { get; set; }

    // Summary: 2-args constructor that creates a Node object and populates the data members
    public Node(T item, Node<T>? next)
    { 
        this.Item = item;
        this.Next = next;
    }
}

// -----------------------------------------------------------------------------------------------------------------

// Class Polynomial
// Summary: The class Polynomial will have a polynomial expression and will calculate, manipulate
//          clone and print accordingly.
public class Polynomial : ICloneable
{
    // Data members
    private Node<Term>? front;

    // Summary: No-args constructor that creates a Polynomial object and populates the data members
    public Polynomial()
    {
        // Set front to null to create a 0 polynomial
        this.front = null;
    }
    
    // Summary: AddTerm method checks the term, traverse through the linkedlist and then adds it
    //          to the appropriate term or in the list itself.
    public void AddTerm(Term t)
    {
        // If front is null, if it is null then add the term to the list
        // Else there is already nodes in the list, then add accordingly
        if (this.front == null)
        {
            this.front = new Node<Term>(t, null);
        }
        else
        {
            // Current variable assigned to front.Next to traverse the list
            // Previous variable to store info for the previous node
            Node<Term>? current = this.front;
            Node<Term>? previous = null;

            // While loop to traverse the list if current is not null
            //      and current Term compareTo t is bigger (positive 1) then
            //      keep looping
            while (current != null && current.Item.CompareTo(t) == 1)
            {
                // Set previous to current and current to current.Next
                //      to find correct term to add with
                previous = current;
                current = current.Next;
            }

            // If the current Item matches with the t term then add them
            // Else if the current Item doesn not match, make a new Node<Term>
            //      and add it in the correct order
            if (current != null && current.Item.CompareTo(t) == 0) 
            {
                // Add the coefficients.
                current.Item.Coefficient += t.Coefficient;

                // If the addition results in a zero coefficient, remove the term.
                if (current.Item.Coefficient == 0)
                {
                    // If checking the first term in linkedlist then previous will be null
                    //    make front currnet.Next to remove the current node
                    // Else make Previous.Next to current.Next to remove current node
                    if (previous == null)
                    {
                        this.front = current.Next;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }
                }
            }
            else
            {
                // Insert the new term with term t and current as constructor args in proper order.
                Node<Term> newTerm = new Node<Term>(t, current);

                // If previous is null, means front has smaller Term than t term
                // Else set previous.Next to newTerm
                if (previous == null)
                {
                    this.front = newTerm;
                }
                else
                {
                    previous.Next = newTerm;
                }
            }
        }

    }

    // Summary: Operator + for polynomial will traverse through both p and q 
    //          polynomial and add their terms accordingly and return their
    //          simplified polynomial expression
    public static Polynomial operator +(Polynomial p, Polynomial q)
    {
        // Result variable for return value, make a 0 polynomial
        Polynomial result = new Polynomial();

        // Traversing variables currentP and currentQ, set them front
        //      node p and q respectively
        Node<Term>? currentP = p.front;
        Node<Term>? currentQ = q.front;


        // While loop until currentP and currentQ are not null to traverse the list
        while (currentP != null && currentQ != null)
        {
            // If currentP compareTo currentQ is 1, then currentP is bigger and we add currentP term
            //      and move to currentP.Next
            // If currentP compareTo currentQ is -1, then currentP is smaller and we add currentQ term
            //      and move to currentQ.Next
            // Else currentP and currentQ are equal, then we add them together and then add it to result
            if (currentP.Item.Exponent.CompareTo(currentQ.Item.Exponent) == 1)
            {
                result.AddTerm(new Term(currentP.Item.Coefficient, currentP.Item.Exponent));
                currentP = currentP.Next;
            }
            else if (currentP.Item.Exponent.CompareTo(currentQ.Item.Exponent) == -1)
            {
                result.AddTerm(new Term(currentQ.Item.Coefficient, currentQ.Item.Exponent));
                currentQ = currentQ.Next;
            }
            else
            {
                result.AddTerm(new Term(currentP.Item.Coefficient + currentQ.Item.Coefficient, currentP.Item.Exponent));
                currentP = currentP.Next;
                currentQ = currentQ.Next;
            }
        }

        // If currentQ had less items, then we add remaining currentP terms in result
        while (currentP != null)
        {
            result.AddTerm(new Term(currentP.Item.Coefficient, currentP.Item.Exponent));
            currentP = currentP.Next;
        }

        // If currentP had less items, then we add remaining currentQ terms in result
        while (currentQ != null)
        {
            result.AddTerm(new Term(currentQ.Item.Coefficient, currentQ.Item.Exponent));
            currentQ = currentQ.Next;
        }

        return result;
    }

    // Summary: Operator * for polynomial will traverse through both p and q 
    //          polynomial and multiply their terms accordingly and return their
    //          simplified polynomial expression
    public static Polynomial operator *(Polynomial p, Polynomial q)
    {
        // Result variable for return value, make a 0 polynomial
        Polynomial result = new Polynomial();

        // Traversing variable currentP, set it front node (p)
        Node<Term>? currentP = p.front;

        // While loop to traverse the p polynomial list
        while (currentP != null)
        {
            // Traversing variable currentQ, set it front node (q)
            Node<Term>? currentQ = q.front;

            // While loop to traverse the q polynomial list and multiple with p polynomial terms
            // such as multiplying the cofficient and adding exponent, then adding it to result
            // then move to next q term
            while (currentQ != null)
            {
                result.AddTerm(new Term(currentP.Item.Coefficient * currentQ.Item.Coefficient, 
                    currentP.Item.Exponent + currentQ.Item.Exponent));

                currentQ = currentQ.Next;
            }

            // Move to next p term
            currentP = currentP.Next;
        }

        return result;
    }

    // Summary: Evaluate method will have value for x which will be used to get the double returning
    //          value of the evaluated polynomial
    public double Evaluate(double x)
    {
        // Result variable for return
        // Current variable to traverse the list
        double result = 0.0;
        Node<Term>? current = this.front;

        // While loop to traverse the polynomial list
        while (current != null)
        {
            // Using Term's Evaluate method, we can get the double for current term
            // and add it to result, then move to current's next node
            result += current.Item.Evaluate(x);
            current = current.Next;
        }

        return result;
    }

    // Summary: Clone method creates and returns a clone of the current polynomial except
    //          that the exponents of the current polynomial are assigned to the coefficients of
    //          the clone in reverse order. For example, 4x^3 – 3x + 9 is cloned as 9x^3 – 3x + 4
    public Object Clone()
    {
        // Stack variable to store coefficient of the polynomial and then use the pop
        // method to assign reverse order to the polynomial
        Stack<double> S = new Stack<double>();

        // Result variable for return
        Polynomial result = new Polynomial();

        // Current variable to traverse the list
        Node<Term>? current = this.front;

        // While loop to traverse the polynomial list, pushing coefficient in the stack
        //      then moving to next term
        while (current != null)
        {
            S.Push(current.Item.Coefficient);
            current = current.Next;
        }

        // Set current to front again, to traverse the polynomial again and then
        //      add each term with reversed order (S.pop) to result and move to
        //      next current node
        while (current != null)
        {
            result.AddTerm(new Term(S.Pop(), current.Item.Exponent));
            current = current.Next;
        }

        return result;
    }
    
    // Summary: Print method will print out the polynomial list in a readable
    //          string output
    public void Print()
    {
        // If front is null then the polynomial is a 0 polynomial
        // Else the polynomial is not 0 polynomial and have terms
        if (front == null)
        {
            Console.WriteLine(0);
        }
        else
        {
            // Current variable to traverse the list
            // isFirstTerm boolean to check if it is first term
            Node<Term>? current = front;
            bool isFirstTerm = true;

            // While loop to traverse through the polynomial list, print the terms with
            //      "+" sign between then, to make it readable
            while (current != null)
            {
                // If isFirstTerm is true then just print the term, then set isFirstTerm to false
                // If Coefficient is a negative value, then we print the term with a negative sign
                // Else we include the "+" sign and the current Item, then move to current next
                if (isFirstTerm)
                {
                    Console.Write(current.Item);
                    isFirstTerm = false;
                }
                else if (current.Item.Coefficient < 0)
                {
                    Console.Write(" - " + current.Item.ToString().Substring(1));
                }
                else
                {
                    Console.Write(" + " + current.Item);
                }

                current = current.Next;
            }

            Console.WriteLine();
        }
    }
}

// -----------------------------------------------------------------------------------------------------------------

// Class Polynomials
// Summary: The class Polynomials s is a collection of polynomials stored in an instance of the generic
//          library class List.
public class Polynomials
{
    // Data members
    private List<Polynomial> L;

    // Summary: No-args constructor that populates the data member L to a new List<Polynomial>
    public Polynomials()
    {
        L = new List<Polynomial>();
    }

    // Summary: Retrieve method takes in an integer that is the item number in the list and returns the
    //          polynomial in that position.
    public Polynomial Retrieve(int i)
    {
        // If the i integer is within the valid range of list then retrieve the polynomial
        // Else throw a new execption
        if (i >= 1 && i <= L.Count)
        {
            return L[i - 1];
        }
        else
        {
            throw new ArgumentOutOfRangeException("Invalid index! Please enter valid position number.");
        }
    }

    // Summary: Insert method allows user to add a new polynomial expression in the list.
    public void Insert(Polynomial p)
    {
        L.Add(p);
    }

    // Summary: Delete method takes in an integer that is the item number in the list and removes the
    //          polynomial in that position.
    public void Delete(int i)
    {
        // If the i integer is within the valid range of list then remove the polynomial
        // Else throw a new execption
        if (i >= 1 && i <= L.Count)
        {
            L.RemoveAt(i - 1);
        }
        else
        {
            throw new ArgumentOutOfRangeException("Invalid index! Please enter valid position number.");
        }
    }

    // Summary: Size method will return the lenght of the list.
    public int Size()
    {
        return L.Count;
    }

    // Summary: Print method will print all the polynomials in the list with their correct position number.
    public void Print()
    {
        // For loop until we print all polynomial in the list
        for (int i = 0; i < L.Count; i++)
        {
            Console.Write($"Polynomial Position {i + 1}: ");
            L[i].Print();
        }
    }
}

// -----------------------------------------------------------------------------------------------------------------

// Class Program
// Summary: The Program class is created to create the user inferface so they can manipulate the Polynomials,
//          Polynomial, Node and Term classes.
class Program
{
    // Summary: Main program that shows user the User Interface
    static void Main(string[] args)
    {
        // Collection of polynomials
        Polynomials S = new Polynomials();

        // If exit is false then the while loop will keep running
        bool exit = false;

        // While exit is false, keep looping and ask user for their choice
        while (!exit)
        {
            Console.WriteLine("\n\n   Choose from the following:");
            Console.WriteLine("1) To create a polynomial and insert it into S");
            Console.WriteLine("2) To add two polynomials from S (retrieved by index) and to insert the resultant polynomial into S");
            Console.WriteLine("3) To multiply two polynomials from S (retrieved by index) and to insert the resultant polynomial into S");
            Console.WriteLine("4) To delete the polynomial from S at a given index");
            Console.WriteLine("5) To evaluate the polynomial from S at a given index");
            Console.WriteLine("6) To clone the polynomial from S (retrieved by index) and to insert its clone into S");
            Console.WriteLine("7) EXIT the program");
            Console.WriteLine();

            // Get user input that matches with the options above
            Console.Write("Your input: ");
            string userInput = Console.ReadLine();
            Console.WriteLine();

            // Parse the user input and then use switch statement to display menu and do the list manulipation according to
            // option number input
            if (int.TryParse(userInput, out int number))
            {
                switch (number)
                {
                    case 1:
                        option1(ref S);
                        break;
                    case 2:
                        option2(ref S);
                        break;
                    case 3:
                        option3(ref S);
                        break;
                    case 4:
                        option4(ref S);
                        break;
                    case 5:
                        option5(ref S);
                        break;
                    case 6:
                        option6(ref S);
                        break;
                    case 7:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\n\nInvalid choice. Please select a valid option.\n\n");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\n\nInvalid input. Please enter a valid option.\n\n");
            }
        }
    }

    // Summary: Option1 method is to display sub menu system that allows users to create new polynomials
    static void option1(ref Polynomials collectionS)
    {
        // New polynomial that will have new terms added to it and then pushed to Polynomials S
        Polynomial p = new Polynomial();

        // If exit is false then the while loop will keep running
        bool exit = false;

        // While exit is false, keep looping and ask user for to add terms to polynomial
        while (!exit)
        {
            Console.WriteLine("\n\n   Option 1 Menu: Creating Polynomial.");
            Console.WriteLine("1) Add term");
            Console.WriteLine("2) Save polynomial (If no new terms added, then save will just create 0 polynomial and save it.");
            Console.WriteLine("3) EXIT Option 1 program!");

            // Get user input that matches with the options above
            Console.Write("\nYour input: ");
            string userInput = Console.ReadLine();

            // Parse the user input and then use switch statement to display menu and do the list manulipation according to
            // option number input
            if (int.TryParse(userInput, out int number))
            {
                if (number == 1)
                {
                    // Boolean to check if the coefficient and exponent was proper read from user input
                    bool readCoef = false;
                    bool readExpo = false;

                    // Display and read coefficient and exponent menu entry
                    Console.Write("Enter Coefficient (double): ");
                    string readCoefficient = Console.ReadLine();
                    double coef = 0.0;

                    Console.Write("Enter Exponent (integer): ");
                    string readExponent = Console.ReadLine();
                    int expo = -1;

                    // Parse both coefficient and exponent number and assign them to respective variables.
                    if (double.TryParse(readCoefficient, out double coefficientNumber))
                    {
                        coef = coefficientNumber;
                        readCoef = true;
                    }
                    else
                    {
                        Console.WriteLine("\n\nInvalid input. Please enter a valid coefficient.\n\n");
                    }

                    if (int.TryParse(readExponent, out int exponentNumber))
                    {
                        expo = exponentNumber;
                        readExpo = true;
                    }
                    else
                    {
                        Console.WriteLine("\n\nInvalid input. Please enter a valid coefficient.\n\n");
                    }

                    // If both coefficient and exponent are valid then add them to p polynomial
                    if (readCoef && readExpo)
                    {
                        p.AddTerm(new Term(coef, expo));
                        Console.WriteLine("\n\nPolynomial term added, current polynomial: ");
                        p.Print();
                    }
                }
                else if (number == 2)
                {
                    collectionS.Insert(p);
                    Console.WriteLine("\n\nPolynomial added to list: ");
                    collectionS.Print();
                }
                else if(number == 3)
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
            }

        }
    }

    // Summary: Option2 method is to display sub menu for adding 2 polynomials based on indexes
    static void option2(ref Polynomials collectionS)
    {
        // If exit is false then the while loop will keep running
        bool exit = false;

        // While exit is false, keep looping and ask user for valid index to add polynomials
        while (!exit)
        {
            Console.WriteLine("\n\n   Option 2 Menu: Add 2 Polynomials.");
            Console.WriteLine("1) Input 2 index which you want to add together");
            Console.WriteLine("2) EXIT Option 2 program!");

            Console.WriteLine("\n\nCurrent Polynomials List: ");
            collectionS.Print();
            Console.WriteLine();

            // Get user input that matches with the options above
            Console.Write("Your input: ");
            string userInput = Console.ReadLine();

            // Parse the user input and then use switch statement to display menu and do the list manulipation according to
            // option number input
            if (int.TryParse(userInput, out int number))
            {
                if(number == 1)
                {
                    // Boolean to check if the indexes were properly read from user input
                    bool readIdx1 = false;
                    bool readIdx2 = false;

                    // Display and take user input for indexes
                    Console.Write("\nEnter 1st index: ");
                    string idx1Input = Console.ReadLine();
                    int idx1 = -1;

                    Console.Write("\nEnter 2nd index: ");
                    string idx2Input = Console.ReadLine();
                    int idx2 = -1;

                    // Parse the integers
                    if (int.TryParse(idx1Input, out int numberIdx1))
                    {
                        idx1 = numberIdx1;
                        readIdx1 = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid index.");
                    }

                    // Parse the integers
                    if (int.TryParse(idx2Input, out int numberIdx2))
                    {
                        idx2 = numberIdx2;
                        readIdx2 = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid index.");
                    }

                    // If both index are valid integers then add the polynomials and store it in the list
                    if (readIdx1 && readIdx2)
                    {
                        collectionS.Insert(collectionS.Retrieve(idx1) + collectionS.Retrieve(idx2));
                        Console.WriteLine("\n\nPolynomials have been added and saved in the Polynomial list: ");
                        collectionS.Print();
                    }
                }
                else if (number == 2)
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
            }
        }
    }

    // Summary: Option3 method is to display sub menu for multiplying 2 polynomials based on indexes
    static void option3(ref Polynomials collectionS)
    {
        // If exit is false then the while loop will keep running
        bool exit = false;

        // While exit is false, keep looping and ask user for valid index to add polynomials
        while (!exit)
        {
            Console.WriteLine("\n\n   Option 3 Menu: Multiply 2 Polynomials.");
            Console.WriteLine("1) Input 2 index which you want to multiple together");
            Console.WriteLine("2) EXIT Option 3 program!");

            Console.WriteLine("\n\nCurrent Polynomials List: ");
            collectionS.Print();
            Console.WriteLine();

            // Get user input that matches with the options above
            Console.Write("Your input: ");
            string userInput = Console.ReadLine();

            // Parse the user input and then use switch statement to display menu and do the list manulipation according to
            // option number input
            if (int.TryParse(userInput, out int number))
            {
                if (number == 1)
                {
                    // Boolean to check if the indexes were properly read from user input
                    bool readIdx1 = false;
                    bool readIdx2 = false;

                    // Display and take user input for indexes
                    Console.Write("\nEnter 1st index: ");
                    string idx1Input = Console.ReadLine();
                    int idx1 = -1;

                    Console.Write("\nEnter 2nd index: ");
                    string idx2Input = Console.ReadLine();
                    int idx2 = -1;

                    // Parse the integers
                    if (int.TryParse(idx1Input, out int numberIdx1))
                    {
                        idx1 = numberIdx1;
                        readIdx1 = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid index.");
                    }

                    // Parse the integers
                    if (int.TryParse(idx2Input, out int numberIdx2))
                    {
                        idx2 = numberIdx2;
                        readIdx2 = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid index.");
                    }

                    // If both index are valid integers then add the polynomials and store it in the list
                    if (readIdx1 && readIdx2)
                    {
                        collectionS.Insert(collectionS.Retrieve(idx1) * collectionS.Retrieve(idx2));
                        Console.WriteLine("\n\nPolynomials have been multiplied and saved in the Polynomial list: ");
                        collectionS.Print();
                    }
                }
                else if (number == 2)
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
            }
        }
    }

    // Summary: Option4 method is to display sub menu system and to delete a polynomial at an index
    static void option4(ref Polynomials collectionS)
    {
        // If exit is false then the while loop will keep running
        bool exit = false;

        // While exit is false, keep looping and ask user for valid index to add polynomials
        while (!exit)
        {
            Console.WriteLine("\n\n   Option 4 Menu: Deleting Polynomials.");
            Console.WriteLine("1) Input an index which you want to delete from the list");
            Console.WriteLine("2) EXIT Option 4 program!");

            Console.WriteLine("\n\nCurrent Polynomials List: ");
            collectionS.Print();
            Console.WriteLine();

            // Get user input that matches with the options above
            Console.Write("Your input: ");
            string userInput = Console.ReadLine();

            // Parse the user input and then use switch statement to display menu and do the list manulipation according to
            // option number input
            if (int.TryParse(userInput, out int number))
            {
                if (number == 1)
                {
                    // Boolean to check if the index is properly read from user input
                    bool readIdx1 = false;

                    // Display and take user input for indexes
                    Console.Write("\nEnter index: ");
                    string idx1Input = Console.ReadLine();
                    int idx1 = -1;

                    // Parse the integers
                    if (int.TryParse(idx1Input, out int numberIdx1))
                    {
                        idx1 = numberIdx1;
                        readIdx1 = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid index.");
                    }

                    // If index is valid integers then remove the polynomials
                    if (readIdx1)
                    {
                        collectionS.Delete(idx1);
                        Console.WriteLine("\n\nPolynomials have been removed from the Polynomial list: ");
                        collectionS.Print();
                    }
                }
                else if (number == 2)
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
            }
        }
    }

    // Summary: Option5 method is to display sub menu system and evalute a polynomial expressive with given x value
    static void option5(ref Polynomials collectionS)
    {
        // If exit is false then the while loop will keep running
        bool exit = false;

        // While exit is false, keep looping and ask user for valid index to add polynomials
        while (!exit)
        {
            Console.WriteLine("\n\n   Option 5 Menu: Evaluate Polynomials.");
            Console.WriteLine("1) Input an index which you want to Evaluate and input x value");
            Console.WriteLine("2) EXIT Option 4 program!");

            Console.WriteLine("\n\nCurrent Polynomials List: ");
            collectionS.Print();
            Console.WriteLine();

            // Get user input that matches with the options above
            Console.Write("Your input: ");
            string userInput = Console.ReadLine();

            // Parse the user input and then use switch statement to display menu and do the list manulipation according to
            // option number input
            if (int.TryParse(userInput, out int number))
            {
                if (number == 1)
                {
                    // Boolean to check if the index and x value is properly read from user input
                    bool readIdx1 = false;
                    bool readX = false;

                    // Display and take user input for indexes
                    Console.Write("\nEnter index: ");
                    string idx1Input = Console.ReadLine();
                    int idx1 = -1;

                    // Display and take user input for x value
                    Console.Write("\nEnter x value: ");
                    string xValueInput = Console.ReadLine();
                    int xVal = -1;

                    // Parse the integers
                    if (int.TryParse(idx1Input, out int numberIdx1))
                    {
                        idx1 = numberIdx1;
                        readIdx1 = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid index.");
                    }

                    // Parse the integers
                    if (int.TryParse(xValueInput, out int numberXVal))
                    {
                        xVal = numberXVal;
                        readX = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid X value.");
                    }

                    // If index is valid integers then remove the polynomials
                    if (readIdx1 && readX)
                    {
                        Console.WriteLine("\n\nPolynomial have been evaluated: ");
                        collectionS.Retrieve(idx1).Print();
                        Console.WriteLine("Output: " + collectionS.Retrieve(idx1).Evaluate(xVal) );
                    }
                }
                else if (number == 2)
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
            }
        }
    }

    // Summary: Option6 method is to display sub menu system and allow user to clone a polynomial and
    //          store it in the list
    static void option6(ref Polynomials collectionS)
    {
        // If exit is false then the while loop will keep running
        bool exit = false;

        // While exit is false, keep looping and ask user for valid index to add polynomials
        while (!exit)
        {
            Console.WriteLine("\n\n   Option 6 Menu: Clone Polynomials.");
            Console.WriteLine("1) Input an index which you want to clone from the list");
            Console.WriteLine("2) EXIT Option 4 program!");

            Console.WriteLine("\n\nCurrent Polynomials List: ");
            collectionS.Print();
            Console.WriteLine();

            // Get user input that matches with the options above
            Console.Write("Your input: ");
            string userInput = Console.ReadLine();

            // Parse the user input and then use switch statement to display menu and do the list manulipation according to
            // option number input
            if (int.TryParse(userInput, out int number))
            {
                if (number == 1)
                {
                    // Boolean to check if the index is properly read from user input
                    bool readIdx1 = false;

                    // Display and take user input for indexes
                    Console.Write("\nEnter index: ");
                    string idx1Input = Console.ReadLine();
                    int idx1 = -1;

                    // Parse the integers
                    if (int.TryParse(idx1Input, out int numberIdx1))
                    {
                        idx1 = numberIdx1;
                        readIdx1 = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid index.");
                    }

                    // If index is valid integers then clone the polynomial and add to list
                    if (readIdx1)
                    {
                        collectionS.Insert((Polynomial)collectionS.Retrieve(idx1).Clone());
                        Console.WriteLine("\n\nPolynomials have been removed from the Polynomial list: ");
                        collectionS.Print();
                    }
                }
                else if (number == 2)
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid option.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid option.");
            }
        }
    }
}

