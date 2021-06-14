using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_teste.Models;

namespace API_teste.Controllers
{
    public class ClienteController : ApiController
    {
        [Route("clientes")]
        public HttpResponseMessage Get()
        {
            using (ClienteDBContext dbContext = new ClienteDBContext())
            {
                var Clientes = dbContext.tb_cliente.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, Clientes);
            }
        }

        [Route("clientes/ByID/{id}")]
        public HttpResponseMessage Get(int id)
        {
            using (ClienteDBContext dbContext = new ClienteDBContext())
            {
                var entity = dbContext.tb_cliente.FirstOrDefault(e => e.ID_CLI == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Não existe cliente com ID " + id.ToString());
                }
            }
        }

        [Route("clientes/ByName/{nome}")]
        public HttpResponseMessage GetByName(String nome)
        {
            using (ClienteDBContext dbContext = new ClienteDBContext())
            {
                 var entity = (from c in dbContext.tb_cliente
                                where c.Nome_CLI.Contains(nome)
                                select new
                                {
                                    Nome_CLI = c.Nome_CLI,
                                    Endereco_CLI = c.Endereco_CLI,
                                    Numero_CLI = c.Numero_CLI,
                                    Bairro_CLI = c.Bairro_CLI,
                                    Cidade_CLI = c.Cidade_CLI,
                                    UF_CLI = c.UF_Cli,
                                }).ToList();
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Não existe cliente com o nome " + nome.ToString());
                }
            }
        }
  
        [Route("clientes/ByAddress/{endereco}")]
        public HttpResponseMessage GetByAddress(String endereco)
        {
            using (ClienteDBContext dbContext = new ClienteDBContext())
            {
                 var entity = (from c in dbContext.tb_cliente
                              where c.Endereco_CLI.Contains(endereco)
                              select new
                              {
                                  Nome_CLI = c.Nome_CLI,
                                  Endereco_CLI = c.Endereco_CLI,
                                  Numero_CLI = c.Numero_CLI,
                                  Bairro_CLI = c.Bairro_CLI,
                                  Cidade_CLI = c.Cidade_CLI,
                                  UF_CLI = c.UF_Cli,
                              }).ToList();
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        "Não existe cliente com o endereço " + endereco.ToString());
                }
            }
        }

        [Route("clientes/ByPhone/{telefone}")]
        public HttpResponseMessage GetByPhone(String telefone)
        {
              using (CliTelDBContext dbContext = new CliTelDBContext()) 
                    {
                          var entity = (from  t in dbContext.tb_telefone
                          join c in dbContext.tb_cliente  on t.ID_CLI equals c.ID_CLI
                                      where t.Telefone_CLI == telefone
                                      select new
                                        {
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
                                "Não existe cliente com o telefone " + telefone.ToString());
                        }
                    }
        }

        [Route("clientes")]
        public HttpResponseMessage Post([FromBody] tb_cliente cliente)
        {
        try
        {
        using (ClienteDBContext dbContext = new ClienteDBContext())
        {
            dbContext.tb_cliente.Add(cliente);
            dbContext.SaveChanges();
            var message = Request.CreateResponse(HttpStatusCode.Created, cliente);
            message.Headers.Location = new Uri(Request.RequestUri +
                cliente.ID_CLI.ToString());
            return message;
        }
        }
        catch (Exception ex)
        {
        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
        }
        }

        [Route("clientes/{id}")]
        public HttpResponseMessage Put(int id, [FromBody] tb_cliente cliente)
        {
        try
        {
        using (ClienteDBContext dbContext = new ClienteDBContext())
        {
            var entity = dbContext.tb_cliente.FirstOrDefault(e => e.ID_CLI == id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    "Cliente com Id " + id.ToString() + " não encontrado");
            }
            else
            {
                entity.Nome_CLI = cliente.Nome_CLI;
                entity.Endereco_CLI = cliente.Endereco_CLI;
                entity.Numero_CLI = cliente.Numero_CLI;
                entity.Bairro_CLI = cliente.Bairro_CLI;
                entity.Cidade_CLI = cliente.Cidade_CLI;
                entity.UF_Cli = cliente.UF_Cli;
                entity.CEP_Cli = cliente.CEP_Cli;
                entity.Telefone_CLI = cliente.Telefone_CLI;

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

        [Route("clientes/{id}")]
        public HttpResponseMessage Delete(int id)
        {
        try
        {
        using (CliTelDBContext dbContext = new CliTelDBContext())
        {
            var tel = dbContext.tb_telefone.FirstOrDefault(e => e.ID_CLI == id);
            if (tel == null)
                 {
                    var entity = dbContext.tb_cliente.FirstOrDefault(e => e.ID_CLI == id);
                    if (entity == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                "Cliente com Id = " + id.ToString() + " não encontrado");
                        }
                    else
                        {
                            dbContext.tb_cliente.Remove(entity);
                            dbContext.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                    }
            else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                "Cliente possui telefone cadastrado. Deletar telefone primeiramente");
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


