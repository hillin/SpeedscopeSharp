using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hillinworks.Speedcore.FileFormat
{
    internal class SampledProfile : ProfileBase
    {
        public override ProfileType Type => ProfileType.Sampled;

        [JsonProperty("samples")]
        public List<List<double>> Samples { get; }

        [JsonProperty("weights")]
        public List<double> Weights { get; }

        [JsonConstructor]
        public SampledProfile(
            string name,
            double startValue,
            double endValue,
            ValueUnit unit,
            List<List<double>> samples,
            List<double> weights)
            : base(name, startValue, endValue, unit)
        {
            Samples = samples;
            Weights = weights;
        }

        public SampledProfile(
            string name,
            double startValue,
            double endValue,
            ValueUnit unit)
            : this(
                name,
                startValue,
                endValue,
                unit,
                new List<List<double>>(),
                new List<double>())
        {

        }
    }
}