using POOI_EF_TORRES_VENERO.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POOI_EF_TORRES_VENERO.Controllers
{
    public class ConsumidorController : Controller
    {
        private string cadenaConexion = ConfigurationManager.ConnectionStrings["conexionADatos"].ConnectionString;
        IEnumerable<Consumidor> listarConsumidores()
        {
            List<Consumidor> lista = new List<Consumidor>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_consumidor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Consumidor
                    {
                        Dnicont = dr.GetString(0),
                        Nomcont = dr.GetString(1),
                        Apecont = dr.GetString(2),
                        Emailcont = dr.IsDBNull(3) ? "" : dr.GetString(3),
                        Idpais = dr.IsDBNull(4) ? "" : dr.GetString(4)
                    });
                }
                dr.Close();
            }
            return lista;
        }

        string agregarConsumidor(Consumidor c)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_agrega_consumidor", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dnicont", (object)c.Dnicont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nomcont", (object)c.Nomcont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@apecont", (object)c.Apecont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@emailcont", string.IsNullOrWhiteSpace(c.Emailcont) ? (object)DBNull.Value : c.Emailcont);
                    cmd.Parameters.AddWithValue("@idpais", (object)c.Idpais ?? DBNull.Value);

                    SqlParameter pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 200);
                    pMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();
                    mensaje = pMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                mensaje = "ERROR: " + ex.Message;
            }
            return mensaje;
        }

        string eliminarConsumidor(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return "ERROR: Debe indicar el DNI del consumidor.";
            }

            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_eliminar_consumidor", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dnicont", dni);

                    SqlParameter pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 200);
                    pMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();
                    mensaje = pMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                mensaje = "ERROR: " + ex.Message;
            }
            return mensaje;
        }

        Consumidor obtenerConsumidor(string dni)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                if (string.IsNullOrWhiteSpace(dni))
                {
                    return null;
                }
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_obtener_consumidor", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dnicont", dni);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Consumidor c = new Consumidor
                    {
                        Dnicont = dr.GetString(0),
                        Nomcont = dr.GetString(1),
                        Apecont = dr.GetString(2),
                        Emailcont = dr.IsDBNull(3) ? "" : dr.GetString(3),
                        Idpais = dr.IsDBNull(4) ? "" : dr.GetString(4)
                    };
                    dr.Close();
                    return c;
                }
                dr.Close();
            }
            return null;
        }

        string actualizarConsumidor(Consumidor c)
        {
            string mensaje = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaConexion))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("usp_actualizar_consumidor", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@dnicont", (object)c.Dnicont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@nomcont", (object)c.Nomcont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@apecont", (object)c.Apecont ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@emailcont", string.IsNullOrWhiteSpace(c.Emailcont) ? (object)DBNull.Value : c.Emailcont);
                    cmd.Parameters.AddWithValue("@idpais", (object)c.Idpais ?? DBNull.Value);

                    SqlParameter pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 200);
                    pMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();
                    mensaje = pMensaje.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                mensaje = "ERROR: " + ex.Message;
            }
            return mensaje;
        }

        List<SelectListItem> listarPaises()
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("usp_listar_paises", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new SelectListItem
                    {
                        Value = dr.GetString(0),
                        Text = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return lista;
        }

        // ===================== ACTIONRESULT =====================

        
        public ActionResult Index()
        {
            ViewBag.Mensaje = TempData["Mensaje"];
            return View(listarConsumidores());
        }

        
        public ActionResult Create()
        {
            ViewBag.Paises = listarPaises();
            return View(new Consumidor());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Consumidor consumidor)
        {
            ViewBag.Paises = listarPaises();

            if (!ModelState.IsValid)
            {
                return View(consumidor);
            }

            string mensaje = agregarConsumidor(consumidor);

            if (mensaje.StartsWith("ERROR",
                StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Mensaje = mensaje;
                return View(consumidor);
            }

            // Si el registro fue exitoso, volver al listado
            TempData["Mensaje"] = mensaje;
            return RedirectToAction("Index");
        }


        public ActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Mensaje"] = "ERROR: Debe indicar el DNI.";
                return RedirectToAction("Index");
            }

            Consumidor c = obtenerConsumidor(id);

            if (c == null)
            {
                TempData["Mensaje"] = "ERROR: El consumidor no existe.";
                return RedirectToAction("Index");
            }

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Mensaje"] = "ERROR: Debe indicar el DNI.";
                return RedirectToAction("Index");
            }

            string mensaje = eliminarConsumidor(id);

            TempData["Mensaje"] = mensaje;

            return RedirectToAction("Index");
        }

        public ActionResult Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["Mensaje"] = "ERROR: Debe indicar el DNI.";
                return RedirectToAction("Index");
            }

            Consumidor c = obtenerConsumidor(id);

            if (c == null)
            {
                TempData["Mensaje"] = "ERROR: El consumidor no existe.";
                return RedirectToAction("Index");
            }

            ViewBag.Paises = listarPaises();

            return View(c);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Consumidor consumidor)
        {
            ViewBag.Paises = listarPaises();

            if (!ModelState.IsValid)
            {
                return View(consumidor);
            }

            string mensaje = actualizarConsumidor(consumidor);

            if (mensaje.StartsWith("OK",
                StringComparison.OrdinalIgnoreCase))
            {
                TempData["Mensaje"] = mensaje;
                return RedirectToAction("Index");
            }

            ViewBag.Mensaje = mensaje;

            return View(consumidor);
        }




        public ActionResult Details(string id)
        {
            Consumidor c = obtenerConsumidor(id);
            if (c == null)
            {
                TempData["Mensaje"] = "ERROR: El consumidor no existe.";
                return RedirectToAction("Index");
            }
            return View(c);
        }
    }
}