namespace isgasoir
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationContext context;

        public IBaseRepository<Studant, long> studantRepository { get; private set; }

        public IBaseRepository<Product, long> productRepository { get; private set; }
        public IBaseRepository<Module, long> moduleRepository { get; private set; }
        public IBaseRepository<Semestre, long> semestreRepository { get; private set; }
        public IBaseRepository<Filiere, long> filiereRepository { get; private set; }
        public IBaseRepository<Chapitre, long> chapitreRepository { get; private set; }
        public IBaseRepository<Activity, long> activityRepository { get; private set; }

        public UnitOfWork(ApplicationContext context)
        {
            this.context = context;
           
            studantRepository= new BaseRepository<Studant,long>(context);
            productRepository = new BaseRepository<Product,long>(context);
            moduleRepository = new BaseRepository<Module, long>(context);
            semestreRepository = new BaseRepository<Semestre, long>(context);
            filiereRepository = new BaseRepository<Filiere, long>(context);
            chapitreRepository = new BaseRepository<Chapitre, long>(context);
            activityRepository = new BaseRepository<Activity, long>(context);




        }

       

        public int complete()
        {
            return context.SaveChanges();
        }

        public  void Dispose()
        {
            context.Dispose();
        }
    }
}
