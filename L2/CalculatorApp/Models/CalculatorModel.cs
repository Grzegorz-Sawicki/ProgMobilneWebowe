﻿namespace CalculatorApp.Models
{
    public class CalculatorModel
    {
        public double Number1 { get; set; }
        public double Number2 { get; set; }
        public double Result { get; set; }
        public bool ready { get; set; } = false;
    }
}