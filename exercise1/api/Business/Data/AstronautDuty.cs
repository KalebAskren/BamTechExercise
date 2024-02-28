using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StargateAPI.Business.Data
{
    [Table("AstronautDuty")]
    public class AstronautDuty
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int PersonId { get; set; }

        public string Rank { get; set; } = string.Empty;

        public string DutyTitle { get; set; } = string.Empty;

        public DateTime DutyStartDate { get; set; }

        public DateTime? DutyEndDate { get; set; }

        [JsonIgnore]
        public virtual Person Person { get; set; }
    }

    public class AstronautDutyConfiguration : IEntityTypeConfiguration<AstronautDuty>
    {
        public void Configure(EntityTypeBuilder<AstronautDuty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
