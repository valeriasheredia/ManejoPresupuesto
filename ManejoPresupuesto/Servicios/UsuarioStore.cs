using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Identity;

namespace ManejoPresupuesto.Servicios
{
    public class UsuarioStore : 
        IUserStore<Usuario>, 
        IUserEmailStore<Usuario>,
        IUserPasswordStore<Usuario>

    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        //CONSTRUCTOR
        public UsuarioStore(IRepositorioUsuarios repositorioUsuarios)
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }

        //CREAR USUARIO
        public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
        {
            user.Id = await repositorioUsuarios.CrearUsuario(user);
            return IdentityResult.Success;
        }
        public Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            
        }

        //BUSCAR USUARIO POR EMAIL
        public async Task<Usuario> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await repositorioUsuarios.BuscarUsuarioPorEmail(normalizedEmail);
        }
        public Task<Usuario> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //BUSCAR USUARIO POR EMAIL (NOMBRE = EMAIL)
        public async Task<Usuario> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await repositorioUsuarios.BuscarUsuarioPorEmail(normalizedUserName);
        }

        //OBTIENE EL EMAIL DEL USUARIO
        public Task<string> GetEmailAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }
        public Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        //RETORNA PASSWORD
        public Task<string> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        //RETORNA ID DEL USUARIO
        public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        //RETORNA NOMBRE DE USUARIO (EN ESTE CASO EMAIL)
        public Task<string> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }
        public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task SetEmailAsync(Usuario user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        //EMAIL NORMALIZADO
        public Task SetNormalizedEmailAsync(Usuario user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.EmailNormalizado = normalizedEmail;
            return Task.CompletedTask;
        }

        //RETORNA EMAIL NORMALIZADO
        public Task SetNormalizedUserNameAsync(Usuario user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        public Task SetPasswordHashAsync(Usuario user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
        public Task SetUserNameAsync(Usuario user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
