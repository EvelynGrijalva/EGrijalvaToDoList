using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

using System.Data.SqlClient;  // Librería para poder ejecutar el Stored
using System.Data;
using System.Web.Services.Description;

namespace MVC_ToDoList.Controllers
{
    public class AccesoController : Controller
    {
        static string connectionString = "Data Source=DESKTOP-RIVF9IG;Initial Catalog=EGrijalvaToDoList;Integrated Security=True"; // Cadena Conexión

        // GET: Acceso
        public ActionResult Login()    // Vista LOGIN
        {
            return View();
        }
        public ActionResult Registrar()   // Vista REGISTRAR
        {
            return View();
        }



        [HttpPost]
        public ActionResult Registrar(ML.Usuario usuario)
        {
            bool registrado;
            string mensaje;

            // Validar que la contraseña sea identica

            if(usuario.Password == usuario.ConfirmarPassword)
            {
                // De forma Original, este método se vería así:   
                // usuario.Password = "Contraseña123";     pero, no es correcto, por eso se Hashea la contraseña.

                usuario.Password = ConvertSha256(usuario.Password);   // Se Actualiza la contraseña con el HASH256 (encriptada)

            }
            else
            {
                ViewData["Mensaje"] = " ¡Ops!, Las Contraseñas que ingresaste NO Coinciden. ";     // El viewData Permite envíar Datos del CONTROLADOR a la VISTA.
                return View();
            }


            // Metodo para Registrar Usuario SP

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", con);

                // Parametros de Entrada Igual que en la Base de Datos:

                cmd.Parameters.AddWithValue("Correo", usuario.Correo);   
                cmd.Parameters.AddWithValue("Password", usuario.Password);

                // Parametros de Salida igual que el SP en Base de Datos:

                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open(); // Se abre la conexión

                cmd.ExecuteNonQuery();

                // Leer Parametros de Salida

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = mensaje;   // Se muestra el Mensaje del Stored Procedure en la VISTA por medio del ViewData

            // Validar si el Usuario ya esta Registrado

            if (registrado)   // Condicional, si registrado es True
            {
                return RedirectToAction("Login", "Acceso");  // Nos redirecciona al Login que esta en el Controller Acceso
            }
            else   // De lo contrarío
            {
                return View();  // Retorna la vista Actual C/ el Mensaje de Error.
            }

        }

        // Logica de LOGIN

        [HttpPost]
        public ActionResult Login(ML.Usuario usuario)
        {
            // Encriptar Contraseña recibida en la Interfaz

            usuario.Password = ConvertSha256(usuario.Password);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", con);

                // Parametros de Entrada Igual que en la Base de Datos:

                cmd.Parameters.AddWithValue("Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("Password", usuario.Password);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                usuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());  // Lee la primera Fila y/o 1ra Columna
        
            }

            if(usuario.IdUsuario != 0)  // Si se encuentra al Usuario c/ esas Credenciales
            {
                Session["usuario"] = usuario;   // Se Crea Sesión

                return RedirectToAction("Index", "Home");   // Se redirecciona al Index del Controlador del Home

            }
            else    // De lo contrario
            {
                ViewData["Mensaje"] = " ¡Lo sentimos!, El Usuario NO ha sido Encontrado. ";

                return View();
            }
            
        }

        // Hasheo de Contraseña    En caso de Error:      using System.Text;          REFERENCIA :   System.Security.Cryptography

        public static string ConvertSha256(string texto)  // Recibe un objeto tipo String (Contraseña)
        {
            StringBuilder sb = new StringBuilder(); 

            using (SHA256 hash = SHA256Managed.Create())  // Logica de Hasheo Tipo Hash256
            {
                Encoding enc = Encoding.UTF8;   

                byte[] result = hash.ComputeHash(enc.GetBytes(texto));  // Obtiene los bytes de la cadena de texto

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();   // Devuelve la Contraseña Hasheada
        }
    }
}