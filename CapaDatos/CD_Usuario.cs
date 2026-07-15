using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CapaDatos
{
    public class CD_Usuario
    {
        public List<Usuario> Listar() {

            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena)) {
                try {



                    StringBuilder query = new StringBuilder();
                    query.Append("SELECT u.IdUsuario, u.Documento, u.NombreCompleto, u.Correo, u.Clave, u.Telefono, u.Estado, r.IdRol, r.Descripcion from Usuario u ");
                    query.Append("inner join ROL r on r.IdRol = u.IdRol");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) {
                        while (dr.Read()) {
                            lista.Add(new Usuario(){
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Documento = dr["Documento"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                oRol = new Rol() { IdRol = Convert.ToInt32(dr["IdRol"]),
                                Descripcion = dr["Descripcion"].ToString()}
                            });   
                        }
                    }
                } catch (Exception ex) {
                    lista = new List<Usuario>();
                };
            }
            return lista;
        }

        public int Registrar(Usuario obj, string Mensaje) {
            int IdUsuarioGenerado = 0;
            Mensaje = string.Empty;
            try{
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena)) {
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", oconexion);

                    /*
                    @Documento varchar(50),
	@NombreCompleto varchar(100),
	@Correo varchar(100),
	@Clave varchar(100),
	@IdRol int,
    @Telefono varchar(100),
	@Estado bit,
    @IdUsuarioResultado int output,
    @Mensaje varchar(500) output*/

                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("IdRol", obj.oRol.IdRol);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("IdUsuarioResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    IdUsuarioGenerado = Convert.ToInt32(cmd.Parameters["IdUsuarioResultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex) {
                IdUsuarioGenerado = 0;
                Mensaje = ex.Message;
            }
            return IdUsuarioGenerado;
        }
    }
}
