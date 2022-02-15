using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Fluent
{
    public class Fluent<T> : IFluentState
    {
        public T Value { get; }
        public FluentState State { get;  }

        public static Fluent<T> Create(T value)
        {
            return new Fluent<T>(value, new FluentState());
        }

        public Fluent<TOther> Create<TOther>(TOther other)
        {
            return new Fluent<TOther>(other, State); //Q: clone state? pure function 
        }
        
        public Fluent(T value, FluentState state)
        {
            Value = value;
            State = state;
        }
        
        public void Deconstruct(out T Value, out FluentState State)
        {
            Value = this.Value;
            State = this.State;
        }
    }

    public class FluentState : IDisposable
    {
        //task: check behavior if it should be reversed like in golang
        private readonly Stack<IDisposable> _disposables = new(); 

        public void AddDisposable(IDisposable disposable)
        {
            if (disposable is FluentState)
            {
                throw new Exception($"Type '{disposable.GetType().FullName}' cannot be added.");
            }

            _disposables.Push(disposable);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        public Fluent<TOther> Create<TOther>(TOther other)
        {
            return new Fluent<TOther>(other, this);
        }
    }
}