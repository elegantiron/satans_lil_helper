using Microsoft.Xna.Framework.Input;

namespace SatansLilHelper.Interfaces;

internal interface IInputHandler : IDrawable
{
    IInputHandler HandleKey(Keys key);
}
