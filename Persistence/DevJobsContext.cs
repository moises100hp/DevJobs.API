using DevJobs.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevJobs.API.Persistence
{
    public class IJobvacancyRepository : DbContext
    {
        public IJobvacancyRepository(DbContextOptions<IJobvacancyRepository> options) : base(options)
        {
            
        }

        public DbSet<JobVacancy> JobVacancies { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<JobVacancy>(e =>
            {
                e.HasKey(jv => jv.Id);
                //e.ToTable("tb_JobVacancies");

                e.HasMany(jv => jv.Applications)
                .WithOne()
                .HasForeignKey(ja => ja.IdJobVacancy)
                .OnDelete(DeleteBehavior.Restrict); //Restrict exclusão lógica
            });

            builder.Entity<JobApplication>(e =>
            {
                e.HasKey(ja => ja.Id);
            });
        }
    }
}
