using System;
using System.Collections.Generic;

namespace JWTToken.Services
{
    public class FibonacciCalculator
    {
        public static List<int> CalculateFibonacci(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Count of Fibonacci numbers can't be less than 0");
            }
            List<int> res = new List<int>();
            int first = 0;
            int second = 1;
            int temp;
            for(int i = 0; i < count; ++i)
            {
                res.Add(first);
                temp = second;
                second += first;
                first = temp;
            }
            return res;
        }
    }
}
