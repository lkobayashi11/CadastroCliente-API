using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_teste.Models;

namespace API_teste.Controllers
{
    public class TelefoneController : ApiController
    {
        [Route("telefone")]
        public HttpResponseMessage Get()
        {
            using (CliTelDBContext dbContext = new CliTelDBContext())
            {
                var entity = (from t in dbContext.tb_telefone
                              join c in dbContext.tb_cliente on t.ID_CLI equals c.ID_CLI
                              select new
                              {
                                  ID_TEL = t.ID_TEL,
                                  Nome_CLI = c.Nome_CLI,
                                  Endereco_CLI = c.Endereco_CLI,
                                  Numero_CLI = c.Numero_CLI,
                                  Bairro_CLI = c.Bairro_CLI,
                                  Cidade_CLI = c.Cidade_CLI,
                                  UF_CLI = c.UF_Cli,
                                  Telefone_CLI = t.Telefone_CLI,
                              }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
        }
        [Route("telefone/ByID/{id}")]
        public HttpResponseMessage GetByID(int id)
        {
            using (CliTelDBContext dbContext = new CliTelDBContext())
            {
                 var entity = (from t in dbContext.tb_telefone
                              join c in dbContext.tb_cliente on t.ID_CLI equals c.ID_CLI
                              where t.ID_TEL == id
                              select new
                              {
                                  ID_TEL = t.ID_TEL,
                                  Nome_CLI = c.Nome_CLI,
                                  Endereco_CLI = c.Endereco_CLI,
                                  Numero_CLI = c.Numero_CLI,
                                  Bairro_CLI = c.Bairro_CLI,
                                  Cidade_CLI = c.Cidade_CLI,
                                  UF_CLI = c.UF_Cli,
                                  Telefone_CLI = t.Telefone_CLI,
                              }).FirstOrDefault();

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Não existe telefone com ID " + id.ToString());
                }
            }
        }
                
        [Route("telefone/ByCustomer/{id}")]
        public HttpResponseMessage GetByCliente(int id)
        {
            using (CliTelDBContext dbContext = new CliTelDBContext())
            {
                var entity = (from t in dbContext.tb_telefone
                              join c in dbContext.tb_cliente on t.ID_CLI equals c.ID_CLI
                              where t.ID_CLI == id
                              select new
                              {
                                  ID_CLI = c.ID_CLI,
                                  Nome_CLI = c.Nome_CLI,
                                  Endereco_CLI = c.Endereco_CLI,
                                  Numero_CLI = c.Numero_CLI,
                                  Bairro_CLI = c.Bairro_CLI,
                                  Cidade_CLI = c.Cidade_CLI,
                                  UF_CLI = c.UF_Cli,
                                  Telefone_CLI = t.Telefone_CLI,
                              }).ToList();

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Não existe telefone para o Cliente ID " + id.ToString());
                }
            }
        }

        [Route("telefone")]
        public HttpResponseMessage Post([FromBody] tb_telefone telefone)
        {
            try
            {
                using (CliTelDBContext dbContext = new CliTelDBContext())
                {
                    dbContext.tb_telefone.Add(telefone);
                    dbContext.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, telefone);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        telefone.ID_TEL.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("telefone/{id}")]
        public HttpResponseMessage Put(int id, [FromBody] tb_telefone telefone)
        {
            try
            {
                using (TelefoneDBContext dbContext = new TelefoneDBContext())
                {
                    var entity = dbContext.tb_telefone.FirstOrDefault(e => e.ID_TEL == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Telefone com o id " + id.ToString() + " não encontrado");
                    }
                    else
                    {
                         entity.Telefone_CLI = telefone.Telefone_CLI;

                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
      
        [Route("telefone/{id}")]
         public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (TelefoneDBContext dbContext = new TelefoneDBContext())
                {
                    var entity = dbContext.tb_telefone.FirstOrDefault(e => e.ID_TEL == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Telefone com o Id = " + id.ToString() + " não encontrado");
                    }
                    else
                    {
                        dbContext.tb_telefone.Remove(entity);
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
       
    }
}
