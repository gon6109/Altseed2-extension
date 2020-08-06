using System;
using System.Collections.Generic;
using System.Text;
using Altseed2;

namespace Altseed2Extension.Input
{
    [Serializable]
    public class InputMapping : ICloneable
    {
        public bool IsAxis;
        public int ButtonNumber;
        public int AxisNumber;
        public float AxisThreshold;
        public AxisInputType AxisType;
        public Key Key;

        [Serializable]
        public enum AxisInputType
        {
            Up,
            Down
        }

        public bool Compare(InputMapping to, bool isKey)
        {
            if (isKey)
                return Key == to.Key;
            else
            {
                if (IsAxis != to.IsAxis)
                    return false;

                if (IsAxis)
                    return AxisNumber == to.AxisNumber && AxisType == to.AxisType && AxisThreshold == to.AxisThreshold;
                else
                    return ButtonNumber == to.ButtonNumber;
            }
        }

        public static InputMapping MergeMapping(InputMapping keyboard, InputMapping joystick)
        {
            return new InputMapping()
            {
                IsAxis = joystick.IsAxis,
                ButtonNumber = joystick.ButtonNumber,
                AxisNumber = joystick.AxisNumber,
                AxisThreshold = joystick.AxisThreshold,
                AxisType = joystick.AxisType,
                Key = keyboard.Key,
            };
        }

        public object Clone()
        {
            var clone = new InputMapping();
            clone.IsAxis = IsAxis;
            clone.ButtonNumber = ButtonNumber;
            clone.AxisNumber = AxisNumber;
            clone.AxisThreshold = AxisThreshold;
            clone.AxisType = AxisType;
            clone.Key = Key;
            return clone;
        }
    }
}
