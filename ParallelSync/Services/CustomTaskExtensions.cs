using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ParallelSync.Services;

public static class CustomTaskExtensions
{
    public static Task<T[]> GetAwaiter<T>(this (Task<T>, Task<T>, Task<T>, Task<T>) tasksTuple)
    {
        return Task.WhenAll(tasksTuple.Item1, tasksTuple.Item2, tasksTuple.Item3, tasksTuple.Item4);
    }
    
    public static Task<T[]> GetAwaiter<T>(params Task<T>[] tasks)
    {
        return Task.WhenAll(tasks);
    }
}