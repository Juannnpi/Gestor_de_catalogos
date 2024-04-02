using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;
using System.Configuration;

namespace negocio
{
    public class ArticulosNegocio
    {
        AccesoDatos datos = new AccesoDatos();
        List<Articulo> lista = new List<Articulo>();
        string consulta = ConfigurationManager.AppSettings["consulta-listar"];

        public List<Articulo> listar()
        {
            try
            {
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.urlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = (decimal)datos.Lector.GetDecimal(7);
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            if (campo == "Precio")
            {
                switch (criterio)
                {
                    case "Mayor a":
                        consulta += "AND A.Precio > '" + filtro + "'";
                        break;
                    case "Menor a":
                        consulta += "AND A.Precio < '" + filtro + "'";
                        break;                        
                }
            }
            else if (campo == "Nombre")
            {
                switch (criterio)
                {
                    case "Comienza con":
                        consulta += "AND A.Nombre like '" + filtro + "%'";
                        break;
                    case "Termina con":
                        consulta += "AND A.Nombre like '%" + filtro + "'";
                        break;
                    default:
                        consulta += "AND A.Nombre like '%" + filtro + "%'";
                        break;
                }
            }            
            if (campo == "Marca")            
                consulta += "AND M.Descripcion = '" + criterio + "'";            
            if (campo == "Categoría")
                consulta += "AND C.Descripcion = '" + criterio + "'";
            listar();
            return lista;
        }

        public void eliminarLogico(int id)
        {
            try
            {
                datos.setearConsulta(ConfigurationManager.AppSettings["accion-eliminarlogico"]);
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarFisico(int id)
        {
            try
            {
                datos.setearConsulta(ConfigurationManager.AppSettings["accion-eliminar"]);
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void agregar(Articulo articulo)
        {
            try
            {
                datos.setearConsulta(ConfigurationManager.AppSettings["accion-insertar"]);
                datos.setearParametro("@Codigo", articulo.Codigo);
                datos.setearParametro("@Nombre", articulo.Nombre);
                datos.setearParametro("@Descripcion", articulo.Descripcion);
                datos.setearParametro("@IdMarca", articulo.Marca.Id);
                datos.setearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametro("@ImagenUrl", articulo.urlImagen);
                datos.setearParametro("@Precio", articulo.Precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Articulo articulo)
        {
            try
            {
                datos.setearConsulta(ConfigurationManager.AppSettings["accion-modificar"]);
                datos.setearParametro("@Codigo", articulo.Codigo);
                datos.setearParametro("@Nombre", articulo.Nombre);
                datos.setearParametro("@Descripcion", articulo.Descripcion);
                datos.setearParametro("@IdMarca", articulo.Marca.Id);
                datos.setearParametro("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametro("@ImagenUrl", articulo.urlImagen);
                datos.setearParametro("@Precio", articulo.Precio);
                datos.setearParametro("@Id", articulo.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
