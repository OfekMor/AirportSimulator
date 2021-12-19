using System;

namespace Simulator
{
    class Program
    {
        static Logic logic = new Logic();
        static void Main(string[] args)
        {
            while (true)
            {
                logic.SimulatorRun();
            }
        }
    }
}
