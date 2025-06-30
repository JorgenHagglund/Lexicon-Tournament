namespace Tournament.Core.Repositories;

public interface IUnitOfWork
{
    ITournamentRepository TournamentRepository { get; }
    IGameRepository GameRepository { get; }
    
    Task<int> CompleteAsync();
//    void Dispose();
}
