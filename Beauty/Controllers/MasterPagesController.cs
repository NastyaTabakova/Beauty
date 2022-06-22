using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Beauty.Models;
using Beauty.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Beauty.Controllers
{
    public class MasterPagesController : Controller
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


        public ActionResult Signout()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updateIsOnline = new MySqlCommand($"update User set isonline='0' where isonline='1'", con);
                updateIsOnline.ExecuteNonQuery();
                con.Close();
            }
            return Redirect("~/RegistrationAndAuthorization/NewAuthorization");
        }


        public ActionResult selectSub(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var model = new List<MasInf>();
                MySqlCommand selectsub2 = new MySqlCommand("select user.firstname, user.lastname, masterinfo.city, masterinfo.street, masterinfo.house, masterservice.price, user.id, masterservice.subcategoryid from user join masterinfo on masterinfo.id = user.id join masterservice on masterservice.masterinfoid = masterinfo.id " +
                        $"where masterservice.subcategoryid = '{id}' and masterservice.status=1", con);
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

 

        public ActionResult selectMasterInfo()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand selectInfo = new MySqlCommand("select firstname, lastname, email, phonenumber from user where isonline='1' and usersrole='1'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(selectInfo);
                DataSet ds = new DataSet();
                da.Fill(ds);
                var model = new List<User>();
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
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



        public ActionResult selectSubcategory() { 

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand selectsub = new MySqlCommand("select * from subcategory",con);
                MySqlDataAdapter da = new MySqlDataAdapter(selectsub);
                DataSet ds = new DataSet();
                da.Fill(ds);
                var model = new List<Subcategory>();
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Subcategory category = new Subcategory();
                        var line = row.ItemArray;
                        category.Id = Convert.ToInt32(line[0]);
                        category.Categoryid = Convert.ToInt32(line[1]);
                        category.Subcategoryname=Convert.ToString(line[2]);

                        model.Add(category);
                    }

                }
                con.Close();
                return View(model);

            }

        }

        [HttpPost]
        public void updateMasterInfo(string email, string firstname, string lastname, string phonenumber)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand updateInfo = new MySqlCommand($"update user set firstname='{firstname}', lastname='{lastname}', email='{email}', phonenumber='{phonenumber}' where isonline='1' and usersrole='1' ", con);
                    updateInfo.ExecuteNonQuery();
                    con.Close();

                }

            }
            catch
            {

            }
        }
        
        [HttpPost]
        public ActionResult updateMasterPassword(string oldpassword, string newpassword)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updateInfo = new MySqlCommand($"update user set userpassword='{newpassword}' where userpassword='{oldpassword}' and isonline='1' and usersrole='1' ", con);
                updateInfo.ExecuteNonQuery();
                con.Close();

            }
            return Redirect("~/MasterPages/selectMasterInfo");
            ////////////////////////////////////////////////
        }

        public ActionResult CreateNewService()
        {
            NewService serv = new NewService();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {

                con.Open();
                serv.categories = new List<Category>();
                MySqlCommand selectCat = new MySqlCommand("select * from Category", con);
                MySqlDataReader readerCat = selectCat.ExecuteReader();
                while (readerCat.Read())
                {
                    Category cat = new Category();
                    cat.Id = Convert.ToInt32(readerCat["id"]);
                    cat.CategoryName = readerCat.GetString("categoryname");
                    serv.categories.Add(cat);
                }
                readerCat.Close();
                con.Close();
            }
            return View(serv);
        }

        [HttpPost]
        public ActionResult CreateNewService(string subc, int price, string discription)
        {
            NewService serv = new NewService();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                
                con.Open();
                int id = 1;
                MySqlCommand selectd = new MySqlCommand("select * from MasterService order by id desc limit 1", con);
                MySqlDataReader r = selectd.ExecuteReader();
                if (r.Read())
                {
                    id = r.GetInt32("id") + 1;
                }
                r.Close();
                MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='1'", con);
                MySqlDataReader reader = selectId.ExecuteReader();
                if (reader.Read())
                {
                    serv.masterid = Convert.ToInt32(reader["id"]);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    Content("ошибка в поиске айди пользователя");
                }

                serv.categories = new List<Category>();
                MySqlCommand selectCat = new MySqlCommand("select * from Category", con);
                MySqlDataReader readerCat = selectCat.ExecuteReader();
                while (readerCat.Read())
                {
                    Category cat = new Category();
                    cat.Id = Convert.ToInt32(readerCat["id"]);
                    cat.CategoryName = readerCat.GetString("categoryname");
                    serv.categories.Add(cat);
                }
                readerCat.Close();

                int subid = 1;
                MySqlCommand selsubid = new MySqlCommand($"select id from subcategory where subcategoryname='{subc}'", con);
                MySqlDataReader subReader = selsubid.ExecuteReader();
                if (subReader.Read())
                {
                    subid = subReader.GetInt32("id");
                }
                subReader.Close();
                MySqlCommand insertService = new MySqlCommand($"insert into masterservice values('{id}', '{serv.masterid}', '{subid}', '{price}', '{discription}', 1)", con);
                insertService.ExecuteNonQuery();
                con.Close();
            }
                return View(serv);
        }


        public JsonResult getSubcategory(int catid)
        {
            List<string> subcategories = new List<string>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    MySqlCommand selectsub = new MySqlCommand($"select SubcategoryName from Subcategory where CategoryId={catid}", con);
                    MySqlDataReader readersub = selectsub.ExecuteReader();
                    while (readersub.Read())
                    {
                        subcategories.Add(readersub.GetString("SubcategoryName"));
                    }
                    readersub.Close();
                    con.Close();
                }
            }
            catch
            {

            }
            return Json(subcategories);
        }
        public ActionResult ServiceList()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int masterid = 0;
                MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='1'", con);
                MySqlDataReader reader = selectId.ExecuteReader();
                if (reader.Read())
                {
                    masterid = Convert.ToInt32(reader["id"]);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    Content("ошибка в поиске айди пользователя");
                }
                MySqlCommand outServiceInfo = new MySqlCommand($"select subcategory.subcategoryname, masterservice.price, masterservice.Discription, masterservice.Id from masterservice join subcategory on masterservice.subcategoryid=subcategory.id where masterinfoid='{masterid} and status=1'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(outServiceInfo);
                DataSet ds = new DataSet();
                da.Fill(ds);
                var model = new List<ServiceInfo>();
                foreach (DataTable dt in ds.Tables)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ServiceInfo info = new ServiceInfo();
                        var line = row.ItemArray;
                        info.ServiceName = Convert.ToString(line[0]);
                        info.Price = Convert.ToInt32(line[1]);
                        info.Discription= Convert.ToString(line[2]);
                        info.Id = Convert.ToInt32(line[3]);
                        model.Add(info);
                    }
                }
                con.Close();
                return View(model);

            }

        }


        [HttpPost]
        public void DelService(int id)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand updStatus = new MySqlCommand($"update masterservice set status = 0 where id = {id} ", con);
                updStatus.ExecuteReader();
                con.Close();

            }

        }
        public ActionResult addShedul()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int masterid = 0;
                MySqlCommand selectId = new MySqlCommand("select id from user where isonline='1' and usersrole='1'", con);
                MySqlDataReader reader = selectId.ExecuteReader();
                if (reader.Read())
                {
                    masterid = Convert.ToInt32(reader["id"]);
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    Content("ошибка в поиске айди пользователя");
                }
                var shed = new List<Sheduler>();
                MySqlCommand selectShed = new MySqlCommand($"select * from sheduler where masterinfoid = {masterid}", con);
                MySqlDataReader dataReader = selectShed.ExecuteReader();

                while (dataReader.Read())
                {
                    Sheduler sheduler = new Sheduler();
                    sheduler.Id = dataReader.GetInt32("id");
                    sheduler.Masterinfoid = dataReader.GetInt32("masterinfoid");
                    sheduler.Date = dataReader.GetString("date");
                    sheduler.Time = dataReader.GetString("time");
                    sheduler.Status = dataReader.GetString("status");
                    shed.Add(sheduler);
                }
                dataReader.Close();
                return View(shed);
            }
        }

       [HttpPost]
        public void addShedul(string date, string timestart)
        {
            
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    int id = 1;
                    MySqlCommand selectd = new MySqlCommand("select * from Sheduler order by id desc limit 1", con);
                    MySqlDataReader r = selectd.ExecuteReader();
                    if (r.Read())
                    {
                        id = r.GetInt32("id") + 1;
                    }
                    r.Close();
                    int masterid = 0;
                    MySqlCommand selectId = new MySqlCommand("select id from user where isonline='1' and usersrole='1'", con);
                    MySqlDataReader reader = selectId.ExecuteReader();
                    if (reader.Read())
                    {
                        masterid = Convert.ToInt32(reader["id"]);
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        Content("ошибка в поиске айди пользователя");
                    }
                    MySqlCommand insertShedul = new MySqlCommand($"insert into Sheduler values ('{id}', '{masterid}', '{date}',  '{timestart}', '0')", con);
                    insertShedul.ExecuteNonQuery();

                    var shed = new List<Sheduler>();

                    MySqlCommand selectShed = new MySqlCommand($"select * from sheduler where masterinfoid = {masterid}", con);
                    MySqlDataReader dataReader = selectShed.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Sheduler sheduler = new Sheduler();
                        sheduler.Id = dataReader.GetInt32("id");
                        sheduler.Masterinfoid = dataReader.GetInt32("masterinfoid");
                        sheduler.Date = dataReader.GetString("date");
                        sheduler.Time = dataReader.GetString("time");
                        sheduler.Status = dataReader.GetString("status");
                        shed.Add(sheduler);
                    }
                    dataReader.Close();
                }
            }
            catch
            {

            }
        }

        public ActionResult addPhoto()
        {
            return View();
        }

        [HttpPost]
        public void addPhoto(IFormFile photo, string descr )
        {
            try
            {
                long lenght = photo.Length;
                using var filestream = photo.OpenReadStream();
                byte[] bytes = new byte[lenght];
                filestream.Read(bytes, 0, (int)photo.Length);
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    int id = 1;
                    MySqlCommand selectd = new MySqlCommand("select * from Image order by id desc limit 1", con);
                    MySqlDataReader r = selectd.ExecuteReader();
                    if (r.Read())
                    {
                        id = r.GetInt32("id") + 1;
                    }
                    r.Close();
                    int masterid = 0;
                    MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='1'", con);
                    MySqlDataReader reader = selectId.ExecuteReader();
                    if (reader.Read())
                    {
                        masterid = Convert.ToInt32(reader["id"]);
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        Content("ошибка в поиске айди пользователя");
                    }
                    MySqlCommand insertphoto = new MySqlCommand($"insert into image values('{id}', '{masterid}', @Photo, '{descr}')", con);
                    insertphoto.Parameters.Add("@Photo", MySqlDbType.Blob).Value = bytes;
                    insertphoto.ExecuteNonQuery();
                }
            }
            catch
            {

            }
        }

        public ActionResult selectHistory()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int userid = 0;
                MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='1'", con);
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
                //MySqlCommand selectHistory = new MySqlCommand($"select user.firstname, user.lastname, user.phonenumber, subcategory.subcategoryname, servicehistory.date, servicehistory.time, masterservice.price, servicehistory.status, servicehistory.id from user join servicehistory on user.id = servicehistory.clientinfoid join masterservice on masterservice.masterinfoid = {userid} join subcategory on masterservice.subcategoryid = subcategory.id ", con);
                MySqlCommand selectHistory = new MySqlCommand($"select * from getHistoryAsc union select * from getHistoryDesc; ", con);
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
                    history.Price = historyreader.GetInt32("cost");
                    history.Status = historyreader.GetString("status");
                    history.ServId = historyreader.GetInt32("id");
                    history.MasterInfoId = userid;
                    historyes.Add(history);
                }
                historyreader.Close();

                con.Close();
                return View(historyes);
            }
        }

        public ActionResult Book()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int userid = 0;
                MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='1'", con);
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
                //MySqlCommand selectHistory = new MySqlCommand($"select distinct user.firstname, user.lastname, user.phonenumber, subcategory.subcategoryname, servicehistory.date, servicehistory.time, masterservice.price, servicehistory.status, servicehistory.id from user join servicehistory on user.id = servicehistory.clientinfoid join masterservice on masterservice.masterinfoid = {userid} join subcategory on masterservice.subcategoryid = subcategory.id where servicehistory.status='Забронировано'", con);
                MySqlCommand selectHistory = new MySqlCommand($"select * from getHistoryAsc where status = 'Забронировано' union select * from getHistoryDesc where status = 'Забронировано'; ", con);
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
                    history.Price = historyreader.GetInt32("cost");
                    history.Status = historyreader.GetString("status");
                    history.ServId = historyreader.GetInt32("id");
                    history.MasterInfoId = userid;
                    historyes.Add(history);
                }
                historyreader.Close();

                con.Close();
                return View(historyes);
            }
        }
        public ActionResult NoBook()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int userid = 0;
                MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='1'", con);
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
                //MySqlCommand selectHistory = new MySqlCommand($"select user.firstname, user.lastname, user.phonenumber, subcategory.subcategoryname, servicehistory.date, servicehistory.time, masterservice.price, servicehistory.status, servicehistory.id from user join servicehistory on user.id = servicehistory.clientinfoid join masterservice on masterservice.masterinfoid = {userid} join subcategory on masterservice.subcategoryid = subcategory.id where servicehistory.status='Отменено' or servicehistory.status='Отменено мастером'", con);
                MySqlCommand selectHistory = new MySqlCommand($"select * from getHistoryAsc where status = 'Отменено' union select * from getHistoryDesc where status = 'Отменено'; ", con);
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
                    history.Price = historyreader.GetInt32("cost");
                    history.Status = historyreader.GetString("status");
                    history.ServId = historyreader.GetInt32("id");
                    history.MasterInfoId = userid;
                    historyes.Add(history);
                }
                historyreader.Close();

                con.Close();
                return View(historyes);
            }
        }

        public ActionResult Complete()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int userid = 0;
                MySqlCommand selectId = new MySqlCommand("select * from user where isonline='1' and usersrole='1'", con);
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
                //MySqlCommand selectHistory = new MySqlCommand($"select user.firstname, user.lastname, user.phonenumber, subcategory.subcategoryname, servicehistory.date, servicehistory.time, masterservice.price, servicehistory.status, servicehistory.id from user join servicehistory on user.id = servicehistory.clientinfoid join masterservice on masterservice.masterinfoid = {userid} join subcategory on masterservice.subcategoryid = subcategory.id where servicehistory.status='Выполнено'", con);
                MySqlCommand selectHistory = new MySqlCommand($"select * from getHistoryAsc where status = 'Выполнено' union select * from getHistoryDesc where status = 'Выполнено'; ", con);
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
                    history.Price = historyreader.GetInt32("cost");
                    history.Status = historyreader.GetString("status");
                    history.ServId = historyreader.GetInt32("id");
                    history.MasterInfoId = userid;
                    historyes.Add(history);
                }
                historyreader.Close();

                con.Close();
                return View(historyes);
            }
        }

        [HttpPost]
        public void cancelEntry(int serviceid, int masid, string date, string time)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                MySqlCommand updStatus = new MySqlCommand($"update servicehistory set status ='Отменено мастером' where id = {serviceid} ", con);
                updStatus.ExecuteReader();
  

                MySqlCommand Status = new MySqlCommand($"update sheduler set status ='0' where masterinfoid = '{masid}' and date='{date}' and time='{time}' ", con);
                Status.ExecuteNonQuery();
                con.Close();
            }
        }

        [HttpPost]
        public void okEntry(int serviceid)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                MySqlCommand updStatus = new MySqlCommand($"update servicehistory set status ='Выполнено' where id = {serviceid} ", con);
                updStatus.ExecuteReader();
                con.Close();
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