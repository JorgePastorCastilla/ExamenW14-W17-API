using ExamenAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data;
using Dapper;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace ExamenAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Player")]
    public class PlayerController : ApiController
    {
        [HttpPost]
        [Route("InsertNewPlayer")]
        //para devolver 200 404 etc
        public IHttpActionResult InsertNewPlayer(PlayerModel player)
        {
            //conexión con la base de datos
            IDbConnection con = new ApplicationDbContext().Database.Connection;

            string sql = "INSERT INTO dbo.Player(Id,FirstName,LastName,Email,DateOfBirth,NickName,City,BlobUri)" +
                $" VALUES ('{player.Id}','{player.FirstName}','{player.LastName}','{player.Email}','{player.DateOfBirth}'" +
                $",'{player.NickName}','{player.City}','{player.BlobUri}')";

            try
            {
                con.Execute(sql);
            }
            catch (Exception ex)
            {
                return BadRequest("Error inserting player in database, " + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return Ok();
        }


        [HttpGet]
        [Route("GetPlayerDateJoined")]
        public string GetPlayerDateJoined()
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection con = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"select DateJoined from dbo.Player where Id like '{ authenticatedAspNetUserId }'";
                DateTime datejoined = con.Query<DateTime>(sql).FirstOrDefault();
                return datejoined.ToShortDateString();
            }

        }

    }
}
