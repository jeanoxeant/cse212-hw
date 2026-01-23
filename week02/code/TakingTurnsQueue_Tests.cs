using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

// TODO Problem 1 - Run test cases and record any defects the test code finds in the comment above the test method.
// DO NOT MODIFY THE CODE IN THE TESTS in this file, just the comments above the tests. 
// Fix the code being tested to match requirements and make all tests pass. 

[TestClass]
public class TakingTurnsQueueTests
{
    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3) and
    // run until the queue is empty
    // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
    // Defect(s) Found: 
    //          1 - Enqueue was adding people to the front of the queue instead of the back,
    //              making it behave like a Stack (LIFO) instead of a Queue (FIFO).
    //          2 - GetNextPerson() skipped people with exactly 1 turn left, preventing them from
    //              completing their final turn and being removed from the queue.
    //          3 - As a result, Bob only appeared once instead of twice, and Sue came out early.
    public void TestTakingTurnsQueue_FiniteRepetition()
    {
        Debug.WriteLine("*** Test: FiniteRepetition ***");
        var bob = new Person("Bob", 2);
        var tim = new Person("Tim", 5);
        var sue = new Person("Sue", 3);

        Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, sue, tim, tim];

        var players = new TakingTurnsQueue();
        players.AddPerson(bob.Name, bob.Turns);
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        int i = 0;
        while (players.Length > 0)
        {
            if (i >= expectedResult.Length)
            {
                Assert.Fail("Queue should have ran out of items by now.");
            }

            var person = players.GetNextPerson();
            // For debugging: output the current turn and expected person
            Debug.WriteLine($"Turn {i + 1}: Got {person.Name}, Expected {expectedResult[i].Name}, Queue size: {players.Length}");
            Assert.AreEqual(expectedResult[i].Name, person.Name); 
            i++; 
        }
        Debug.WriteLine($"Test completed: Processed {i} turns successfully");
    }

    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3)
    // After running 5 times, add George with 3 turns.  Run until the queue is empty.
    // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, George, Sue, Tim, George, Tim, George
    // Defect(s) Found: 
    //          1 - Enqueue adds to the front instead of the back, causing new players like George to
    //              jump ahead in the queue rather than waiting their own turn.
    //          2 - GetNextPerson() skips people with exactly 1 turn left, so Bob's second turn and
    //              Sue's final turn are skipped.
    public void TestTakingTurnsQueue_AddPlayerMidway()
    {
        Debug.WriteLine("*** Test: AddPlayerMidway ***");
        var bob = new Person("Bob", 2);
        var tim = new Person("Tim", 5);
        var sue = new Person("Sue", 3);
        var george = new Person("George", 3);

        Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, george, sue, tim, george, tim, george];

        var players = new TakingTurnsQueue();
        players.AddPerson(bob.Name, bob.Turns);
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        int i = 0;
        for (; i < 5; i++)
        {
            var person = players.GetNextPerson();
            // For debugging: output the current turn and expected person
            Debug.WriteLine($"First phase, Turn {i + 1}: Got {person.Name}, Expected {expectedResult[i].Name}");
            Assert.AreEqual(expectedResult[i].Name, person.Name);
        }
        Debug.WriteLine($"Adding George to the queue (midway)");
        players.AddPerson("George", 3);

        while (players.Length > 0)
        {
            if (i >= expectedResult.Length)
            {
                Assert.Fail("Queue should have ran out of items by now.");
            }

            var person = players.GetNextPerson();
            // For debugging: output the current turn and expected person
            Debug.WriteLine($"Second phase, Turn {i + 1}: Got {person.Name}, Expected {expectedResult[i].Name}, Queue size: {players.Length}");
            Assert.AreEqual(expectedResult[i].Name, person.Name);

            i++;
        }
        Debug.WriteLine($"Test completed: Processed {i} turns successfully");
    }

    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Bob (2), Tim (Forever), Sue (3)
    // Run 10 times.
    // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
    // Defect(s) Found: 
    //          1 - Enqueue adds to the front, causing the queue to reverse order (Stack behavior).
    //          2 - GetNextPerson() skips people with exactly 1 turn left, so Bob's and Sue's final
    //              turns don't complete properly.
    //          3 - Tim (with infinite turns) should stay in the queue, but the wrong turn order
    //              breaks the expected pattern between finite and infinite players.
    public void TestTakingTurnsQueue_ForeverZero()
    {
        Debug.WriteLine("*** Test: ForeverZero ***");
        var timTurns = 0;

        var bob = new Person("Bob", 2);
        var tim = new Person("Tim", timTurns);
        var sue = new Person("Sue", 3);

        Person[] expectedResult = [bob, tim, sue, bob, tim, sue, tim, sue, tim, tim];

        var players = new TakingTurnsQueue();
        players.AddPerson(bob.Name, bob.Turns);
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        for (int i = 0; i < 10; i++)
        {
            var person = players.GetNextPerson();
            // For debugging: output the current turn and expected person
            Debug.WriteLine($"Turn {i + 1}: Got {person.Name}, Expected {expectedResult[i].Name}, Tim's turns: {tim.Turns}");
            Assert.AreEqual(expectedResult[i].Name, person.Name);
        }

        // Verify that the people with infinite turns really do have infinite turns.
        var infinitePerson = players.GetNextPerson();
        Debug.WriteLine($"After 10 turns, Tim still has {infinitePerson.Turns} turns (infinite)");
        Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
    }

    [TestMethod]
    // Scenario: Create a queue with the following people and turns: Tim (Forever), Sue (3)
    // Run 10 times.
    // Expected Result: Tim, Sue, Tim, Sue, Tim, Sue, Tim, Tim, Tim, Tim
    // Defect(s) Found: 
    //          1 - Enqueue adds to the front, reversing the queue order (Stack behavior instead of Queue).
    //          2 - GetNextPerson() skips Sue's final turn when she has exactly 1 turn left,
    //              preventing her from completing her rotation.
    //          3 - Tim (with negative turns which translates infinite turns) should serve correctly in the expected order,
    //              but the bugs break the Tim/Sue pattern.
    public void TestTakingTurnsQueue_ForeverNegative()
    {
        Debug.WriteLine("*** Test: ForeverNegative ***");
        var timTurns = -3;
        var tim = new Person("Tim", timTurns);
        var sue = new Person("Sue", 3);

        Person[] expectedResult = [tim, sue, tim, sue, tim, sue, tim, tim, tim, tim];

        var players = new TakingTurnsQueue();
        players.AddPerson(tim.Name, tim.Turns);
        players.AddPerson(sue.Name, sue.Turns);

        for (int i = 0; i < 10; i++)
        {
            var person = players.GetNextPerson();
            Debug.WriteLine($"Turn {i + 1}: Got {person.Name}, Expected {expectedResult[i].Name}, Sue's remaining turns: {sue.Turns}");
            Assert.AreEqual(expectedResult[i].Name, person.Name);
        }

        // Verify that the people with infinite turns really do have infinite turns.
        var infinitePerson = players.GetNextPerson();
        // For debugging: output the current turn and expected person
        Debug.WriteLine($"After 10 turns, Tim still has {infinitePerson.Turns} turns (negative = infinite)");
        Debug.WriteLine($"Expected turns value: {timTurns}");
        Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
    }

    [TestMethod]
    // Scenario: Try to get the next person from an empty queue
    // Expected Result: Exception should be thrown with appropriate error message.
    // Defect(s) Found: No defects found. The empty queue correctly throws InvalidOperationException 
    //                 with the expected message "No one in the queue."
    public void TestTakingTurnsQueue_Empty()
    {
        Debug.WriteLine("*** Test: Empty ***");
        var players = new TakingTurnsQueue();
        // For debugging: output the initial length of the queue
        Debug.WriteLine($"Queue is empty. Initial length: {players.Length}");

        try
        {
            Debug.WriteLine("get next person from empty queue"); // For debugging
            players.GetNextPerson();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("No one in the queue.", e.Message);
            Debug.WriteLine($"Caught expected InvalidOperationException: '{e.Message}'");
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception e)
        {
            Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", 
            e.GetType(), e.Message)
            );
        }
    }
}