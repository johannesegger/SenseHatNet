using System;

namespace Sense.Stick
{
    public class JoystickEvent
    {
        public JoystickEvent(DateTime time, JoystickKey key, JoystickKeyState state)
        {
            Time = time;
            Key = key;
            State = state;
        }

        public DateTime Time { get; }
        public JoystickKey Key { get; }
        public JoystickKeyState State { get; }
    }
}