using Microsoft.Win32;
using ML;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BL
{
  public class Usuario
    {                                       //   ADD  USUARIO
        public static ML.Result AddUsuario(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (_DL.EGrijalvaToDoListEntities context = new _DL.EGrijalvaToDoListEntities())
                {
                    SqlParameter Nombre = new SqlParameter("@Nombre", usuario.Nombre);
                    SqlParameter ApellidoPaterno = new SqlParameter("ApellidoPaterno", usuario.ApellidoPaterno);
                    SqlParameter ApellidoMaterno = new SqlParameter("ApellidoMaterno", usuario.ApellidoMaterno);
                    SqlParameter FechaNacimiento = new SqlParameter("FechaNacimiento", usuario.FechaNacimiento);
                    SqlParameter Correo = new SqlParameter("Correo", usuario.Correo);
                    SqlParameter Password = new SqlParameter("Password", usuario.Password);

                    string user = "AddUsuario @Nombre ,@ApellidoPaterno, @ApellidoMaterno, @FechaNacimiento, @Correo, @Password";

                    var query = context.Database.ExecuteSqlCommand(user, Nombre, ApellidoPaterno, ApellidoMaterno, FechaNacimiento, Correo, Password);

                    if(query > 0)
                    {
                        result.Correct = true;
                        result.Message = " El Registro fue Agregado con Exito."
;                   }
                    else
                    {
                        result.Correct = false;
                        result.Message = "¡Error!, NO ha sido Posible Agregar el Registro.";
                    }
                }
            }
            catch (Exception ex) 
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = ex.Message;
            }

            return result;
        }

                                           //    UPDATE USUARIO
        public static ML.Result UpdateUsuario(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(_DL.EGrijalvaToDoListEntities context = new _DL.EGrijalvaToDoListEntities())
                {
                    SqlParameter Nombre = new SqlParameter("Nombre", usuario.Nombre);
                    SqlParameter ApellidoPaterno = new SqlParameter("ApellidoPaterno", usuario.ApellidoPaterno);
                    SqlParameter ApellidoMaterno = new SqlParameter("ApellidoMaterno", usuario.ApellidoMaterno);
                    SqlParameter FechaNAcimiento = new SqlParameter("FechaNacimiento", usuario.FechaNacimiento);
                    SqlParameter Correo = new SqlParameter("Correo", usuario.Correo);
                    SqlParameter Password = new SqlParameter("Password", usuario.Password);

                    string User = "UpdateUsuario @Nombre, @ApellidoPaterno, @ApellidoMaterno, @FechaNacimiento, @Correo, @Password";

                    var query = context.Database.ExecuteSqlCommand(User, Nombre, ApellidoPaterno, ApellidoMaterno, FechaNAcimiento, Correo, Password);

                    if(query > 0)
                    {
                        result.Correct = true;
                        result.Message = "El Registro fue Actualizado Correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "¡Error!, El Registro NO pudo ser Actualizado con Exito";
                    }
                }
            }
            catch(Exception Ex)
            {
                result.Correct = false;
                result.Ex = Ex;
                result.Message = Ex.Message;
            }

            return result;
        }

                                           //    DELETE USUARIO
        //public static ML.Result DeleteUsuario(int IdUsuario)
        //{
        //    ML.Result result = new ML.Result();

        //    try
        //    {
        //        using (_DL.EGrijalvaToDoListEntities context = new _DL.EGrijalvaToDoListEntities())
        //        {
        //            SqlParameter isUsuario = new SqlParameter("@IdUsuario", IdUsuario);
        //            var query = context.Database.ExcecuteSqlInterpolated($"DeleteUsuario {IdUsuario}");
                   
        //            if( query > 0 )
        //            {
        //                result.Correct = true;
        //                result.Message = "El Registro fue Eliminado Correctamente.";
        //            }
        //            else
        //            {
        //                result.Correct = false;
        //                result.Message = "¡Error!, El Registro NO pudo ser Eliminado.";                   
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        result.Correct = false;
        //        result.Ex = ex;
        //        result.Message = ex.Message;
        //    }

        //    return result;
        //}

                                           //    GET ALL USUARIO
        public static ML.Result GetAllUsuario()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (_DL.EGrijalvaToDoListEntities context = new _DL.EGrijalvaToDoListEntities())
                {
                    var query = (from usuarioLINQ in context.Usuario
                                 select new
                                 {
                                     IdUsuario = usuarioLINQ.IdUsuario,
                                     Nombre = usuarioLINQ.Nombre,
                                     ApellidoPaterno = usuarioLINQ.ApellidoPaterno,
                                     ApellidoMaterno = usuarioLINQ.ApellidoMaterno,
                                     FechaNacimiento = usuarioLINQ.FechaNacimiento,
                                     Correo = usuarioLINQ.Correo,
                                     Password = usuarioLINQ.Password

                                 }).ToList();

                    if(query != null)
                    {
                        if (query.Count > 0)
                        {

                            result.Objects = new List<object>();
                            foreach (var item in query)
                            {
                                ML.Usuario usuario = new ML.Usuario();

                                usuario.IdUsuario = item.IdUsuario;
                                usuario.Nombre = item.Nombre;
                                usuario.ApellidoPaterno = item.ApellidoPaterno;
                                usuario.ApellidoMaterno = item.ApellidoMaterno;
                                usuario.FechaNacimiento = (DateTime)item.FechaNacimiento;
                                usuario.Correo = item.Correo;
                                usuario.Password = item.Password;

                                result.Objects.Add(usuario);
                            }

                            result.Correct = true;
                            result.Message = "Usuarios Consultados Correctamente";
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "La Tabla de Usuarios esta vacia.";
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "NO se pudieron Consultar los Usuarios.";
                    }
                }
               
            }
            catch( Exception ex ) 
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = ex.Message;
            }

            return result;
        }

                                           //     GET BY ID USUARIO
        public static ML.Result GetById(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(_DL.EGrijalvaToDoListEntities context = new _DL.EGrijalvaToDoListEntities())
                {
                    var query = (from usuarioLINQ in context.Usuario
                                 where usuarioLINQ.IdUsuario == IdUsuario
                                 select new
                                 {
                                     IdUsuario = usuarioLINQ.IdUsuario,
                                     Nombre = usuarioLINQ.Nombre,
                                     ApellidoPaterno = usuarioLINQ.ApellidoPaterno,
                                     ApellidoMaterno = usuarioLINQ.ApellidoMaterno,
                                     FechaNacimiento = usuarioLINQ.FechaNacimiento,
                                     Correo = usuarioLINQ.Correo,
                                     Password = usuarioLINQ.Password

                                 }).FirstOrDefault();

                    if(query!= null )
                    {
                        var item = query;

                        ML.Usuario usuario = new ML.Usuario();

                        usuario.IdUsuario = item.IdUsuario;
                        usuario.Nombre = item.Nombre;
                        usuario.ApellidoPaterno = item.ApellidoPaterno;
                        usuario.ApellidoMaterno = item.ApellidoMaterno;
                        usuario.FechaNacimiento = (DateTime)item.FechaNacimiento;
                        usuario.Correo = item.Correo;
                        usuario.Password = item.Password;

                        result.Object = usuario;

                        result.Correct = true;
                        result.Message = "Usuario Consultado con Exito.";
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "NO se encontro al Usuario.";
                    }
                }
            }
            catch ( Exception ex )
            {
                result.Correct = false;
                result.Ex = ex;
                result.Message = ex.Message;
            }

            return result;
        }

        public static Result UsuarioGetById(int v, object idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
