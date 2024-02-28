using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("Error")]
    public class Error
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
    public class ErrorConfiguration : IEntityTypeConfiguration<Error>
    {
        public void Configure(EntityTypeBuilder<Error> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
