using Core.Domain;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Repositories
{

    public class DbRepository : IDbRepository<Certificate>
    {
        private readonly AppDbContext _db;

        public DbRepository(AppDbContext db)
        {
            _db = db;
            FeedCertificatesAsync();
        }


        public Certificate AddNew(Certificate certificate)
        {
            _db.Add(certificate);
            return certificate;
        }

        public Task<int> CommitAsync() => _db.SaveChangesAsync();

        public Task<List<Certificate>> GetAllCertificates()
        {
            return _db.Certificates.Include(c => c.Customer).OrderByDescending(n => n.Number).ToListAsync();
        }

        private void FeedCertificatesAsync()
        {
            _db.Add(new Certificate()
            {
                Number = "1",
                CreationDate = DateTime.UtcNow,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddYears(1),
                CertificateSum = 200,
                InsuredItem = "Apple Iphone 14 PRO",
                InsuredSum = 999,
                Customer = new Customer()
                {
                    Name = "Customer 1",
                    DateOfBirth = new DateTime(2000, 1, 1)
                }
            });

            _db.SaveChanges();
        }
    }
}
