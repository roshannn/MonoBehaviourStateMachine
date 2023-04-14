using System;

public interface IPromise<T>
{

   
    public void Resolve(T result)
    {
      
    }

    public void Reject(Exception ex)
    {
        
    }

}
