using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ozel
{
    public static class ConvertAngle
    {
        public static float ConvertToAngle180(float input)
        {
            while (input > 360)
            {
                input = input - 360;
            }
            while (input < -360)
            {
                input = input + 360;
            }
            if (input > 180)
            {
                input = input - 360;
            }
            if (input < -180)
            {
                input = 360 + input;
            }

            return input;
        }
    }


}
