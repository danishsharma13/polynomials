// Group members: Danish Sharma, Sami, Tushar
// Group members IDs: 0623392, 
// Assignment 1 Polynomials

// -------------------------------------------------------------------------------------------------------

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
        // If exponent is greater than 1 then we display exponent integer
        //      in string
        // If exponent is 1, then we only display coefficient and x
        // Else exponent is 0, then we only show coefficient
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

// -------------------------------------------------------------------------------------------------------

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

// -------------------------------------------------------------------------------------------------------

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
            if (current != null&& current.Item.CompareTo(t) == 0) 
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
                double newCoefficient = currentP.Item.Coefficient * currentQ.Item.Coefficient;
                int newExponent = currentP.Item.Exponent + currentQ.Item.Exponent;

                result.AddTerm(new Term(newCoefficient, newExponent));

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
        // Current variable to traverse the list
        // isFirstTerm boolean to check if it is first term
        Node<Term>? current = front;
        bool isFirstTerm = true;

        // While loop to traverse through the polynomial list, print the terms with
        //      "+" sign between then, to make it readable
        while (current != null)
        {
            // If isFirstTerm is true then just print the term, then set isFirstTerm to false
            // Else we include the "+" sign and the current Item, then move to current next
            if (isFirstTerm)
            {
                Console.Write(current.Item);
                isFirstTerm = false;
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


