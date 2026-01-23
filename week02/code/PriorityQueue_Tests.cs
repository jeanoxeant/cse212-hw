using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue Items with Different Priorities and Dequeue the Highest Priority Item.
    // Expected Result: The item with the highest priority should be dequeued first.
    // Defect(s) Found: 
    //          1 - The priority comparison in Dequeue() uses '>=' which causes incorrect Dequeue behavior
    //              Instead of Dequeueing the highest priority item, it dequeues the last item with 
    //              the highest priority due to the '>=' comparison.
    public void TestPriorityQueue_DequeueHighestPriority()
    {
        Debug.WriteLine("*** Test 1: DequeueHighestPriority ***");
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("LowPriorityItem", 1);
        priorityQueue.Enqueue("HighPriorityItem", 10);
        priorityQueue.Enqueue("MediumPriorityItem", 5);

        Debug.WriteLine(priorityQueue.ToString()); // Debug output to check the queue state

        var dequeuedItem = priorityQueue.Dequeue(); // Should dequeue "HighPriorityItem"
        Debug.WriteLine($"Dequeued Item: {dequeuedItem}"); // Debug output to check the dequeued item

        Assert.AreEqual("HighPriorityItem", dequeuedItem, "The dequeued item should be the one with the highest priority.");
    }

    [TestMethod]
    // Scenario: Enqueue Items with Duplicate Priorities and Dequeue the first Enqueued Item
    // Expected Result: The first item enqueued with the highest priority should be dequeued.
    // Defect(s) Found: 
    //      1 - When multiple items share the same priority, the queue does not preserve the
    //          order in which they were added. The expected behavior is FIFO for equal priorities (first enqueued, first dequeued).
    //      2 - The priority comparison treats equal priorities as "higher" (uses '>='), which
    //          causes a later-enqueued item to win ties( meaning the last one added with that priority is dequeued first).
    //      3 - The Dequeue() implementation removes the wrong element from the list.
    //          It must remove the item at the index we found as highest priority, not some other
    //          element (which results in the wrong item being returned/removed).
    public void TestPriorityQueue_DequeueDuplicatePriorities()
    {
        Debug.WriteLine("*** Test 2: DequeueDuplicatePriorities ***");
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("FirstItem", 5);
        priorityQueue.Enqueue("SecondItem", 5);
        priorityQueue.Enqueue("ThirdItem", 5);

        Debug.WriteLine(priorityQueue.ToString()); // Debug output to check the queue state
        var dequeuedItem = priorityQueue.Dequeue(); // Should dequeue "FirstItem"
        Debug.WriteLine($"Dequeued Item: {dequeuedItem}"); // Debug output to check the dequeued item
        
        Assert.AreEqual("FirstItem", dequeuedItem, "The dequeued item should be the first one enqueued with the highest priority.");
        
    }

    // Add more test cases as needed below.
    [TestMethod]
    // Scenario: Dequeue from an Empty Queue
    // Expected Result: An InvalidOperationException should be thrown.
    // Defect(s) Found: None
    public void TestPriorityQueue_DequeueEmptyQueue()
    {
        Debug.WriteLine("*** Test 3: DequeueEmptyQueue ***");
        var priorityQueue = new PriorityQueue();
        try
        {
            priorityQueue.Dequeue();
            Debug.WriteLine("An expected Exception was not thrown.");
            Assert.Fail("Expected an exception to be thrown");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message, "The exception message should indicate that the queue is empty.");
            Debug.WriteLine("Caught expected exception: " + e.Message);
        }
    }
}