using System.ComponentModel.DataAnnotations;

namespace GetRequiredSectionSample.Configurations
{
    public class SampleOptions
    {

        public const string ConfigurationName = "SampleSection";

        [Required]
        public string SampleProperty { get; set; }
    }
}
