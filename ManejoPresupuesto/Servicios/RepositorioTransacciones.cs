using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<ResultadoObtenerPorMes>> ObtenerPorMes(int usuarioId, int año);
        Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(ParametroObtenerTransaccionesPorUsuario modelo);
        Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo);
    }
    public class RepositorioTransacciones: IRepositorioTransacciones
    {
        private readonly string connectionStrings;
        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Transaccion transaccion)
        {
            using var connection = new SqlConnection(connectionStrings);
            var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar",
                new
                {
                    transaccion.UsuarioId,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota
                }, 
                commandType: System.Data.CommandType.StoredProcedure);
            transaccion.Id = id;
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var connection = new SqlConnection(connectionStrings);
            return await connection.QueryAsync<Transaccion>(
                @"select t.Id, t.Monto, t.FechaTransaccion,c.Nombre as Categoria,
                cu.Nombre as Cuenta, c.TipoOperacionId
                from Transacciones t
                inner join Categorias c
                on c.id = t.CategoriaId
                inner join Cuentas cu
                on cu.id = t.CuentaId
                where t.CuentaId = @CuentaId and t.UsuarioId = @UsuarioId
                and FechaTransaccion between @FechaInicio and @FechaFin", modelo);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectionStrings);
            return await connection.QueryAsync<Transaccion>(
                @"select t.Id, t.Monto, t.FechaTransaccion,c.Nombre as Categoria,
                cu.Nombre as Cuenta, c.TipoOperacionId, Nota
                from Transacciones t
                inner join Categorias c
                on c.id = t.CategoriaId  
                inner join Cuentas cu
                on cu.id = t.CuentaId
                where t.UsuarioId = @UsuarioId
                and FechaTransaccion between @FechaInicio and @FechaFin
                order by t.FechaTransaccion desc", modelo);
        }

        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior,
            int cuentaAnteriorId)
        {
            using var connection = new SqlConnection(connectionStrings);
            await connection.ExecuteAsync("Transacciones_Actualizar",
                new
                {
                    transaccion.Id,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota,
                    montoAnterior,
                    cuentaAnteriorId
                }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Transaccion> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionStrings);
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(
                                    @"select Transacciones.*, cat.TipoOperacionId
                                    from Transacciones
                                    inner join Categorias cat
                                    on cat.Id=Transacciones.CategoriaId
                                    where Transacciones.Id = @Id and Transacciones.UsuarioId = @UsuarioId", 
                                    new { id, usuarioId });
        }

        public async Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(
            ParametroObtenerTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectionStrings);
            return await connection.QueryAsync<ResultadoObtenerPorSemana>(@"
                                            select datediff(d, @fechaInicio, FechaTransaccion) /7 +1 as Semana,
                                            sum(Monto) as Monto, cat.TipoOperacionId
                                            from Transacciones
                                            inner join Categorias cat
                                            on cat.Id = Transacciones.CategoriaId
                                            where transacciones.UsuarioId = @usuarioId and
                                            FechaTransaccion between @fechaInicio and @fechaFin
                                            group by datediff(d, @fechaInicio, FechaTransaccion) /7, cat.TipoOperacionId", modelo);
        }

        public async Task<IEnumerable<ResultadoObtenerPorMes>> ObtenerPorMes(int usuarioId, int año)
        {
            using var connection = new SqlConnection(connectionStrings);
            return await connection.QueryAsync<ResultadoObtenerPorMes>(@"
                            select MONTH(FechaTransaccion) as Mes,
                            sum(monto) as Monto, cat.TipoOperacionId
                            from Transacciones
                            inner join Categorias cat
                            on cat.id = Transacciones.CategoriaId
                            where Transacciones.UsuarioId = @usuarioId and year(FechaTransaccion) = @Año
                            group by MONTH(FechaTransaccion), cat.TipoOperacionId", new {usuarioId, año});
        }
        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionStrings);
            await connection.ExecuteAsync("Transacciones_Borrar",
                new { id }, commandType: System.Data.CommandType.StoredProcedure);
            
        }
    }
}
