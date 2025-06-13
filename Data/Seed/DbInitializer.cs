using TaskManagementSystem.Models;

namespace TaskManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Garante que o banco existe
            context.Database.EnsureCreated();

            // Verifica se já existem funcionários
            if (context.Users.Any())
                return;

            // Dados de exemplo
            // var workers = new Worker[]
            // {
            //     new Worker { Name = "João Silva", Position = "Analista", Gender = 'M' },
            //     new Worker { Name = "Maria Oliveira", Position = "Desenvolvedora", Gender = 'F' },
            //     new Worker { Name = "Carlos Souza", Position = "Gerente", Gender = 'M' }
            // };
            Worker AdminWorker = new Worker { Name = "John Doe", Position = "Administrador", Gender = 'M' };
            context.Workers.Add(AdminWorker);
            context.SaveChanges();

            var users = new User[] {
                new User { WorkerId = AdminWorker.Id, Role = "admin", Password = "admin",  }
            };

            // context.Workers.AddRange(workers);
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
