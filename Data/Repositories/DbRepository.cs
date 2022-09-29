using Core.Domain;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{

    public class DbRepository : IDbRepository<Certificate>
    {
        private readonly AppDbContext _db;

        public DbRepository(AppDbContext db)
        {
            _db = db;
        }

        public Certificate AddNew(Certificate certificate)
        {
            _db.Add(certificate);
            return certificate;
        }


        public Task<int> CommitAsync() => _db.SaveChangesAsync();


        public Task<List<Certificate>> GetAllCertificates()
        {
            return _db.Certificates.Include(c => c.Customer).OrderBy(n => n.Number).ToListAsync();
        }       
    }
}
