// Group members: Danish Sharma, Sami, Tushar
// Group members IDs: 0623392, 
// Assignment 1 Polynomials

// Class Term
// Summary: The class Term encapsulates the coefficient and exponent of a single polynomial term.
//          Exponents are limited in range from 0 to 20, inclusively
public class Term : IComparable
{
    // Data members
    private double coefficient;
    private int exponent;

    // Name:    Danish
    // Summary: Setter and Getters for data members with implementation
    //          of exception when exponent is out of range
    public void setCoefficient(double value)
    {
        coefficient = value;
    }

    public void setExponent(int value)
    {
        // If the value is not within the 0 to 20 range then throw exception
        // otherwise set coefficient to value entered
        if (value < 0 || value > 20)
        {
            throw new ArgumentOutOfRangeException("The value must be between 0 and 20!");
        }
        exponent = value;
    }

    public double getCoefficient()
    {
        return coefficient;
    }

    public int getExponent() 
    { 
        return exponent; 
    }

    // Name:    Danish
    // Summary: 2-args constructor creates a term object with the given coefficient and exponent
    public Term(double coefficient, int exponent)
    {
        // Use setter methods to create a new Term object and populate it's
        // data members
        setCoefficient(coefficient);
        setExponent(exponent);
    }

    // Evaluates the current term at x and returns the result
    /*public double Evaluate(double x)
    {

    }*/
    
    // Name:    Danish
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
        // If exponent is less than obj's exponent return -1
        if (this.getExponent() == termObj.getExponent())
        {
            return 0;
        }
        else if (this.getExponent() > termObj.getExponent())
        {
            return 1;
        }
        else 
        { 
            return -1; 
        }
    }

    // Name:    Danish
    // Summary: CompareTo method compares current object to another object
    //          and returns an integer based on the comparison
    public override string ToString()
    {
        return $"{this.coefficient}x^{this.exponent}";
    }
}
