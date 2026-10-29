using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUESYNC_C_
{
    internal class diccionarios
    {
        // ==========================================
        //         CONFIGURACIÓN Y CATÁLOGOS 
        // ==========================================
        public static string rutaArchivo = "comprobantes_dhproyectos.txt";
        public static int max_registros = 100;
        public static int total_registros = 0;

        // Precios base de construcción
        public static double precioBasico = 900.0;
        public static double precioPremium = 1200.0;

        // Mano de Obra 
        public static string[] idMo = { "1", "2", "3" };
        public static string[] nomMo = { "Cuadrilla Bási-k (3 pers)", "Cuadrilla Estándar (5 pers)", "Cuadrilla Pro-Master (8 pers)" };
        public static double[] costoMo = { 150.0, 250.0, 400.0 };

        // Mantenimiento
        public static double tarifaHora = 50.0;
        public static double precioKitQuimicos = 80.0;

        // Catálogo de Materiales 
        public static string[] idMat = { "1", "2", "3", "4" };
        public static string[] nomMat = { "Bomba Centrífuga 2HP", "Filtro Cilíndrico Industrial", "Válvula de Comporta 1/2", "Tubería de Cobre 3/4" };
        public static double[] precMat = { 450.0, 120.0, 85.0, 110.0 };
        public static int[] stockMat = { 5, 12, 8, 15 };

        //  los comprobantes guardados en memoria
        public static string[] nroComp = new string[max_registros];
        public static string[] tipoComp = new string[max_registros];
        public static string[] cliente = new string[max_registros];
        public static string[] documento = new string[max_registros];
        public static string[] telefono = new string[max_registros];
        public static string[] servicio = new string[max_registros];
        public static double[] monto = new double[max_registros];
        public static string[] fecha = new string[max_registros];
        public static string[] hora = new string[max_registros];
     

    // Método para registrar un comprobante directamente en los arreglos de diccionarios
        public static void Registrar(string nro, string tipo, string cli, string doc, string telf, string serv, double mon, string fec, string hor)
        {
            if (total_registros < max_registros)
            {
                nroComp[total_registros] = nro;
                tipoComp[total_registros] = tipo;
                cliente[total_registros] = cli;
                documento[total_registros] = doc;
                telefono[total_registros] = telf;
                servicio[total_registros] = serv;
                monto[total_registros] = mon;
                fecha[total_registros] = fec;
                hora[total_registros] = hor;

                total_registros++;
            }
        }
    }

}


