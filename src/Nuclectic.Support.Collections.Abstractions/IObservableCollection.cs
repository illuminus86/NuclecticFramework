﻿#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2013 Nuclex Development Labs

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

using System;

namespace Nuclectic.Support.Collections {

  /// <summary>Interface for collections that can be observed</summary>
  /// <typeparam name="TItem">Type of items managed in the collection</typeparam>
  public interface IObservableCollection<TItem> {

    /// <summary>Raised when an item has been added to the collection</summary>
    event EventHandler<ItemEventArgs<TItem>> ItemAdded;

    /// <summary>Raised when an item is removed from the collection</summary>
    event EventHandler<ItemEventArgs<TItem>> ItemRemoved;

    /// <summary>Raised when an item is replaced in the collection</summary>
    event EventHandler<ItemReplaceEventArgs<TItem>> ItemReplaced;

    /// <summary>Raised when the collection is about to be cleared</summary>
    /// <remarks>
    ///   This could be covered by calling ItemRemoved for each item currently
    ///   contained in the collection, but it is often simpler and more efficient
    ///   to process the clearing of the entire collection as a special operation.
    /// </remarks>
    event EventHandler Clearing;

    /// <summary>Raised when the collection has been cleared of its items</summary>
    event EventHandler Cleared;

  }

} // namespace Nuclex.Support.Collections
