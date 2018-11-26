using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Mvvm.Properties;

namespace LVK.Mvvm.ViewModels
{
    [PublicAPI]
    public abstract partial class ViewModel : Disposable, IPropertyWriteListener
    {
        [NotNull]
        private readonly Dictionary<IProperty, string> _PropertyNames = new Dictionary<IProperty, string>(new ObjectReferenceEqualityComparer());
        
        protected ViewModel([NotNull] IMvvmContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.PropertyWriteScopeExited += ContextOnPropertyWriteScopeExited;
        }

        [NotNull]
        protected IMvvmContext Context { get; }

        [NotNull]
        private T RegisterProperty<T>([NotNull] T property, [CanBeNull] string name)
            where T: IProperty
        {
            if (!(name is null))
            {
                _PropertyNames[property] = name;
                RegisterDisposable(
                    new ActionDisposable(
                        () => Context.RegisterWriteListener(property, this), () => Context.UnregisterWriteListener(property, this)));
            }

            return property;
        }
    }
}