using System;
using Ignix.EventBusSystem;
using NUnit.Framework;

public class EventBusTests
{
    private class TestArgs : EventArgs
    {
        public int argsValue;
    }
    
    private EventBus _eventBus;
    
    [SetUp]
    public void Setup()
    {
        _eventBus = new EventBus();
    }

    [Test]
    [TestCase(1)]
    [TestCase(5)]
    [TestCase(-10)]
    public void Event_Registered_Correctly(int testValue)
    {
        int value = 0;

        void OnEventCalled(TestArgs args)
        {
            value = args.argsValue;
        }
        
        Assert.AreEqual(0, value);
        
        _eventBus.Register<TestArgs>(OnEventCalled);
        _eventBus.Send(new TestArgs() { argsValue = testValue});
        _eventBus.ExecuteQueue();
        
        Assert.AreEqual(testValue, value);
        
        _eventBus.Unregister<TestArgs>(OnEventCalled);
        _eventBus.Send(new TestArgs() { argsValue = testValue + 10});
        _eventBus.ExecuteQueue();

        Assert.AreEqual(testValue, value);
    }
}
