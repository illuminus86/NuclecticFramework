﻿#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/
#endregion

namespace Nuclectic.Input.Devices {
    /// <summary>
  ///   Sends out notifications for intercepted window messages related to the keyboard
  /// </summary>
  public interface IKeyboardMessageSource {

    /// <summary>Triggered when a key has been pressed down</summary>
    event KeyboardKeyEventDelegate KeyPressed;

    /// <summary>Triggered when a key has been released again</summary>
    event KeyboardKeyEventDelegate KeyReleased;

    /// <summary>Triggered when the user has entered a character</summary>
    event KeyboardCharacterEventDelegate CharacterEntered;

  }

} // namespace Nuclex.Input.Devices

