using System;
using System.Threading;

namespace Hillinworks.Speedscope
{
    public abstract class FreezableObject
    {
        protected bool IsFrozen { get; set; }

        private ReaderWriterLockSlim Lock { get; }
            = new ReaderWriterLockSlim();

        protected void CheckFrozen()
        {
            if (!this.IsFrozen)
            {
                throw new InvalidOperationException("this context is not frozen");
            }
        }

        protected void CheckNotFrozen()
        {
            if (this.IsFrozen)
            {
                throw new InvalidOperationException("this context is frozen");
            }
        }

        protected void EnsureNotFrozen(Action action)
        {
            this.CheckNotFrozen();

            this.Lock.EnterReadLock();
            try
            {
                this.CheckNotFrozen();

                action();
            }
            finally
            {
                this.Lock.ExitReadLock();
            }
        }

        protected T EnsureNotFrozen<T>(Func<T> func)
        {
            this.CheckNotFrozen();

            this.Lock.EnterReadLock();
            try
            {
                this.CheckNotFrozen();

                return func();
            }
            finally
            {
                this.Lock.ExitReadLock();
            }
        }

        internal virtual void Freeze()
        {
            this.CheckNotFrozen();

            this.Lock.EnterWriteLock();
            try
            {
                this.CheckNotFrozen();
                this.IsFrozen = true;
            }
            finally
            {
                this.Lock.ExitWriteLock();
            }
        }
    }
}