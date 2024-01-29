using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BL
{
    public class Estatus
    {
        public static ML.Result GetAllStatus()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (_DL.EGrijalvaToDoListEntities context = new _DL.EGrijalvaToDoListEntities())
                {
                    var query = ( from statusLINQ in context.Estatus
                                  select new
                                  {
                                      IdStatus = statusLINQ.IdStatus,
                                      Descripcion = statusLINQ.Descripcion
                                 }).ToList();

                    if(query != null)
                    {
                        if(query.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach(var item in query)
                            {
                                ML.Estatus estatus = new ML.Estatus();

                                estatus.IdStatus = item.IdStatus;
                                estatus.Descripcion = item.Descripcion;

                                result.Objects.Add(item);
                            }

                            result.Correct = true;
                            result.Message = "Mostrar Estatus";
                        }
                        else 
                        {
                            result.Correct = true;
                            result.Message = "No Existen Valores";
                        } 
                    }
                    else
                    {
                        result.Correct = false;
                        result.Message = "NO se pudo Completar la Consulta";
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
    }
}
