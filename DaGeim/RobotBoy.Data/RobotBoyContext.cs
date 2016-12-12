namespace RobotBoy.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using RobotBoy.Entities.Player;
    public class RobotBoyContext : DbContext
    {
        // Your context has been configured to use a 'RobotBoyContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'RobotBoy.Data.RobotBoyContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'RobotBoyContext' 
        // connection string in the application configuration file.
        public RobotBoyContext()
            : base("name=RobotBoyContext")
        {
        }

        public DbSet<Player> Players { get; set; }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}