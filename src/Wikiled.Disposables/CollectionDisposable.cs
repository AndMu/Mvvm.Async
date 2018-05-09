﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Wikiled.Disposables
{
    /// <summary>
    /// Disposes a collection of disposables.
    /// </summary>
    public sealed class CollectionDisposable : SingleDisposable<ImmutableQueue<IDisposable>>
    {
        /// <summary>
        /// Creates a disposable that disposes a collection of disposables.
        /// </summary>
        /// <param name="disposables">The disposables to dispose.</param>
        public CollectionDisposable(params IDisposable[] disposables)
            : this((IEnumerable<IDisposable>)disposables)
        {
        }

        /// <summary>
        /// Creates a disposable that disposes a collection of disposables.
        /// </summary>
        /// <param name="disposables">The disposables to dispose.</param>
        public CollectionDisposable(IEnumerable<IDisposable> disposables)
            : base(ImmutableQueue.CreateRange(disposables))
        {
        }

        /// <inheritdoc />
        protected override void Dispose(ImmutableQueue<IDisposable> context)
        {
            foreach (var disposable in context)
                disposable.Dispose();
        }

        /// <summary>
        /// Adds a disposable to the collection of disposables. If this instance is already disposed, then <paramref name="disposable"/> is disposed immediately.
        /// </summary>
        /// <param name="disposable">The disposable to add to our collection.</param>
        public void Add(IDisposable disposable)
        {
            if (!TryUpdateContext(x => x.Enqueue(disposable)))
                disposable.Dispose();
        }

        /// <summary>
        /// Creates a disposable that disposes a collection of disposables.
        /// </summary>
        /// <param name="disposables">The disposables to dispose.</param>
        public static CollectionDisposable Create(params IDisposable[] disposables) => new CollectionDisposable(disposables);

        /// <summary>
        /// Creates a disposable that disposes a collection of disposables.
        /// </summary>
        /// <param name="disposables">The disposables to dispose.</param>
        public static CollectionDisposable Create(IEnumerable<IDisposable> disposables) => new CollectionDisposable(disposables);
    }
}
