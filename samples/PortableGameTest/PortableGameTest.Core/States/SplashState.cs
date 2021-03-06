﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.OwnedInstances;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Game.State;
using PortableGameTest.Core.Graphics;
using PortableGameTest.Core.States;
using PortableGameTest.Framework.Game.States;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core
{
    public class SplashState
        : GameState
    {
	    private readonly IAutoGameStateService _GameStateService;

	    public SplashState(IAutoGameStateService gameStateService)
	    {
		    _GameStateService = gameStateService;
	    }

	    public override void Update(GameTime gameTime)
        {
	        _GameStateService.Push<KeyboardLogState>(GameStateModality.Exclusive);
        }
    }
}
