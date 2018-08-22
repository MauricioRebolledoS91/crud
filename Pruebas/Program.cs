using System;

namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Modelos.Repositorio<Modelos.Estatu> obj = new Modelos.Repositorio<Modelos.Estatu>())
            {
                obj.Excepcion += Obj_Excepcion;
                obj.Create(new Modelos.Estatu() { Descripcion = "En proceso" });
            }

            Console.WriteLine("Presione <enter> para salir...");
            Console.ReadLine();
        }

        private static void Obj_Excepcion(object sender, Modelos.ExceptionEventArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.EntityValidationErrors != null) //Fue una excepción de otro tipo
            {
                foreach (var item in e.EntityValidationErrors)
                    foreach (var error in item.ValidationErrors)
                        Console.WriteLine(string.Format("Error message: {0}, PropertyName: {1}", error.ErrorMessage, error.PropertyName));
            }
        }
    }
}
