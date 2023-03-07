﻿using System;
using System.Collections.Generic;

namespace PolishNotation
{
    public class PostfixNotationExpression
    {
        public PostfixNotationExpression()
        {
            operators = new List<string>(standart_operators);

        }
        private List<string> operators;
        private List<string> standart_operators =
            new List<string>(new string[] { "(", ")", "+", "-", "*", "/", "^", "cos", "sin", "tg", "ctg", "ln", "log", "sqrt"});

        public List<string> Separate(string input)
        {
            List<string> arr = new List<string>();
            int pos = 0;
            while (pos < input.Length)
            {
                string s = string.Empty;
                // dobavlenie 4isla s unarnim minusom
                if (input[pos] == '-')
                {
                    if (pos == 0)
                    {
                        for (int i = pos; i < input.Length &&
                                (Char.IsLetter(input[i]) || input[i] == '-' || Char.IsDigit(input[i])); i++)
                            s += input[i];
                    }
                    else if (input[pos - 1] == '(')
                    {
                        for (int i = pos; i < input.Length &&
                                (Char.IsLetter(input[i]) || input[i] == '-' || Char.IsDigit(input[i])); i++)
                            s += input[i];
                    }
                }
                else if (standart_operators.Contains(input[pos].ToString()))
                {
                    s += input[pos];
                }
                else if (!standart_operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]))
                        for (int i = pos; i < input.Length &&
                            (Char.IsDigit(input[i]) || input[i] == '.'); i++)
                            s += input[i];
                    else if (Char.IsLetter(input[pos]))
                        for (int i = pos; i < input.Length &&
                            (Char.IsLetter(input[i]) || Char.IsDigit(input[i])); i++)
                            s += input[i];
                }
                if (s.Length == 0)
                {
                    pos += 1;
                }
                else
                {
                    pos += s.Length;
                    arr.Add(s);
                }
            }
            return arr;
        }
        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                case "cos":
                case "sin":
                case "tg":
                case "ctg":
                case "ln":
                case "log":
                case "sqrt":
                    return 3;
                default:
                    return 4;
            }
        }

        public string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string c in Separate(input))
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else
                    outputSeparated.Add(c);
            }
            if (stack.Count > 0)
                foreach (string c in stack)
                    outputSeparated.Add(c);

            return outputSeparated.ToArray();
        }
        
        public double result(string input)
        {
            Dictionary<string, string> constanti = new Dictionary<string, string>()
            {
                {"E", (Math.E).ToString()},
                {"Pi", (Math.PI).ToString()}
            };
            foreach (var c in constanti)
            {
                input = input.Replace(c.Key, c.Value);
            }
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(ConvertToPostfixNotation(input));
            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!operators.Contains(str))
                {
                    stack.Push(str);
                    str = queue.Dequeue();
                }
                else
                {
                    double summ = 0;
                    try
                    {

                        switch (str)
                        {

                            case "+":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = a + b;
                                    break;
                                }
                            case "-":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = b - a;
                                    break;
                                }
                            case "*":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = b * a;
                                    break;
                                }
                            case "/":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = b / a;
                                    break;
                                }
                            case "^":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = Math.Pow(b, a);
                                    break;
                                }
                            case "cos":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    summ = Convert.ToDouble(Math.Cos(a));
                                    break;
                                }
                            case "sin":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    summ = Convert.ToDouble(Math.Sin(a));
                                    break;
                                }
                            case "tg":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    summ = Convert.ToDouble(Math.Tan(a));
                                    break;
                                }
                            case "ctg":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    summ = Convert.ToDouble(1 / Math.Tan(a));
                                    break;
                                }
                            case "ln":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    summ = Convert.ToDouble(Math.Log(a));
                                    break;
                                }
                            case "log":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    double b = Convert.ToDouble(stack.Pop());
                                    summ = Math.Log(a, b);
                                    break;
                                }
                            case "sqrt":
                                {
                                    double a = Convert.ToDouble(stack.Pop());
                                    summ = Math.Sqrt(a);
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ERROR!");
                        Console.WriteLine(ex);
                    }
                    stack.Push(summ.ToString());
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }

            }
            return Convert.ToDouble(stack.Pop());
        }
        
    }
}
