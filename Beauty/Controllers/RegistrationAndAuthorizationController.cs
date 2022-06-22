using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Beauty.Controllers
{
    public class RegistrationAndAuthorizationController : Controller
    {
        public string connectionString = "server=localhost;user=root;port=3306;password=1111;database=beauty";

        public ActionResult Registration()
        {
            //return View();
            return RedirectToRoute(new { controller = "RegistrationAndAuthorization", action = "NewRegistration" });
        }

        public ActionResult NewRegistration()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Registration(string email, string password, string firstname, string lastname, string phonenumber)
        { 
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int id = 1;
                MySqlCommand selectid = new MySqlCommand("select * from User order by id desc limit 1", con);
                MySqlDataReader reader = selectid.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32("id") + 1;
                }
                reader.Close();

                MySqlCommand insert = new MySqlCommand($"insert into User values({id},'{firstname}','{lastname}','{email}','{phonenumber}','{password}', 0, 0) ", con); 
                insert.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToRoute(new { controller = "RegistrationAndAuthorization", action = "NewAuthorization" });
        }

        public ActionResult Authorization()
        {
            return View();
        }

        public ActionResult NewAuthorization()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorization (string email, string password)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand selectUser = new MySqlCommand($"select * from User where email='{email}' and userpassword ='{password}' and usersrole='0'", con);
                MySqlDataReader userReader = selectUser.ExecuteReader();
                if (userReader.Read())
                {
                    userReader.Close();
                    MySqlCommand updateIsOnline = new MySqlCommand($"update user set isonline='1' where email='{email}' and userpassword ='{password}' and usersrole='0'", con);
                    updateIsOnline.ExecuteNonQuery();
                    return Redirect("~/UserPages/UserHomePage");
                }
                userReader.Close();
                MySqlCommand selectMaster = new MySqlCommand($"select * from User where email='{email}' and userpassword ='{password}' and usersrole='1'", con);
                MySqlDataReader masterReader = selectMaster.ExecuteReader();
                if (masterReader.Read())
                {
                    masterReader.Close();
                    MySqlCommand updateIsOnline = new MySqlCommand($"update user set isonline='1' where email='{email}' and userpassword ='{password}' and usersrole='1'", con);
                    updateIsOnline.ExecuteNonQuery();
                    return Redirect("~/MasterPages/HomePage");
                }
                masterReader.Close();
                MySqlCommand selectAdmin = new MySqlCommand($"select * from User where email='{email}' and userpassword ='{password}' and usersrole='2'", con);
                MySqlDataReader adminReader = selectAdmin.ExecuteReader();
                if (adminReader.Read())
                {
                    adminReader.Close();
                    MySqlCommand updateIsOnline = new MySqlCommand($"update user set isonline='1' where email='{email}' and userpassword ='{password}' and usersrole='2'", con);
                    updateIsOnline.ExecuteNonQuery();
                    return Redirect("~/Admin/Categories");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        public ActionResult MasterRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MasterRegistration (string email, string password, string firstname, string lastname, string phonenumber, string workexperience, string servicelocation, string country, string city, string street, string house)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int id = 1;
                MySqlCommand selectid = new MySqlCommand("select * from User order by id desc limit 1", con);
                MySqlDataReader reader = selectid.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32("id") + 1;
                }
                reader.Close();


                MySqlCommand insert = new MySqlCommand($"insert into User values({id},'{firstname}','{lastname}','{email}','{phonenumber}','{password}', 1, 0) ", con);
                insert.ExecuteNonQuery();
                MySqlCommand insertInfo = new MySqlCommand($"insert into MasterInfo values ('{id}', '{workexperience}', '{servicelocation}', '{country}', '{city}', '{street}', '{house}')", con);
                insertInfo.ExecuteNonQuery();  
                con.Close();
            }

            //return Redirect("~/MasterPages/HomePage");
            return RedirectToRoute(new { controller = "RegistrationAndAuthorization", action = "NewAuthorization" });

        }



    }
}