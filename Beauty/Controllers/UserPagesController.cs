using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Beauty.Models;
using Beauty.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Beauty.Controllers
{
    public class UserPagesController : Controller
    {
        public string connectionString = "server=localhost;user=root;port=3306;password=1111;database=beauty";
        public ActionResult UserHomePage()
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

        public ActionResult Signout()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updateIsOnline = new MySqlCommand($"update user set isonline='0' where isonline='1'", con);
                updateIsOnline.ExecuteNonQuery();
                con.Close();
            }
            //return Redirect("~/RegistrationAndAuthorization/Authorization");
            return RedirectToRoute(new { controller = "RegistrationAndAuthorization", action = "NewAuthorization" });
        }

        public JsonResult getTimes(int Id, string Date)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                List<string> times = new List<string>();

                MySqlCommand getTimes = new MySqlCommand($"select Time from sheduler where MasterInfoId = {Id} and Date = '{Date}' and Status='0'", con);
                MySqlDataReader reader = getTimes.ExecuteReader();
                while (reader.Read())
                {
                    times.Add(reader.GetString("Time"));
                }
                reader.Close();

                con.Close();

                return Json(times);
            }
        }



        public ActionResult selectSub(int id)
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

                foreach(MasInf info in model)
                {
                    info.Dates = new List<string>();
                    MySqlCommand selectalldates = new MySqlCommand($"select distinct Date from sheduler where masterinfoid='{info.Id}' and status='0'", con);
                    MySqlDataReader datereader = selectalldates.ExecuteReader();
                    while (datereader.Read())
                    {
                        string date = datereader.GetString("Date");
                        
                        info.Dates.Add(date);
                    }
                    datereader.Close();
                }


                con.Close();
                return View(model);

            }  
        }

       

        public ActionResult sortByPrice(int id )
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var model = new List<MasInf>();
                MySqlCommand selectsub2 = new MySqlCommand("select user.firstname, user.lastname, masterinfo.city, masterinfo.street, masterinfo.house, masterservice.price, user.id from user join masterinfo on masterinfo.id = user.id join masterservice on masterservice.masterinfoid = masterinfo.id " +
                    $"where masterservice.subcategoryid = '{id}' order by price ", con);
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
                    model.Add(info);
                }
                reader.Close();
                foreach (MasInf info in model)
                {
                    info.Dates = new List<string>();
                    MySqlCommand selectalldates = new MySqlCommand($"select * from sheduler where masterinfoid='{info.Id}'", con);
                    MySqlDataReader datereader = selectalldates.ExecuteReader();
                    while (datereader.Read())
                    {
                        string date = datereader.GetString("WorkDay");
                        info.Dates.Add(date);
                    }
                    datereader.Close();
                }
                con.Close();
                return View(model);
            }
        }

        public ActionResult selectUserInfo()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand selectInfo = new MySqlCommand("select firstname, lastname, email, phonenumber from user where isonline='1' and usersrole='0'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(selectInfo);
                DataSet ds = new DataSet();
                da.Fill(ds);
                var model = new List<User>();
                foreach(DataTable dt in ds.Tables)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        User info = new User();
                        var line = row.ItemArray;
                        info.FirstName = Convert.ToString(line[0]);
                        info.LastName = Convert.ToString(line[1]);
                        info.Email = Convert.ToString(line[2]);
                        info.PhoneNumber = Convert.ToString(line[3]);

                        model.Add(info);
                    }
                }
                con.Close();
                return View(model);
            }
        }

        

        [HttpPost]
        public void updateUserInfo(string email, string firstname, string lastname, string phonenumber)
        {

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand updateInfo = new MySqlCommand($"update user set firstname='{firstname}', lastname='{lastname}', email='{email}', phonenumber='{phonenumber}' where isonline='1' and usersrole='0' ", con);
                    updateInfo.ExecuteNonQuery();
                    con.Close();

                }
        }

        [HttpPost]
        public ActionResult updateUserPassword(string oldpassword, string newpassword)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updateInfo = new MySqlCommand($"update user set userpassword='{newpassword}' where userpassword='{oldpassword}' and isonline='1' and usersrole='0' ", con);
                updateInfo.ExecuteNonQuery();
                con.Close();

            }
            return Redirect("~/MasterPages/selectUserInfo");
            
        }


        [HttpPost]
        public void CheckService(int idmaster, string date, string time, int price, int subid )
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int id = 1;
                MySqlCommand selectd = new MySqlCommand("select * from ServiceHistory order by id desc limit 1", con);
                MySqlDataReader r = selectd.ExecuteReader();
                if (r.Read())
                {
                    id = r.GetInt32("id") + 1;
                }
                r.Close();
                int userid = 0;
                MySqlCommand selectId = new MySqlCommand("select id from user where isonline='1'", con);
                MySqlDataReader reader = selectId.ExecuteReader();
                if (reader.Read())
                {
                    userid = Convert.ToInt32(reader["id"]);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    Content("ошибка в поиске айди пользователя");
                }

                int masterServiceId = 0;
                MySqlCommand selectMasterServiceId = new MySqlCommand($"select id from masterservice where masterinfoid={idmaster} and subcategoryid={subid}", con);
                MySqlDataReader masterServiceIdReader = selectMasterServiceId.ExecuteReader();
                if (masterServiceIdReader.Read())
                {
                    masterServiceId = masterServiceIdReader.GetInt32("id");
                    masterServiceIdReader.Close();
                }

                MySqlCommand insertCheck = new MySqlCommand($"insert into ServiceHistory values('{id}', '{userid}', '{masterServiceId}', '{date}', '{time}', '{price}', 'Забронировано')", con);
                insertCheck.ExecuteNonQuery();

                MySqlCommand updateStatus = new MySqlCommand($"update sheduler set Status = 1 where MasterInfoId = {idmaster} and date = '{date}' and time = '{time}'", con);
                updateStatus.ExecuteNonQuery();

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

        public ActionResult selectHistory(int serviceid)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int userid = 0;
                MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='0'", con);
                MySqlDataReader reader = selectId.ExecuteReader();
                if (reader.Read())
                {
                    userid = Convert.ToInt32(reader["id"]);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    Content("ошибка в поиске айди пользователя");
                }
                MySqlCommand selectHistory = new MySqlCommand($"select user.firstname, user.lastname, user.phonenumber, masterinfo.city, masterinfo.street, masterinfo.house, subcategory.subcategoryname, servicehistory.date, masterservice.price, servicehistory.status, servicehistory.id, servicehistory.Time, masterservice.MasterinfoId from user join masterinfo on user.id = masterinfo.id join masterservice on user.id = masterservice.masterinfoid join servicehistory on servicehistory.masterserviceid = masterservice.id join subcategory on masterservice.subcategoryid = subcategory.id where clientinfoid = {userid}", con);
                MySqlDataReader historyreader = selectHistory.ExecuteReader();
                var historyes = new List<History>();
                while (historyreader.Read())
                {
                    History history = new History();
                    history.FirstName = historyreader.GetString("firstname");
                    history.LastName = historyreader.GetString("lastname");
                    history.PhoneNumber = historyreader.GetString("phonenumber");
                    history.SubcategoryName = historyreader.GetString("subcategoryname");
                    history.Date = historyreader.GetString("date");
                    history.Time = historyreader.GetString("time");
                    history.Price = historyreader.GetInt32("price");
                    history.City = historyreader.GetString("city");
                    history.Street = historyreader.GetString("street");
                    history.House = historyreader.GetString("house");
                    history.Status = historyreader.GetString("status");
                    history.ServId = historyreader.GetInt32("id");
                    history.MasterInfoId= historyreader.GetInt32("masterinfoid");

                    historyes.Add(history);
                }
                historyreader.Close();

                con.Close();
                return View(historyes);
            }

        }



        public void cancelEntry(int serviceid, int masid, string date, string time)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updStatus = new MySqlCommand($"update servicehistory set status ='Отменено' where id = '{serviceid}' ", con);
                updStatus.ExecuteNonQuery();
                con.Close();
            }
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updStatus = new MySqlCommand($"update sheduler set status ='0' where masterinfoid = '{masid}' and date='{date}' and time='{time}' ", con);
                updStatus.ExecuteNonQuery();
                con.Close();
            }
        }

        public ActionResult BackEntry(int serviceid)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updStatus = new MySqlCommand($"update servicehistory set status ='Забронировано' where id = '{serviceid}' ", con);
                updStatus.ExecuteNonQuery();
                con.Close();
            }

            return Redirect("~/UserPages/selectHistory");
        }
    }
}