using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PogisOS
{
    class CustomConsole
    {
        // Variables
        int line = 0;
        int column = 0;

        // Functions
        public void Console()
        {

        }

        public void print(string data)
        {
            foreach (char letter in data)
            {
                column += 10;
                if (letter == '\n')
                {
                    column = 0;
                    line += 16;
                }
            }

        }
    }
}
