using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hillinworks.Speedscope.FileFormat;
using Newtonsoft.Json;

namespace Hillinworks.Speedscope
{
    public class ProfileContext : FreezableObject
    {
        public ProfileContext(string? name = null)
        {
            this.Name = name;
            this.BaseTime = DateTime.UtcNow;
        }

        public string? Name { get; set; }
        public string? Exporter { get; set; }

        private object FrameSyncObject { get; } = new object();

        private List<Frame> Frames { get; }
            = new List<Frame>();

        private Dictionary<Frame, int> FrameLookup { get; }
            = new Dictionary<Frame, int>();

        private ConcurrentBag<SubProfileContextBase> SubProfileContexts { get; }
            = new ConcurrentBag<SubProfileContextBase>();

        internal DateTime BaseTime { get; }

        internal int TryAddFrame(Frame frame)
        {
            return this.EnsureNotFrozen(() =>
            {
                lock (this.FrameSyncObject)
                {
                    if (this.FrameLookup.TryGetValue(frame, out var index))
                    {
                        return index;
                    }

                    index = this.Frames.Count;
                    this.Frames.Add(frame);
                    this.FrameLookup.Add(frame, index);

                    return index;
                }
            });
        }

        public EventedProfileContext CreateEventedProfileContext(string name, TimeUnit timeUnit)
        {
            var context = new EventedProfileContext(this, name, timeUnit);
            this.SubProfileContexts.Add(context);
            return context;
        }

        internal override void Freeze()
        {
            foreach (var context in this.SubProfileContexts)
            {
                context.Freeze();
            }

            base.Freeze();
        }

        public void Commit()
        {
            this.Freeze();
        }

        public void Save(Stream stream)
        {
            var profile = new Profile
            {
                Name = this.Name,
                Exporter = this.Exporter
            };

            profile.Shared.Frames.AddRange(this.Frames);
            profile.Profiles.AddRange(
                this.SubProfileContexts
                    .Where(c => !c.IsEmpty)
                    .Select(c => c.CreateProfile())
                    .OrderBy(c => c.StartValue));

            using var writer = new StreamWriter(stream);

            writer.Write(
                JsonConvert.SerializeObject(
                    profile,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }));
        }
    }
}