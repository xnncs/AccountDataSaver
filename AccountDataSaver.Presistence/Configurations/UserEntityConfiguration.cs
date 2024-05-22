using AccountDataSaver.Presistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountDataSaver.Presistence.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.Login).IsRequired();
        builder.Property(x => x.Name).IsRequired();

        builder.HasMany(user => user.Accounts)
            .WithOne(account => account.Author)
            .HasForeignKey(account => account.AuthorId);
    }
}