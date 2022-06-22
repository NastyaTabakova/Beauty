using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Beauty.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Beauty.Controllers
{
    public class RandomUserController : Controller
    {
        public string connectionString = "server=localhost;user=root;port=3306;password=1111;database=beauty";

        public ActionResult HomePage()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var category = new List<CategoryNew>();
                MySqlCommand selectPhoto = new MySqlCommand($"select * from Category", con);
                MySqlDataAdapter da = new MySqlDataAdapter(selectPhoto);
                DataSet ds = new DataSet();
                da.Fill(ds);
                foreach (DataTable dt in ds.Tables)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        CategoryNew categ = new CategoryNew();
                        var line = row.ItemArray;
                        categ.Id = Convert.ToInt32(line[0]);
                        categ.CategoryName = Convert.ToString(line[1]);
                        Byte[] byteBLOBData = new Byte[100000000];
                        byteBLOBData = ((Byte[])line[2]);
                        categ.Image = byteBLOBData;
                        category.Add(categ);
                    }
                }
                con.Close();
                return View(category);
            }
        }
        public ActionResult Subcategory(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var subcategory = new List<Subcategory>();
                MySqlCommand selectSubcategory = new MySqlCommand($"select * from Subcategory where CategoryId='{id}'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(selectSubcategory);
                DataSet ds = new DataSet();
                da.Fill(ds);
                foreach (DataTable dt in ds.Tables)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        Subcategory sub = new Subcategory();
                        var line = row.ItemArray;
                        sub.Id = Convert.ToInt32(line[0]);
                        sub.Categoryid = Convert.ToInt32(line[1]);
                        sub.Subcategoryname = Convert.ToString(line[2]);
                        Byte[] byteBLOBData = new Byte[100000000];
                        byteBLOBData = ((Byte[])line[3]);
                        sub.Image = byteBLOBData;
                        subcategory.Add(sub);
                    }
                }
                con.Close();
                return View(subcategory);
            }
        }
        public ActionResult Services(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var model = new List<MasInf>();
                MySqlCommand selectsub2 = new MySqlCommand("select user.firstname, user.lastname, masterinfo.city, masterinfo.street, masterinfo.house, masterservice.price, user.id, masterservice.subcategoryid, masterservice.discription from user join masterinfo on masterinfo.id = user.id join masterservice on masterservice.masterinfoid = masterinfo.id " +
                    $"where masterservice.subcategoryid = '{id}' and masterservice.status=1 ", con);
                MySqlDataReader reader = selectsub2.ExecuteReader();
                while (reader.Read())
                {
                    MasInf info = new MasInf();
                    info.FirstName = reader.GetString("firstname");
                    info.LastName = reader.GetString("lastname");
                    info.City = reader.GetString("city");
                    info.Street = reader.GetString("street");
                    info.House = reader.GetString("house");
                    info.Price = reader.GetInt32("price");
                    info.Id = reader.GetInt32("id");
                    info.SubId = reader.GetInt32("subcategoryid");
                    info.Discription = reader.GetString("discription");
                    model.Add(info);
                }
                reader.Close();
                con.Close();
                return View(model);
            }
        }


        public ActionResult HelloPage()
        {
            return View();
        }

        public ActionResult selectSub(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var model = new List<MasInf>();
                MySqlCommand selectsub2 = new MySqlCommand("select user.firstname, user.lastname, masterinfo.city, masterinfo.street, masterinfo.house,   masterservice.price, user.id, masterservice.subcategoryid from user join masterinfo on masterinfo.id = user.id join masterservice on masterservice.masterinfoid = masterinfo.id " +
                    $"where masterservice.subcategoryid = '{id}' ", con);
                MySqlDataReader reader = selectsub2.ExecuteReader();
                while (reader.Read())
                {
                    MasInf info = new MasInf();
                    info.FirstName = reader.GetString("firstname");
                    info.LastName = reader.GetString("lastname");
                    info.City = reader.GetString("city");
                    info.Street = reader.GetString("street");
                    info.House = reader.GetString("house");
                    info.Price = reader.GetInt32("price");
                    info.Id = reader.GetInt32("id");
                    info.SubId = reader.GetInt32("subcategoryid");
                    info.Discription = reader.GetString("discription");
                    model.Add(info);
                }
                reader.Close();
               
                con.Close();
                return View(model);
            }
        }

        public ActionResult selectMasterPortfolio(int idmas)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                var model = new List<Portfolio>();
                Portfolio portfolio = new Portfolio();
                MySqlCommand selectInfo = new MySqlCommand($"select user.firstname, user.lastname, masterinfo.country, masterinfo.city, masterinfo.street, masterinfo.house, masterinfo.servicelocation, masterinfo.workexperience from user join masterinfo on user.id=masterinfo.id where user.id = {idmas}", con);
                MySqlDataReader portfolioreader = selectInfo.ExecuteReader();
                while (portfolioreader.Read())
                {
                    portfolio.FirstName = portfolioreader.GetString("firstname");
                    portfolio.LastName = portfolioreader.GetString("lastname");
                    portfolio.Country = portfolioreader.GetString("country");
                    portfolio.City = portfolioreader.GetString("city");
                    portfolio.Street = portfolioreader.GetString("street");
                    portfolio.House = portfolioreader.GetString("house");
                    portfolio.ServiceLocation = portfolioreader.GetString("ServiceLocation");
                    portfolio.WorkExperience = portfolioreader.GetString("WorkExperience");
                }
                portfolioreader.Close();

                MySqlCommand selectPhoto = new MySqlCommand($"select url from image where userid={idmas}", con);
                MySqlDataAdapter da = new MySqlDataAdapter(selectPhoto);
                DataSet ds = new DataSet();
                da.Fill(ds);
                foreach (DataTable dt in ds.Tables)
                {
                    int c = 0;
                    portfolio.Images = new List<byte[]>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Byte[] byteBLOBData = new Byte[100000000];
                        byteBLOBData = ((Byte[])dt.Rows[c]["url"]);
                        portfolio.Image = byteBLOBData;
                        c++;
                        portfolio.Images.Add(byteBLOBData);
                    }
                }
                model.Add(portfolio);
                con.Close();
                return View(model);
            }
        }
    }
}