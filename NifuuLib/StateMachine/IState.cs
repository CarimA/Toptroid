using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.StateMachine
{
    public interface IState
    {
        void Update(GameTime gameTime);
        void Enter(params object[] args);
        void Exit();
    }
}
