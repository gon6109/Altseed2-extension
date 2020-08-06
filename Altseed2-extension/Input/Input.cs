using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Input
{
    /// <summary>
    /// 入力
    /// </summary>
    public static class Input
    {
        static Dictionary<Inputs, int> inputState;
        static Dictionary<Inputs, InputMapping> inputMappings = new Dictionary<Inputs, InputMapping>();

        public static void InitInput()
        {
            inputState = new Dictionary<Inputs, int>();
            foreach (Inputs item in Enum.GetValues(typeof(Inputs)))
            {
                inputState[item] = 0;
            }

            inputMappings[Inputs.Up] = new InputMapping()
            {
                IsAxis = true,
                AxisNumber = 1,
                AxisThreshold = 0.5f,
                AxisType = InputMapping.AxisInputType.Up,
                Key = Key.Up,
            };

            inputMappings[Inputs.Down] = new InputMapping()
            {
                IsAxis = true,
                AxisNumber = 1,
                AxisThreshold = -0.5f,
                AxisType = InputMapping.AxisInputType.Down,
                Key = Key.Down,
            };

            inputMappings[Inputs.Left] = new InputMapping()
            {
                IsAxis = true,
                AxisNumber = 0,
                AxisThreshold = -0.5f,
                AxisType = InputMapping.AxisInputType.Down,
                Key = Key.Left,
            };

            inputMappings[Inputs.Right] = new InputMapping()
            {
                IsAxis = true,
                AxisNumber = 0,
                AxisThreshold = 0.5f,
                AxisType = InputMapping.AxisInputType.Up,
                Key = Key.Right,
            };

            inputMappings[Inputs.A] = new InputMapping()
            {
                IsAxis = false,
                ButtonNumber = 0,
                Key = Key.Z,
            };

            inputMappings[Inputs.B] = new InputMapping()
            {
                IsAxis = false,
                ButtonNumber = 1,
                Key = Key.LeftShift,
            };

            inputMappings[Inputs.L] = new InputMapping()
            {
                IsAxis = false,
                ButtonNumber = 5,
                Key = Key.LeftControl,
            };

            inputMappings[Inputs.R] = new InputMapping()
            {
                IsAxis = false,
                ButtonNumber = 6,
                Key = Key.RightControl,
            };

            inputMappings[Inputs.Esc] = new InputMapping()
            {
                IsAxis = false,
                ButtonNumber = 8,
                Key = Key.Escape,
            };
        }

        public static void UpdateInput()
        {
            if (inputState == null) InitInput();
            if (Engine.Joystick.ConnectedJoystickCount > 0)
            {
                foreach (Inputs item in Enum.GetValues(typeof(Inputs)))
                {
                    switch (InputButton(item))
                    {
                        case ButtonState.Push:
                            inputState[item] = 1;
                            break;
                        case ButtonState.Release:
                            inputState[item] = -1;
                            break;
                        case ButtonState.Hold:
                            inputState[item]++;
                            break;
                        case ButtonState.Free:
                            inputState[item] = 0;
                            break;
                    }
                }
            }
            else
            {
                foreach (Inputs item in Enum.GetValues(typeof(Inputs)))
                {
                    switch (GetButtonState(item))
                    {
                        case ButtonState.Push:
                            inputState[item] = 1;
                            break;
                        case ButtonState.Release:
                            inputState[item] = -1;
                            break;
                        case ButtonState.Hold:
                            inputState[item]++;
                            break;
                        case ButtonState.Free:
                            inputState[item] = 0;
                            break;
                    }
                }
            }
        }

        public static int GetInputState(Inputs inputs)
        {
            return inputState[inputs];
        }

        public static Func<Inputs, ButtonState> GetButtonState { get; set; } = DefaultFunc;

        static ButtonState DefaultFunc(Inputs inputs)
        {
            if (!inputMappings.ContainsKey(inputs)) return ButtonState.Free;
            return Engine.Keyboard.GetKeyState(inputMappings[inputs].Key);
        }

        static ButtonState InputButton(Inputs inputs)
        {
            if (!inputMappings.ContainsKey(inputs)) return ButtonState.Free;
            if (Engine.Keyboard.GetKeyState(inputMappings[inputs].Key) != ButtonState.Free) return Engine.Keyboard.GetKeyState(inputMappings[inputs].Key);
            if (Engine.Joystick.ConnectedJoystickCount == 0) return ButtonState.Free;
            if (inputMappings[inputs].IsAxis)
            {
                if (inputMappings[inputs].AxisType == InputMapping.AxisInputType.Down)
                {
                    if (Engine.Joystick.GetAxisState(1, inputMappings[inputs].AxisNumber) <= inputMappings[inputs].AxisThreshold)
                    {
                        if (GetInputState(inputs) > 0) return ButtonState.Hold;
                        if (GetInputState(inputs) < 1) return ButtonState.Push;
                    }
                    else
                    {
                        if (GetInputState(inputs) == 0) return ButtonState.Free;
                        if (GetInputState(inputs) > 0) return ButtonState.Release;
                    }
                }
                else if (inputMappings[inputs].AxisType == InputMapping.AxisInputType.Up)
                {
                    if (Engine.Joystick.GetAxisState(1, inputMappings[inputs].AxisNumber) >= inputMappings[inputs].AxisThreshold)
                    {
                        if (GetInputState(inputs) > 0) return ButtonState.Hold;
                        if (GetInputState(inputs) < 1) return ButtonState.Push;
                    }
                    else
                    {
                        if (GetInputState(inputs) == 0) return ButtonState.Free;
                        if (GetInputState(inputs) > 0) return ButtonState.Release;
                    }
                }
            }
            return Engine.Joystick.GetButtonState(1, inputMappings[inputs].ButtonNumber);
        }

        public static void SetConfig(KeyConfig keyConfig)
        {
            foreach (var item in keyConfig.InputMappings)
            {
                if (keyConfig.ControllerName == "KeyBoard") inputMappings[item.Key] = InputMapping.MergeMapping(keyConfig.InputMappings[item.Key], inputMappings[item.Key]);
                else inputMappings[item.Key] = InputMapping.MergeMapping(inputMappings[item.Key], keyConfig.InputMappings[item.Key]);
            }
        }
    }

    /// <summary>
    /// 入力リスト
    /// </summary>
    public enum Inputs
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        A = 4,
        B = 5,
        L = 6,
        R = 7,
        Esc = 8,
    }
}
