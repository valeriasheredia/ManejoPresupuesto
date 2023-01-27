﻿using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }


        //CREAR USUARIO
        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var usuarioId = await connection.QuerySingleAsync<int>(@"
                            insert into Usuarios ( Email, EmailNormalizado, PasswordHash)
                            values (@Email, @EmailNormalizado, @PasswordHash);
                            SELECT SCOPE_IDENTITY();
                            ", usuario);
            await connection.ExecuteAsync("CrearDatosUsuarioNuevo",
                new { usuarioId },
                commandType: System.Data.CommandType.StoredProcedure);
            return usuarioId;
        }

        //BUSCAR USUARIO POR EMAIL
        public async Task<Usuario> BuscarUsuarioPorEmail(string emailNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(
                            "select * from Usuarios where EmailNormalizado = @emailNormalizado", 
                            new { emailNormalizado });
        }
    }
}
