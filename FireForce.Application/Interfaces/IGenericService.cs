namespace FireForce.Application.Interfaces
{
    public interface IGenericService<TDto> where TDto : class
    {
        Task<TDto?> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<int> CreateAsync(TDto dto, string currentUser);
        Task<bool> UpdateAsync(TDto dto, string currentUser);
        Task<bool> DeleteAsync(int id, string currentUser);
    }
}