using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using DiffLib;

using JetBrains.Annotations;

namespace LVK.Mvvm.Properties.ListProperties
{
    [PublicAPI]
    public class ObservableCollectionProperty<T> : ObservableCollection<T>, IProperty
    {
        [NotNull]
        private readonly IMvvmContext _Context;

        [NotNull]
        private readonly IReadableProperty<IEnumerable<T>> _SourceCollectionProperty;

        public ObservableCollectionProperty([NotNull] IMvvmContext context, [NotNull] IReadableProperty<IEnumerable<T>> sourceCollectionProperty)
        {
            _Context = context ?? throw new ArgumentNullException(nameof(context));
            _SourceCollectionProperty = sourceCollectionProperty ?? throw new ArgumentNullException(nameof(sourceCollectionProperty));

            Update();
        }

        private void Update()
        {
            List<T> targetState = (_SourceCollectionProperty.Value ?? Enumerable.Empty<T>()).ToList();
            this.MutateToBeLike(targetState);
        }
    }
}