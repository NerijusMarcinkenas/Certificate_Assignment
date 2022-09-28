namespace DataAccess.Repositories
{
    public interface IDbRepository<TCertificate> where TCertificate : class
    {
        public Task<List<TCertificate>> GetAllCertificates();
        public TCertificate AddNew(TCertificate certificate);
        public Task<int> CommitAsync();
    }
}
