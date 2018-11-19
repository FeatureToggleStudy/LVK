using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Mvvm;
using LVK.Mvvm.Properties;
using LVK.Mvvm.ViewModels;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly IMvvmContext _MvvmContext;

        public ApplicationEntryPoint([NotNull] IMvvmContext mvvmContext)
        {
            _MvvmContext = mvvmContext ?? throw new ArgumentNullException(nameof(mvvmContext));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            await Task.Yield();

            var vm = new TestViewModel(_MvvmContext);
            vm.PropertyChanged += (s, e) => Console.WriteLine($"PropertyChanged: {e?.PropertyName}");

            vm.A = 10;
            vm.B = 32;

            Console.WriteLine(vm);

            vm.B = 40;
            Console.WriteLine(vm);
            
            return 0;
        }
    }

    internal class TestViewModel : ViewModel
    {
        [NotNull]
        private readonly IProperty<int> _A;
        [NotNull]
        private readonly IProperty<int> _B;
        [NotNull]
        private readonly IReadableProperty<int> _C;
 
        public TestViewModel([NotNull] IMvvmContext context)
            : base(context)
        {
            _A = Property(nameof(A), 0);
            _B = Property(nameof(B), 0);
            _C = Computed(nameof(C), () => A + B);
        }

        public int A
        {
            get => _A.Value;
            set => _A.Value = value;
        }

        public int B
        {
            get => _B.Value;
            set => _B.Value = value;
        }
    
        public int C => _C.Value;

        public override string ToString() => $"A: {A}, B: {B}, C: {C}";
    }
}