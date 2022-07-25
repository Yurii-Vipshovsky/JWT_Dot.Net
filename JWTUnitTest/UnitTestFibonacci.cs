using JWTToken.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace JWTUnitTest
{
    [TestClass]
    public class UnitTestFibonacci
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
         "Count of Fibonacci numbers can't be less than 0")]
        public void TestFibonacciCalculatorBelowZero()
        {
            FibonacciCalculator.CalculateFibonacci(-1);
        }
        [TestMethod]
        public void TestFibonacciCalculator()
        {
            CollectionAssert.AreEqual(new List<int>() { 0, 1, 1, 2 },
            FibonacciCalculator.CalculateFibonacci(4));
            CollectionAssert.AreEqual(new List<int>() { 0, 1, 1, 2, 3, 5 },
            FibonacciCalculator.CalculateFibonacci(6));
            CollectionAssert.AreEqual(new List<int>() { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181 },
            FibonacciCalculator.CalculateFibonacci(20));
        }
    }
}
