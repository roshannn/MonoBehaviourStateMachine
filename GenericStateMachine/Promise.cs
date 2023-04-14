using System;
using System.Collections.Generic;

public class Promise<T> :IPromise<T> 
{

    private Action<T> resolveCallback;
    private Action<Exception> rejectCallback;

    public void Resolve(T result) 
    {
        resolveCallback?.Invoke(result);
    }

    public void Reject(Exception ex) 
    {
        rejectCallback?.Invoke(ex);
    }

    public Promise<T> Then(Action<T> onResolved) 
    {
        resolveCallback += onResolved;
        return this;
    }

    public Promise<T> Catch(Action<Exception> onRejected) 
    {
        rejectCallback += onRejected;
        return this;
    }
}
