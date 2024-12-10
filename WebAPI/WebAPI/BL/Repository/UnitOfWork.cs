using Microsoft.AspNetCore.Identity;
using WebAPI.BL.Interface;
using WebAPI.DAL.Database;

namespace WebAPI.BL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dc;
        IConfiguration configuration;
        private readonly UserManager<IdentityUser> _userManager;
        public UnitOfWork(DataContext dc, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.dc = dc;
            this._userManager = userManager;
            this.configuration = configuration;
        }
        public IJobRep JobRep => 
            new JobRep(dc);       

        public IApplicationRep ApplicationRep => new ApplicationRep(dc);

        public IJobDetailsRep DetailsRep => new JobDetailsRep(dc);

        public IJWTService JWTService => new JWTService(_userManager, configuration);

        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;
        }
    }
}
