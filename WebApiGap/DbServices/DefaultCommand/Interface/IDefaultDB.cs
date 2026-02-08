namespace API_GAI.DbServices.DefaultCommand.Interface
{
    public interface IDefaultDB<Tentity> where Tentity : class
    {
        Task<Tentity> AddAsync(Tentity t);

        Task<Tentity> UpdateAsync(Tentity t);

        Task<List<Tentity>> GetAllAsync();
    }
}
