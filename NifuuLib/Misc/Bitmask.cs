using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Misc
{
    public class Bitmask
    {
        internal BitArray Values;

        public Bitmask(int[] values)
        {
            Values = new BitArray(values);
        }

        public Bitmask(int length)
        {
            Values = new BitArray(length, false);
        }

        public bool Get(int index)
        {
            return Values.Get(index);
        }

        public void Set(int index, bool state)
        {
            Values.Set(index, state);
        }

        public void Toggle(int index)
        {
            Values.Set(index, !(Values.Get(index)));
        }

        public string Export()
        {
            int[] array = new int[(Values.Length / 32) + 1];
            Values.CopyTo(array, 0);

            string output = "";

            foreach (int a in array)
            {
                output += a + " ";
            }

            return output.Trim();
        }

        public static Bitmask Import(string Input)
        {
            int[] ints = Input.Split(' ').Select(int.Parse).ToArray();
            Bitmask bitmask = new Bitmask(ints);
            return bitmask;
        }

        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < Values.Length; i++)
            {
                output += Values.Get(i) ? 1 : 0;
            }
            return output;
        }
    }
}
