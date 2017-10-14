using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Camera
{
    public interface ICamera
    {
        Matrix Transform { get; }
        float Rotation { get; set; }
        Vector2 Origin { get; set; }
        float Zoom { get; set; }

        IFocusable Focus { get; set; }

        bool IsInView(Rectangle rectangle);
    }
}
