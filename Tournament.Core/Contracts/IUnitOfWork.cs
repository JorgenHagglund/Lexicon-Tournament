namespace Tournament.Core.Contracts;

public interface IUnitOfWork
{
    ITournamentRepository TournamentRepository { get; }
    IGameRepository GameRepository { get; }
    
    Task<int> CompleteAsync();
//    void Dispose();
}
