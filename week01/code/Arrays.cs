using System.IO.Pipelines;

public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // TODO Problem 1 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.

        /* 1. I will create an array of doubles with the specified length and I will call it 'result'.
           2. I will create a for loop that will loop through the array indices from 0 to length - 1.
           3. For each index i, I will calculate the multiple by multiplying 'number' with (i + 1).
           4.  I will store the calculated multiple in the array at index i.
           5. After the loop, return the filled array.
        */


        double[] result = new double[length]; //creating an array of doubles with the specified length. 

        for (int i = 0; i < length; i++) //looping through the array indices from 0 to length - 1
        {
            // Calculate the multiple by multiplying 'number' with (i + 1)
            result[i] = number * (i + 1); //storing the calculated multiple in the array at index i
        }
        return result; //returning the filled array
        // TODO Problem 1 End

    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // TODO Problem 2 Start
        // Remember: Using comments in your program, write down your process for solving this problem
        // step by step before you write the code. The plan should be clear enough that it could
        // be implemented by another person.
        /* 1. I will create a temporary list to hold the rotated values.
           2. I will add the last 'amount' of elements from the original list to the temporary list.
           3. I will add the remaining elements from the original list to the temporary list.
           4. I will clear the original list.
           5. I will copy the elements from the temporary list back to the original list.
        */


        List<int> temp = new List<int>(); //1. Create a temporary list to hold the rotated values.
        for (int i = data.Count - amount; i < data.Count; i++) //
        {
            temp.Add(data[i]); //2. Add the last 'amount' of elements from the original list to the temporary list.
        }
        for (int i = 0; i < data.Count - amount; i++) //3. Add the remaining elements from the original list to the temporary list.
        {
            temp.Add(data[i]); //4. Clear the original list and copy the elements from the temporary list back to the original list.
        }
        data.Clear(); //5. Clear the original list.
        data.AddRange(temp); //6. Copy the elements from the temporary list back to the original list.  
        // TODO Problem 2 End
    }
}
