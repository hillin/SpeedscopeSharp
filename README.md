# SpeedscopeSharp
SpeedscopeSharp provides a profiling context for you to easily create custom profiling events in your code, 
and output a profiling file which can be viewed with https://www.speedscope.app/.

## Getting Started

### Installation
Install from nuget: 
```
Install-Package Hillinworks.Speedscope
```
or
```
dotnet add package Hillinworks.Speedscope
```

### Profile your code

1. Create a `ProfilingContext`:

``` 
var profileContext = new ProfileContext("MyProfilingContext");
```

2. Create a sub-context. Currently the only supported sub-context type is `EventedProfilingContext`.

A sub-context is used to profile a single thread of process, i.e. the events within it cannot interleave. 
If you want to profiling a multi-threaded process, use one sub-context per thread is a good idea.

You can create an `EventedProfilingContext` like this:

```
var eventedProfileContext = profileContext.CreateEventedProfileContext("MyEventedProfilingContext", TimeUnit.Seconds);
```

The created `EventedProfilingContext` can be used to record *events*. An event is simply a named time frame, you can wrap
any kind of operation within an event: function calls, blocking operations, async awaits....

3. Create an event

```
var eventHandle = eventedProfileContext.OpenEvent("MyOperation");
// do some time-consuming stuffs
eventedProfileContext.CloseEvent(eventHandle);
```

This will create an event in the `EventedProfilingContext`, with the time consumed by the wrapped operations recorded.

An event handle is `IDisposable`, so you can also write it like this:

```
using(var eventHandle = eventedProfileContext.OpenEvent("MyOperation"))
{
    // do some time-consuming stuffs
}
```

An opened event must be closed, by either explicitly calling `CloseEvent`, or disposing the event handle.

Events can be nested:

```
using(var eventHandle = eventedProfileContext.OpenEvent("MyOperation"))
{
    using(var step1 = eventedProfileContext.OpenEvent("Step 1"))
    {
        // do step 1
    }
    using(var step2 = eventedProfileContext.OpenEvent("Step 2"))
    {
        // do step 2
    }
    // do remaining stuffs
}
```

4. Commit the `ProfilingContext`

Once the profiling is completed, call

```
profileContext.Commit();
```

to commit the `ProfileContext`. Once it's committed, you can not create sub-context or events on that `ProfileContext` any more.

Then you can save the profling result to a file:

```
using (var file = File.Create(pathToSaveProflingFile))
{
    profileContext.Save(file);
}
```

The saved file can be loaded and visualized at https://www.speedscope.app/.
