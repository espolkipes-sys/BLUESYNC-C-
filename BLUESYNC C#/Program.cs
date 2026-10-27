using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUESYNC_C_
{
    internal class Program
    {    
        // MÉTODOS DE FECHA Y HORA
        public static string ObtenerFechaActual()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");//parametro ordenado
        }
        public static string ObtenerHoraActual()
        {
            return DateTime.Now.ToString("HH:mm:ss");//parametro ordenado
        }
        static void Main(string[] args)
        { 
        }
    }
}
