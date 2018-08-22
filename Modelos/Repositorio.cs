using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MetodosDeExtension;

namespace Modelos
{
    //Para el manejo de los errores que lance el repositorio
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);

    public class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : class
    {
        public event ExceptionEventHandler Excepcion;
        DbContext Context;

        public Repositorio()
        {
            Context = new CRUD_MVCEntities();
        }

        //Para la inyección de cualquier contexto usando el mismo repositorio
        public Repositorio(DbContext context)
        {
            Context = context;
        }

        private DbSet<TEntity> EntitySet { get { return Context.Set<TEntity>(); } }

        public TEntity Create(TEntity toCreate)
        {
            TEntity Result = null;
            try
            {
                Result = EntitySet.Add(toCreate);
                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite, EntityValidationErrors = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite });
                this.EscribirEnArchivoLog(ex);
            }
            return Result;
        }

        public bool Delete(TEntity toDelete)
        {
            bool Result = false;
            try
            {
                EntitySet.Attach(toDelete);
                EntitySet.Remove(toDelete);
                Result = Context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite, EntityValidationErrors = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite });
                this.EscribirEnArchivoLog(ex);
            }
            return Result;
        }

        public bool Update(TEntity toUpdate)
        {
            bool Result = false;
            try
            {
                EntitySet.Attach(toUpdate);
                Context.Entry<TEntity>(toUpdate).State = EntityState.Modified;
                Result = Context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite, EntityValidationErrors = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite });
                this.EscribirEnArchivoLog(ex);
            }
            return Result;
        }

        public bool Update(Expression<Func<TEntity, bool>> criterio, string propertyName, object valor)
        {
            bool Result = false;
            try
            {
                Context.Entry<TEntity>(EntitySet.FirstOrDefault(criterio)).Property(propertyName).CurrentValue = valor;
                Result = Context.SaveChanges() > 0;
            }
            catch (DbEntityValidationException ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite, EntityValidationErrors = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite });
                this.EscribirEnArchivoLog(ex);
            }
            return Result;
        }

        public TEntity Retrieve(Expression<Func<TEntity, bool>> criterio)
        {
            TEntity Result = null;
            try
            {
                Result = EntitySet.FirstOrDefault(criterio);
            }
            catch (DbEntityValidationException ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite, EntityValidationErrors = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite });
                this.EscribirEnArchivoLog(ex);
            }
            return Result;
        }

        public List<TEntity> Filter(Expression<Func<TEntity, bool>> criterio)
        {
            List<TEntity> Result = null;
            try
            {
                Result = EntitySet.Where(criterio).ToList();
            }
            catch (DbEntityValidationException ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite, EntityValidationErrors = ex.EntityValidationErrors });
                this.EscribirEnArchivoLog(ex);
            }
            catch (Exception ex)
            {
                Excepcion?.Invoke(this, new ExceptionEventArgs() { Message = ex.Message, InnerException = ex.InnerException, Source = ex.Source, StackTrace = ex.StackTrace, TargetSite = ex.TargetSite });
                this.EscribirEnArchivoLog(ex);
            }
            return Result;
        }        

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }

    /// <summary>
    /// Clase para el tipo de argumentos enviados 
    /// a los consumidores del repositorio
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public MethodBase TargetSite { get; set; }
        public Exception InnerException { get; set; }
        public IEnumerable<DbEntityValidationResult> EntityValidationErrors { get; set; }
    }
}
