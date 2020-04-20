using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.EntityFrameworkCore;

using Backnet.Models;
using ENTIDAD;

namespace Backnet.Controllers
{
    public class ServicesController : ApiController
    {
        /* INICIO MÉTODOS POST */
        [HttpPost]
        [Route("services/addStores")]
        public HttpResponseMessage AddStores([FromBody]StoresRequest store)
        {
            try
            {
                using (BackNetEntity db = new BackNetEntity())
                {
                    var oStores = new Models.stores();
                    oStores.name = store.name;
                    oStores.address = store.address;
                    db.stores.Add(oStores);
                    db.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, store);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        store.id.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("services/addArticles")]
        public HttpResponseMessage AddArticles([FromBody]ArticleRequest article)
        {
            try
            {
                using (BackNetEntity db = new BackNetEntity())
                {
                    var oArticle = new Models.articles();
                    oArticle.name = article.name;
                    oArticle.description = article.description;
                    oArticle.price = article.price;
                    oArticle.total_in_shelf = article.total_in_shelf;
                    oArticle.total_in_vault = article.total_in_vault;
                    oArticle.store_id = article.store_id;
                    db.articles.Add(oArticle);
                    db.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, article);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        article.id.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
          
        }
        /* FINAL MÉTODOS POST */

        /* INICIO MÉTODOS GET */
        [HttpGet]
        [Route("services/stores")]
        public IHttpActionResult GetAllStores()
        {
            IList<getStores> lista = null;

            using (var db = new BackNetEntity())
            {
                lista = db.stores.Include("StoreAdd")
                    .Select(s => new getStores()
                    {
                        id = s.id,
                        name = s.name,
                        address = s.address
                    }).ToList<getStores>();
            }

            if (lista.Count == 0)
            {
                return NotFound();
            }

            return Ok(lista);
        }

        [HttpGet]
        [Route("services/stores")]
        public IHttpActionResult GetStoresById(int id)
        {
            getStores store = null;
            using (BackNetEntity db = new BackNetEntity())
            {
                //IQueryable<stores> list =
                //    from s in db.stores
                //    select new stores
                //    {
                //        id = s.id,
                //        name = s.name,
                //        address = s.address
                //    };

                store = db.stores.Include("StoreAdd")
                    .Where(s => s.id == id)
                    .Select(s => new getStores()
                    {
                        id = s.id,
                        name = s.name,
                        address = s.address
                    }).FirstOrDefault<getStores>();

            }

            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }

        [HttpGet]
        [Route("services/articles")]
        public IHttpActionResult GetArticles()
        {
            IList<getArticles> lista = null;
            using (var db = new BackNetEntity())
            {
                lista = db.articles.Include("ArticleAdd")
                         .Select(s => new getArticles()
                         {
                             id = s.id,
                             name = s.name,
                             description = s.description,
                             price = s.price,
                             total_in_shelf = s.total_in_shelf,
                             total_in_vault = s.total_in_vault,
                             store_id = s.store_id
                         }).ToList<getArticles>();
            }

            if (lista.Count == 0)
            {
                return NotFound();
            }

            return Ok(lista);
        }

        [HttpGet]
        [Route("services/articles")]
        public IHttpActionResult GetArticlesById(int id)
        {
            getArticles article = null;
            using (var db = new BackNetEntity())
            {
                article = db.articles.Include("ArticleAdd")
                        .Where(s => s.id == id)
                         .Select(s => new getArticles()
                         {
                             id = s.id,
                             name = s.name,
                             description = s.description,
                             price = s.price,
                             total_in_shelf = s.total_in_shelf,
                             total_in_vault = s.total_in_vault,
                             store_id = s.store_id
                         }).FirstOrDefault<getArticles>();
            }

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);

        }
        /* FINAL MÉTODOS GET */

        /* INICIO MÉTODOS PUT */
        [HttpPut]
        [Route("services/updStores")]
        public HttpResponseMessage PutStores(StoresRequest oModel)
        {
            try
            {
                using (BackNetEntity db = new BackNetEntity())
                {
                    stores oStore = db.stores.Find(oModel.id);
                    if (oStore == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "La Store con el Id " + oModel.id.ToString() + " no pudo actualizarse");
                    }
                    else
                    {
                        oStore.name = oModel.name;
                        oStore.address = oModel.address;
                        db.Entry(oStore).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        [Route("services/updArticles")]
        public HttpResponseMessage PutArticles(ArticleRequest oModel)
        {
            try
            {
                using (BackNetEntity db = new BackNetEntity())
                {
                    articles oArticles = db.articles.Find(oModel.id);
                    if (oArticles == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "La Store con el Id " + oModel.id.ToString() + " no pudo actualizarse");
                    }
                    else
                    {
                        oArticles.name = oModel.name;
                        oArticles.description = oModel.description;
                        oArticles.price = oModel.price;
                        oArticles.total_in_shelf = oModel.total_in_shelf;
                        oArticles.total_in_vault = oModel.total_in_vault;
                        oArticles.store_id = oModel.store_id;
                        db.Entry(oArticles).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        /* FINAL MÉTODOS PUT */

        /* INICIO MÉTODOS DELETE */
        [HttpDelete]
        [Route("services/delStores")]
        public HttpResponseMessage DelStores(int id)
        {
            try
            {
                using (BackNetEntity db = new BackNetEntity())
                {
                    stores oStore = db.stores.Find(id);
                    if (oStore == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "La Store con el Id " + id.ToString() + " no pudo Eliminarse");
                    }
                    else
                    {
                        db.stores.Remove(oStore);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //[HttpDelete("{id}")]
        [HttpDelete]
        [Route("services/delArticles")]
        public HttpResponseMessage DelArticles(int id)
        {
            try
            {
                using (BackNetEntity db = new BackNetEntity())
                { 
                    articles oArticles = db.articles.Find(id);
                    if (oArticles == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "La Store con el Id " + id.ToString() + " no pudo Eliminarse");
                    }
                    else
                    {
                        db.articles.Remove(oArticles);
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        /* FIN MÉTODOS DELETE */
    }
}
