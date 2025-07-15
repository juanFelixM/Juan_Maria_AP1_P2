using Juan_Maria_AP1_P2.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Juan_Maria_AP1_P2.Models;

namespace Juan_Maria_AP1_P2.Services
{
    public class EntradaServices
    {
        private readonly IDbContextFactory<Contexto> DbFactory;

        public EntradaServices(IDbContextFactory<Contexto> dbFactory)
        {
            DbFactory = dbFactory;
        }

        public async Task<List<Producto>> ListarProductos()
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Productos.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Existe(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Entradas.AnyAsync(e => e.EntradasId == id);
        }

        private async Task<bool> Insertar(Entradas entrada)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            contexto.Entradas.Add(entrada);

            foreach (var detalle in entrada.EntradasDetalle)
            {
                if (detalle.ProductoId <= 0)
                    throw new InvalidOperationException($"ProductoId {detalle.ProductoId} no es válido para el detalle.");
            }

            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task<bool> Modificar(Entradas entrada)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            contexto.Entry(entrada).State = EntityState.Modified;

            var detallesExistentes = contexto.EntradasDetalle.Where(ed => ed.EntradasId == entrada.EntradasId);
            contexto.EntradasDetalle.RemoveRange(detallesExistentes);

            foreach (var detalle in entrada.EntradasDetalle)
            {
                if (detalle.ProductoId <= 0)
                    throw new InvalidOperationException($"ProductoId {detalle.ProductoId} no es válido para el detalle.");
                contexto.EntradasDetalle.Add(detalle);
            }

            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Guardar(Entradas entrada)
        {
            if (!await Existe(entrada.EntradasId))
                return await Insertar(entrada);
            else
                return await Modificar(entrada);
        }

        public async Task<bool> Eliminar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var eliminado = await contexto.Entradas
                .Where(e => e.EntradasId == id)
                .ExecuteDeleteAsync();

            return eliminado > 0;
        }

        public async Task<Entradas?> Buscar(int id)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Entradas
                .Include(e => e.EntradasDetalle)
                    .ThenInclude(ed => ed.Producto)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EntradasId == id);
        }

        public async Task<List<Entradas>> Listar(Expression<Func<Entradas, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Entradas
                .Include(e => e.EntradasDetalle)
                    .ThenInclude(ed => ed.Producto)
                .AsNoTracking()
                .Where(criterio)
                .ToListAsync();
        }
    }
} 