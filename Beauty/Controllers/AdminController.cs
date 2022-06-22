using Beauty.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Controllers
{
   
    public class AdminController : Controller
    {
        public string connectionString = "server=localhost;user=root;port=3306;password=1111;database=beauty";
        public ActionResult Categories()
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

        public ActionResult Subcategory( int id)
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

        public ActionResult InsertCategory()
        {
            return View();
        }

        [HttpPost]
        public void InsertCategory(IFormFile photo, string categoryName)
        {

            long lenght = photo.Length;
            using var filestream = photo.OpenReadStream();
            byte[] bytes = new byte[lenght];
            filestream.Read(bytes, 0, (int)photo.Length);
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int id = 1;
                MySqlCommand selectd = new MySqlCommand("select * from Category order by id desc limit 1", con);
                MySqlDataReader r = selectd.ExecuteReader();
                if (r.Read())
                {
                    id = r.GetInt32("id") + 1;
                }
                r.Close();
                MySqlCommand insertphoto = new MySqlCommand($"insert into Category values('{id}','{categoryName}', @Photo)", con);
                insertphoto.Parameters.Add("@Photo", MySqlDbType.Blob).Value = bytes;
                insertphoto.ExecuteNonQuery();
            }
        }

        public ActionResult InsertSubcategory()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString)) 
            {
                con.Open();
                var categories = new List<Category>();
                MySqlCommand selectCat = new MySqlCommand("select * from Category", con);
                MySqlDataReader readerCat = selectCat.ExecuteReader();
                while (readerCat.Read())
                {
                    Category cat = new Category();
                    cat.Id = Convert.ToInt32(readerCat["id"]);
                    cat.CategoryName = readerCat.GetString("categoryname");
                    categories.Add(cat);
                }
                readerCat.Close();
                con.Close();
                return View(categories);
            }
        }

        [HttpPost]
        public void InsertSubcategory(int categId, string subcategoryName, IFormFile photo)
        {

            long lenght = photo.Length;
            using var filestream = photo.OpenReadStream();
            byte[] bytes = new byte[lenght];
            filestream.Read(bytes, 0, (int)photo.Length);
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                int id = 1;
                MySqlCommand selectd = new MySqlCommand("select * from Subcategory order by id desc limit 1", con);
                MySqlDataReader r = selectd.ExecuteReader();
                if (r.Read())
                {
                    id = r.GetInt32("id") + 1;
                }
                r.Close();
                MySqlCommand insertSub = new MySqlCommand($"insert into Subcategory values('{id}','{categId}', '{subcategoryName}', @Photo)", con);
                insertSub.Parameters.Add("@Photo", MySqlDbType.Blob).Value = bytes;
                insertSub.ExecuteNonQuery();
            }
        }

        public ActionResult UpdateCategory()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var categories = new List<Category>();
                MySqlCommand selectCat = new MySqlCommand("select * from Category", con);
                MySqlDataReader readerCat = selectCat.ExecuteReader();
                while (readerCat.Read())
                {
                    Category cat = new Category();
                    cat.Id = Convert.ToInt32(readerCat["id"]);
                    cat.CategoryName = readerCat.GetString("categoryname");
                    categories.Add(cat);
                }
                readerCat.Close();
                con.Close();
                return View(categories);
            }
        }


        [HttpPut]
        public void UpdateCategory(int categId, string newCateg, IFormFile photo)
        {
            long lenght = photo.Length;
            using var filestream = photo.OpenReadStream();
            byte[] bytes = new byte[lenght];
            filestream.Read(bytes, 0, (int)photo.Length);
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                
                con.Open();
                MySqlCommand insertphoto = new MySqlCommand($"update category set categoryname='{newCateg}',  image=@Photo where id={categId}", con);
                insertphoto.Parameters.Add("@Photo", MySqlDbType.Blob).Value = bytes;
                insertphoto.ExecuteNonQuery();
                con.Close();

            }          
        }


        public ActionResult UpdateSubcategory()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var categories = new List<Subcategory>();
                MySqlCommand selectCat = new MySqlCommand("select * from Subcategory", con);
                MySqlDataReader readerCat = selectCat.ExecuteReader();
                while (readerCat.Read())
                {
                    Subcategory cat = new Subcategory();
                    cat.Id = Convert.ToInt32(readerCat["id"]);
                    cat.Categoryid = readerCat.GetInt32("categoryid");
                    cat.Subcategoryname = readerCat.GetString("subcategoryname");
                    categories.Add(cat);
                }
                readerCat.Close();
                con.Close();
                return View(categories);
            }
        }

        [HttpPut]
        public void UpdateSubcategory(int categId, string newCateg, IFormFile photo)
        {
            long lenght = photo.Length;
            using var filestream = photo.OpenReadStream();
            byte[] bytes = new byte[lenght];
            filestream.Read(bytes, 0, (int)photo.Length);
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {

                con.Open();
                MySqlCommand insertphoto = new MySqlCommand($"update subcategory set subcategoryname='{newCateg}',  image=@Photo where id={categId}", con);
                insertphoto.Parameters.Add("@Photo", MySqlDbType.Blob).Value = bytes;
                insertphoto.ExecuteNonQuery();
                con.Close();

            }
        }


        public ActionResult DeleteCategory()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var categories = new List<Category>();
                MySqlCommand selectCat = new MySqlCommand("select * from Category", con);
                MySqlDataReader readerCat = selectCat.ExecuteReader();
                while (readerCat.Read())
                {
                    Category cat = new Category();
                    cat.Id = Convert.ToInt32(readerCat["id"]);
                    cat.CategoryName = readerCat.GetString("categoryname");
                    categories.Add(cat);
                }
                readerCat.Close();
                con.Close();
                return View(categories);
            }
        }


        [HttpDelete]
        public void DeleteCategory(int categId)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand delete = new MySqlCommand($"delete from category where id={categId}", con);
                delete.ExecuteNonQuery();
                con.Close();
            }
        }

        public ActionResult DeleteSubCategory()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                var categories = new List<Subcategory>();
                MySqlCommand selectCat = new MySqlCommand("select * from Subcategory", con);
                MySqlDataReader readerCat = selectCat.ExecuteReader();
                while (readerCat.Read())
                {
                    Subcategory cat = new Subcategory();
                    cat.Id = readerCat.GetInt32("id");
                    cat.Categoryid = readerCat.GetInt32("categoryid");
                    cat.Subcategoryname = readerCat.GetString("subcategoryname");
                    categories.Add(cat);
                }
                readerCat.Close();
                con.Close();
                return View(categories);
            }
        }

        [HttpDelete]
        public void DeleteSubCategory(int subId)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand delete = new MySqlCommand($"delete from subcategory where id={subId}", con);
                delete.ExecuteNonQuery();
                con.Close();
            }
        }


        public ActionResult SelectUsers()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand select = new MySqlCommand($"select * from User where UsersRole='1' or UsersRole='0' ", con);
                MySqlDataReader reader = select.ExecuteReader();
                var users = new List<User>();
                while (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32("id");
                    user.FirstName = reader.GetString("firstname");
                    user.LastName = reader.GetString("lastname");
                    user.Email = reader.GetString("email");
                    user.PhoneNumber = reader.GetString("phonenumber");
                    users.Add(user);
                }
                return View(users);
            }
        }

        public ActionResult SelectMasters()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand select = new MySqlCommand($"select * from User where UsersRole='1'", con);
                MySqlDataReader reader = select.ExecuteReader();
                var users = new List<User>();
                while (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32("id");
                    user.FirstName = reader.GetString("firstname");
                    user.LastName = reader.GetString("lastname");
                    user.Email = reader.GetString("email");
                    user.PhoneNumber = reader.GetString("phonenumber");
                    users.Add(user);
                }
                return View(users);
            }
        }
        public ActionResult SelectClients()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand select = new MySqlCommand($"select * from User where UsersRole='0' ", con);
                MySqlDataReader reader = select.ExecuteReader();
                var users = new List<User>();
                while (reader.Read())
                {
                    User user = new User();
                    user.Id = reader.GetInt32("id");
                    user.FirstName = reader.GetString("firstname");
                    user.LastName = reader.GetString("lastname");
                    user.Email = reader.GetString("email");
                    user.PhoneNumber = reader.GetString("phonenumber");
                    users.Add(user);
                }
                return View(users);
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

    }
}
