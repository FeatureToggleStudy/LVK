using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Mvvm.Properties
{
    internal class Property<T> : MvvmBound, IProperty<T>
    {
        private T _Value;
        
        public Property([NotNull] IMvvmContext context, T defaultValue)
            : base(context)
        {
            _Value = defaultValue;
        }

        public T Value
        {
            get
            {
                Context.RegisterRead(this);
                return _Value;
            }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_Value, value))
                    return;

                _Value = value;
                Context.RegisterWrite(this);
            }
        }

        public T PeekValue => _Value;
    }
}