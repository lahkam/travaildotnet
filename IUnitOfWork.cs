namespace isgasoir
{
    public interface IUnitOfWork:IDisposable
    {
        IBaseRepository<Studant,long> studantRepository { get; }
        IBaseRepository<Product,long> productRepository { get; }
        IBaseRepository<Semestre, long> semestreRepository { get; }
        IBaseRepository<Module, long> moduleRepository { get; }
        IBaseRepository<Filiere, long> filiereRepository { get; }
        IBaseRepository<Chapitre, long> chapitreRepository { get; }
        IBaseRepository<Activity, long> activityRepository { get; }
        int complete();
    }
}
