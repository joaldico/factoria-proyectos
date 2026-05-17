using Microsoft.Extensions.Options;
using Factoria.Proyectos.Api.Shared.Dto.Genericas;
using System.Text;
using System;

namespace Factoria.Proyectos.Api.Infraestructura.Servicios
{
    public class ArchivoTexto
    {
        private readonly Log _settings;

        public ArchivoTexto(IOptions<Log> settings)
        {
            _settings = settings.Value;
        }

        public void GenerarArchivo<T>(
        T obj,
        string? rutaArchivo = null,
        string nombreArchivo = "")
        {
            try
            {
                rutaArchivo ??= ObtenerRutaPorDefecto();
                CrearDirectorioSiNoExiste(rutaArchivo);

                using var stream = new FileStream(
                Path.Combine(rutaArchivo, nombreArchivo),FileMode.Append,FileAccess.Write,FileShare.Write);

                using var writer = new StreamWriter(stream, Encoding.UTF8);
                writer.WriteLine($"Fecha y Hora = {DateTime.Now}");
                foreach (var prop in obj!.GetType().GetProperties())
                {
                    writer.WriteLine($"{prop.Name} = {prop.GetValue(obj) ?? ""}");
                }
                writer.WriteLine(new string('_', 50));
            }
            catch (Exception ex) 
            {
                Console.WriteLine("CRITICAL ERROR LOGGING OBJECT:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine($"Error intentando guardar objeto tipo: {typeof(T).Name}");
            }
        }

        public void GenerarArchivoTexto(
        string contenido,
        string? rutaArchivo = null,
        string nombreArchivo = "")
        {
            try
            {
                rutaArchivo ??= ObtenerRutaPorDefecto();
                CrearDirectorioSiNoExiste(rutaArchivo);

                using var stream = new FileStream(
                Path.Combine(rutaArchivo, nombreArchivo),
                FileMode.Append,
                FileAccess.Write,
                FileShare.Write);

                using var writer = new StreamWriter(stream, Encoding.UTF8);

                writer.WriteLine($"Fecha y Hora = {DateTime.Now}");
                writer.WriteLine(contenido);
                writer.WriteLine(new string('_', 50));
            }
            catch (Exception ex)
            {
                Console.WriteLine("CRITICAL ERROR LOGGING TEXT:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("CONTENIDO DEL ERROR ORIGINAL:");
                Console.WriteLine(contenido); 
            }
        }

        private string ObtenerRutaPorDefecto()
        {
            return Path.Combine(_settings.RutaLog, AnioMesDia(_settings.NombreLog, ".txt"));
        }

        private string AnioMesDia(string texto, string extension = "")
        {
            DateTime now = DateTime.Now;
            return $"{texto}_{now.Year}_{now.Month.ToString().PadLeft(2, '0')}_{now.Day.ToString().PadLeft(2, '0')}{extension}";
        }

        private void CrearDirectorioSiNoExiste(string ruta)
        {
            var dir = Path.GetDirectoryName(ruta);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);
        }
    }
}