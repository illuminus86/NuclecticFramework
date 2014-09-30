﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuclectic.Input.Abstractions;
using Nuclectic.Input.Abstractions.Devices;
using Nuclex.Input.Devices;

namespace PortableGameTest.Framework.Input
{
    internal class NoDirectInputManager
        : IDirectInputManager
    {
        public bool IsDirectInputAvailable { get { return false; } }
        public IGamePad[] CreateGamePads()
        {
            throw new NotSupportedException();
        }
    }
}
