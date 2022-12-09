using System;
using System.Data.Entity;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Project_eStudiez.Models;
using Project_eStudiez.Interface;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using Project_eStudiez.OtherMethods;

namespace Project_eStudiez.Controllers
{

    public class HomeController : Controller
    {
        ModeleStudiez db = new ModeleStudiez();

        private IRepository<tbl_user> I_user;
        private IRepository<tbl_standard> I_standard;
        private IRepository<tbl_role> I_role;
        private IRepository<tbl_resources> I_resources;
        private IRepository<tbl_pdf> I_pdf;
        private IRepository<tbl_mark> I_mark;
        private IRepository<tbl_link> I_link;
        private IRepository<tbl_feedback> I_feedback;
        private IRepository<tbl_extraclass> I_extraclass;
        public HomeController(RepositoryClass<tbl_user> I_user, RepositoryClass<tbl_standard> I_standard, RepositoryClass<tbl_role> I_role, RepositoryClass<tbl_resources> I_resources, RepositoryClass<tbl_pdf> I_pdf, RepositoryClass<tbl_mark> I_mark, RepositoryClass<tbl_link> I_link, RepositoryClass<tbl_feedback> I_feedback, RepositoryClass<tbl_extraclass> I_extraclass)
        {
            this.I_user = I_user;
            this.I_standard = I_standard;
            this.I_role = I_role;
            this.I_resources = I_resources;
            this.I_pdf = I_pdf;
            this.I_mark = I_mark;
            this.I_link = I_link;
            this.I_feedback = I_feedback;
            this.I_extraclass = I_extraclass;
        }
        // GET: Home
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            List<tbl_user> lst = (List<tbl_user>)await I_user.Read();
            List<tbl_role> lst2 = (List<tbl_role>)await I_role.Read();
            if (lst2.Count == 0)
            {
                List<tbl_role> list = new List<tbl_role>() { new tbl_role() { role_name = "Teacher" }, new tbl_role() { role_name = "Parent/Guardian" }, new tbl_role() { role_name = "Student" }, new tbl_role() { role_name = "Super Admin" } };
                foreach (tbl_role item in list)
                {
                    db.tbl_role.Add(item);
                    int a = db.SaveChanges();
                    //await I_role.Update(item);

                }
            }
            if (lst.Count == 0)
            {
                _ =await I_user.Create(new tbl_user() { user_approved = 1, fk_role_id = 4, user_email = "annasraza17@gmail.com", user_image = "~/image/img_msg.jpg", user_email_status = "Verified", user_password = "MTIz", user_name ="Annas Raza" });
                //int aa = I_user.Create(new tbl_user() { user_approved = 1, fk_role_id = 4, user_email = "annasraza17@gmail.com", user_image = "~/image/img_msg.jpg", user_email_status = "Verified", user_password = "MTIz", user_name ="Annas Raza" });
                //int b= aa;


            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(tbl_login u)
        {
            try
            {
                string usr_pwd;
                if (ModelState.IsValid)
                {
                    List<tbl_user> lst = db.tbl_user.Where(m => m.user_name == u.user_name).ToList();
                    if (lst != null)
                    {
                        foreach (tbl_user item in lst)
                        {
                            usr_pwd = u.user_password.Encrypt_password();
                            if (item.user_password == usr_pwd)
                            {
                                tbl_user row = db.tbl_user.Where(m => m.user_name == u.user_name && m.user_password == usr_pwd).FirstOrDefault();

                                if (row != null)
                                {
                                    Session["name"] = row.user_name.ToString();
                                    Session["email"] = row.user_email.ToString();
                                    Session["user_id"] = row.user_id.ToString();
                                    Session["user_img"] = row.user_image.ToString();
                                    Session["user_password"] = row.user_password.ToString();

                                    if (row.fk_role_id == 1)
                                    {
                                        Session["role"] = "Teacher";

                                    }
                                    else if (row.fk_role_id == 2)
                                    {
                                        Session["role"] = "Parent/Guardian";
                                    }
                                    else if (row.fk_role_id == 3)
                                    {
                                        Session["role"] = "Student";
                                        Session["enrollment_no"] = row.user_std_enrollment_no;
                                    }
                                    else if (row.fk_role_id == 4)
                                    {
                                        Session["role"] = "Super Admin";
                                    }

                                    if (row.user_email_status == "Not Verified" && row.user_approved == 0)
                                    {
                                        return RedirectToAction("Email_Verification", "Home");
                                    }
                                    if (row.user_email_status == "Verified" && row.user_approved == 0)
                                    {
                                        return RedirectToAction("Anonymous", "Home");
                                    }
                                    if (row.user_email_status == "Verified" && row.user_approved == 1)
                                    {
                                        FormsAuthentication.SetAuthCookie(u.user_name, false);
                                        return RedirectToAction("HomePage", "Home");
                                    }

                                }
                                else
                                {
                                    ViewBag.msg = "Username or Password is in correct";
                                    //return View();
                                }

                            }
                            else
                            {
                                ViewBag.msg = "Username or Password is in correct";
                                // return View();
                            }


                        }
                    }
                    else
                    {
                        ViewBag.msg = "No Record Found";
                        //return View();
                    }
                }
                else
                {
                    ViewBag.msg = "Please fill all fields";
                    // return View();
                }
                return View();

            }
            catch (Exception)
            {
                ViewBag.msg = "No Record Found";
                return RedirectToAction("Index","Home");

            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Email_Verification()
        {

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Email_Verification(string code)
        {
            tbl_user row = await I_user.Get_id(int.Parse(Session["user_id"].ToString()));
            if (row.user_verification_code == code.ToString())
            {
                row.user_email_status = "Verified";
                int a = await I_user.Update(row);
                if (a > 0)
                {
                    return RedirectToAction("Anonymous");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                TempData["er11"] = "Incorrect Code";
            }
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            
                List<tbl_role> lst = db.tbl_role.ToList();
                if (lst.Count == 0)
                {
                    List<tbl_role> list = new List<tbl_role>() { new tbl_role() { role_name = "Teacher" }, new tbl_role() { role_name = "Parent/Guardian" }, new tbl_role() { role_name = "Student" }, new tbl_role() { role_name = "Super Admin" } };
                    foreach (tbl_role item in list)
                    {
                    db.tbl_role.Add(item);
                    int a = db.SaveChanges();
                    //await I_role.Update(item);

                    }
                }
                Session["fk_role"] = new SelectList(db.tbl_role.Take(3).ToList(), "role_id", "role_name");
                return View();

        }








    [HttpPost]
    [AllowAnonymous]
        public async Task<ActionResult> Register(tbl_user u)
        {
           
            string code = DateTime.Now.ToString("fffffff");
            try
            {
                if (!string.IsNullOrEmpty(u.user_name) && !string.IsNullOrEmpty(u.user_email))
                {
                    if (string.IsNullOrEmpty(u.user_password) == false && string.IsNullOrEmpty(u.user_confirm_password) == false)
                    {

                        if (u.user_password.Trim() != "" && u.user_confirm_password.Trim() != "")
                        {


                            if (u.user_password == u.user_confirm_password)
                            {
                                u.user_address = null;
                                u.user_image = "~/image/img_msg.jpg";
                                u.fk_standard_id = null;
                                u.user_phone = null;
                                u.user_age = null;
                                u.user_email_status = "Not Verified";
                                u.user_verification_code = code;

                                if (u.fk_role_id == 3)
                                {
                                    u.user_std_enrollment_no = DateTime.Now.ToString("fffffff");
                                }
                                u.user_password = u.user_password.Encrypt_password();

                                //db.tbl_user.Add(u);
                                //int a = db.SaveChanges();
                                //========== Due to Generic Repository Pattern ==========
                                int a = await I_user.Create(u);


                                if (a > 0)
                                {
                                    u.user_email.Send_email(code);
                                    return RedirectToAction("Index", "Home");
                                }
                                else
                                {
                                    ViewBag.msg = "Failed to Register";
                                }
                            }
                            else
                            {
                                ViewBag.msg = "Password does not match";
                            }

                        }
                        else
                        {
                            ViewBag.msg = "Password can not be empty spaces";
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Password can not be null or empty";
                    }




                }
                else
                {
                    ViewBag.msg = "Please fill all fileds";
                }
                return View();
            }
            catch (Exception)
            {
                ViewBag.msg = "Email is duplicate please select other email";

                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            return View(await I_user.Read());
        }

        [HttpGet]
        public async Task<ActionResult> HomePage()
        {
            if (Session["role"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            if (Session["role"].Equals("Student"))
            {
                tbl_user row =await I_user.Get_id(int.Parse(Session["user_id"].ToString()));
                try
                {
                    Class3in1 TinOne = new Class3in1() { lst_tbl_pdf = db.tbl_pdf.Where(m=> m.pdf_class == row.tbl_standard.standard_name).OrderByDescending(m => m.pdf_datetime).ToList(), lst_tbl_link = db.tbl_link.Where(m => m.link_class == row.tbl_standard.standard_name).OrderByDescending(m => m.link_datetime).ToList(), lst_tbl_extraclass = db.tbl_extraclass.Where(m => m.extraclass_class == row.tbl_standard.standard_name).OrderByDescending(m => m.extraclass_datetime_created_at).ToList(), row_extraclass = db.tbl_extraclass.Where(m => m.extraclass_class == row.tbl_standard.standard_name).OrderByDescending(m => m.extraclass_datetime_created_at).First() };
                    return View(TinOne);
                }
                catch (Exception)
                {
                    try
                    {
                        Class3in1 TinOne = new Class3in1() { lst_tbl_pdf = db.tbl_pdf.Where(m => m.pdf_class == row.tbl_standard.standard_name).OrderByDescending(m => m.pdf_datetime).ToList(), lst_tbl_link = db.tbl_link.Where(m => m.link_class == row.tbl_standard.standard_name).OrderByDescending(m => m.link_datetime).ToList(), lst_tbl_extraclass = db.tbl_extraclass.Where(m => m.extraclass_class == row.tbl_standard.standard_name).OrderByDescending(m => m.extraclass_datetime_created_at).ToList(), row_extraclass = new tbl_extraclass() { extraclass_class = "No Updated Class", extraclass_subject = "", extraclass_dateTime = "" } };
                        return View(TinOne);
                    }
                    catch (Exception)
                    {
                        Class3in1 TinOne = new Class3in1() { lst_tbl_pdf = db.tbl_pdf.OrderByDescending(m => m.pdf_datetime).ToList(), lst_tbl_link = db.tbl_link.OrderByDescending(m => m.link_datetime).ToList(), lst_tbl_extraclass = db.tbl_extraclass.OrderByDescending(m => m.extraclass_datetime_created_at).ToList(), row_extraclass = new tbl_extraclass() { extraclass_class = "No Updated Class", extraclass_subject = "", extraclass_dateTime = "" } };
                        return View(TinOne);
                    }
                   
                }
            }
            try
            {
                Class3in1 TinOne = new Class3in1() { lst_tbl_pdf = db.tbl_pdf.OrderByDescending(m => m.pdf_datetime).ToList(), lst_tbl_link = db.tbl_link.OrderByDescending(m => m.link_datetime).ToList(), lst_tbl_extraclass = db.tbl_extraclass.OrderByDescending(m => m.extraclass_datetime_created_at).ToList(), row_extraclass = db.tbl_extraclass.OrderByDescending(m => m.extraclass_datetime_created_at).First() };
                return View(TinOne);
            }
            catch (Exception)
            {
                Class3in1 TinOne = new Class3in1() { lst_tbl_pdf = db.tbl_pdf.OrderByDescending(m => m.pdf_datetime).ToList(), lst_tbl_link = db.tbl_link.OrderByDescending(m => m.link_datetime).ToList(), lst_tbl_extraclass = db.tbl_extraclass.OrderByDescending(m => m.extraclass_datetime_created_at).ToList(), row_extraclass = new tbl_extraclass() { extraclass_class = "No Updated Class", extraclass_subject = "", extraclass_dateTime = "" } };
                return View(TinOne);
            }

        }
        [HttpGet]
        public ActionResult ProfilePage()
        {
            if (Session["role"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            Session["tbl_standard"] = new SelectList(db.tbl_standard.ToList(), "standard_id", "standard_name");
            tbl_user row = db.tbl_user.Find(int.Parse(Session["user_id"].ToString()));
            tbl_standard row1 = db.tbl_standard.Find(row.fk_standard_id);
            if (row1 != null)
            {
                row.tbl_standard = row1;
            }

            else if (row1 is null)
            {
                row.tbl_standard = new tbl_standard() { standard_name = "No Class Selected" };
                //row.tbl_standard?? new tbl_standard() { standard_name = "No Class Selected" };
            }



            return View(row);
        }

        [HttpPost]
        public async Task<ActionResult> ProfilePage(tbl_user u)
        {
            u.user_verification_code = null;
            if (Session["role"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }

            if (ModelState.IsValid)
            {

                if (u.Http_img == null && u.user_image != "~/image/img_msg.jpg")
                {
                    u.user_image = Session["user_img"].ToString();
                }
                else if (u.Http_img == null && u.user_image == "~/image/img_msg.jpg")
                {
                    u.user_image = "~/image/img_msg.jpg";
                }
                else
                {
                    string file_name = Path.GetFileNameWithoutExtension(u.Http_img.FileName);
                    string extension = Path.GetExtension(u.Http_img.FileName);
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                    {

                        if (Session["user_img"].ToString() != "~/image/img_msg.jpg")
                        {
                            if (System.IO.File.Exists(Request.MapPath(Session["user_img"].ToString())))
                            {

                                System.IO.File.Delete(Request.MapPath(Session["user_img"].ToString()));
                            }
                        }


                        file_name = file_name + DateTime.Now.ToString("fffffff") + extension;
                        u.user_image = "~/image/" + file_name;
                        Session["user_img"] = u.user_image;
                        file_name = Path.Combine(Server.MapPath("~/image/"), file_name);
                        u.Http_img.SaveAs(file_name);
                    }
                    else
                    {
                        TempData["err"] = "Only .jpg , .jpeg & .png are allowed";
                    }
                }
                //if (u.user_standard != null && string.IsNullOrWhiteSpace(u.user_standard) == false)
                //{
                //    u.user_standard = u.user_standard.ToLower();

                //}
                //db.Entry(u).State = EntityState.Modified;
                //int a = db.SaveChanges();

                //============= Due to Generic Repository Pattern =============
                int a = await I_user.Update(u);
                if (a > 0)
                {
                    TempData["msg"] = "Profile Updated";
                }
                else
                {
                    TempData["err"] = "Failed to Updated";
                }



            }
            else
            {
                ViewBag.err = "Please fill all fields";
            }

            //return View(db.tbl_user.Find(int.Parse(Session["user_id"].ToString())));
            return RedirectToAction("ProfilePage", "Home");

        }
        public ActionResult CheckPwd(tbl_login l)
        {
            if (Session["role"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            if (l.user_password.Encrypt_password() == Session["user_password"].ToString())
            {
                ModelState.Clear();
                return PartialView("_PwdPartialView");

            }
            else
            {
                return Json("<h4 class=\"text-danger\">Password is incorrect</h4>");
            }
        }

        public ActionResult ChangePwd(tbl_user u)
        {
            try
            {

                if (Session["role"] == null)
                {
                    return RedirectToAction("Logout", "Home");
                }
                if (string.IsNullOrEmpty(u.user_password) == false && string.IsNullOrEmpty(u.user_confirm_password) == false)
                {

                    if (u.user_password.Trim() != "" && u.user_confirm_password.Trim() != "")
                    {


                        if (u.user_password == u.user_confirm_password)
                        {
                            int a;
                            u.user_password = u.user_password.Encrypt_password();
                            string cs = ConfigurationManager.ConnectionStrings["db_eStudiez"].ConnectionString;
                            SqlConnection con;
                            using (con = new SqlConnection(cs))
                            {
                                con.Open();

                                SqlCommand cmd = new SqlCommand("UPDATE tbl_user SET user_password = '" + u.user_password + "'  WHERE user_id = '" + int.Parse(Session["user_id"].ToString()) + "'; ", con);
                                a = cmd.ExecuteNonQuery();
                                con.Close();
                            }

                            if (a > 0)
                            {
                                Session["user_password"] = u.user_password;
                                ModelState.Clear();
                                return Json("<h4 class=\"text-success\">Password Changed</h4>");
                            }
                            else
                            {
                                ModelState.Clear();
                                return Json("<h4 class=\"text-danger\">Failed to update Password</h4>");
                            }

                        }
                        else
                        {
                            ModelState.Clear();
                            return Json("<h4 class=\"text-danger\">Password doesnot match</h4>");
                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        return Json("<h4 class=\"text-danger\">Password cannot be empty</h4>");
                    }
                }
                else
                {
                    ModelState.Clear();
                    return Json("<h4 class=\"text-danger\">Password can not be null or empty</h4>");
                }
            }
            catch (Exception)
            {
                return Json("<h4 class=\"text-danger\">Password cannot be empty</h4>");
            }
        }




        [HttpGet]
        public ActionResult Approve()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian") || Session["role"].Equals("Teacher"))
            {
                return RedirectToAction("Logout", "Home");
            }
            return View(db.tbl_user.Where(m => m.user_approved == 0).ToList());
        }


        [HttpPost]
        public async Task<ActionResult> UserApprove(int id)
        {

            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian") || Session["role"].Equals("Teacher"))
            {
                return RedirectToAction("Logout", "Home");
            }
            int a;


            string cs = ConfigurationManager.ConnectionStrings["db_eStudiez"].ConnectionString;
            SqlConnection con;
            using (con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tbl_user SET user_approved = 1 WHERE user_id = '" + id + "'; ", con);
                a = cmd.ExecuteNonQuery();
                con.Close();
            }


            if (a > 0)
            {
                //tbl_user row = await I_user.Get_id(id);
                //row.user_email.Send_emailApproved();
                //<Summary>
                //Due to above three lines it takes time.
                //</Summary>

                TempData["msg1"] = "User Approved";
                return PartialView("_approveuser", db.tbl_user.Where(m => m.user_approved == 0).ToList());
            }
            else
            {
                TempData["err1"] = "Failed to approve user !!!";
            }
            return PartialView("_approveuser", db.tbl_user.Where(m => m.user_approved == 0).ToList());
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian") || Session["role"].Equals("Teacher"))
            {
                return RedirectToAction("Logout", "Home");
            }
            //tbl_user row = db.tbl_user.Find(id);
            //if (row != null)
            //{
            //    db.Entry(row).State = EntityState.Deleted;
            //    int a = db.SaveChanges();
            //}
            //========== Due to Generic Repository Pattern ==========

            await I_user.Delete(id);

            return PartialView("_approveuser", db.tbl_user.Where(m => m.user_approved == 0).ToList());
        }



        [HttpGet]
        public ActionResult AllTeachers()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian") || Session["role"].Equals("Teacher"))
            {
                return RedirectToAction("Logout", "Home");
            }
            return View(db.tbl_user.Where(m => m.user_approved == 1 && m.fk_role_id == 1).ToList());
        }
        [HttpPost]
        public async Task<ActionResult> DeleteTeachers(int id)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian") || Session["role"].Equals("Teacher"))
            {
                return RedirectToAction("Logout", "Home");
            }
            //tbl_user row = db.tbl_user.Find(id);
            //========== Due to Generic Repository Pattern ==========
            tbl_user row = await I_user.Get_id(id);
            if (row != null)
            {
                if (row.user_image != "~/image/img_msg.jpg")
                {
                    if (System.IO.File.Exists(Request.MapPath(row.user_image)))
                    {
                        System.IO.File.Delete(Request.MapPath(row.user_image));
                    }
                }
                //db.Entry(row).State = EntityState.Deleted;
                //int a = db.SaveChanges();

                //========== Due to Generic Repository Pattern ==========
                await I_user.Delete(id);

            }
            return PartialView("_AllTeacher", db.tbl_user.Where(m => m.user_approved == 1 && m.fk_role_id == 1).ToList());
        }


        [HttpGet]
        public ActionResult AllParent()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            return View(db.tbl_user.Where(m => m.user_approved == 1 && m.fk_role_id == 2).ToList());
        }
        [HttpPost]
        public async Task<ActionResult> DeleteParent(int id)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            //tbl_user row = db.tbl_user.Find(id);
            //========== Due to Generic Repository Pattern ==========
            tbl_user row = await I_user.Get_id(id);
            if (row != null)
            {
                if (row.user_image != "~/image/img_msg.jpg")
                {
                    if (System.IO.File.Exists(Request.MapPath(row.user_image)))
                    {
                        System.IO.File.Delete(Request.MapPath(row.user_image));
                    }
                }

                //db.Entry(row).State = EntityState.Deleted;
                //int a = db.SaveChanges();
                //========== Due to Generic Repository Pattern ==========
                int a = await I_user.DeleteByRow(row);

            }
            return PartialView("_AllParents", db.tbl_user.Where(m => m.user_approved == 1 && m.fk_role_id == 2).ToList());
        }

        [HttpGet]
        public ActionResult AllStudent()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            return View(db.tbl_user.Where(m => m.user_approved == 1 && m.fk_role_id == 3).ToList());
        }
        [HttpPost]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            string temp_img = null;
            //tbl_user row = db.tbl_user.Find(id);
            //========== Due to Generic Repository Pattern ==========
            tbl_user row = await I_user.Get_id(id);

            if (row != null)
            {
                try
                {
                    if (row.user_image != "~/image/img_msg.jpg")
                    {

                        temp_img = row.user_image;

                    }

                    //db.Entry(row).State = EntityState.Deleted;
                    //int a = db.SaveChanges();
                    //=========== Due to Generic Repository Pattern =========== 
                    int a =await I_user.DeleteByRow(row);
                    
                        if (temp_img != null)
                        {
                            if (System.IO.File.Exists(Request.MapPath(temp_img)))
                            {
                                System.IO.File.Delete(Request.MapPath(temp_img));
                            }
                        }
                 

                }
                catch (Exception)
                {
                    TempData["rerr"] = "Cant Delete Data";
                }
            }
            return PartialView("_AllStudent", db.tbl_user.Where(m => m.user_approved == 1 && m.fk_role_id == 3).ToList());
        }

        public async Task<ActionResult> Material()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            ViewData["table_standard"] = (List<tbl_standard>) await I_standard.Read();
            return View();
        }

        public ActionResult ViewMaterial()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            Class3in1 TinO = new Class3in1() { lst_tbl_pdf = db.tbl_pdf.OrderByDescending(m => m.pdf_datetime).ToList(), lst_tbl_link = db.tbl_link.OrderByDescending(m => m.link_datetime).ToList(), lst_tbl_extraclass = db.tbl_extraclass.OrderByDescending(m => m.extraclass_datetime_created_at).ToList() };

            return View(TinO);
        }

        public async Task<ActionResult> link(string link_class, string link_material, string link_title)
        {

            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            if (string.IsNullOrEmpty(link_class) == false && string.IsNullOrEmpty(link_material) == false && string.IsNullOrEmpty(link_title) == false && string.IsNullOrWhiteSpace(link_class) == false && string.IsNullOrWhiteSpace(link_material) == false && string.IsNullOrWhiteSpace(link_title) == false)
            {
                tbl_link obj = new tbl_link() { link_class = link_class, link_material = link_material, link_title = link_title, link_datetime = DateTime.Now };
                //db.tbl_link.Add(obj);
                //int a = db.SaveChanges();
                //========== Due to Generic Repository Pattern ==========
                int a = await I_link.Create(obj);
                if (a > 0)
                {
                    return Json("<h3 class=\"text-success\">Link Added</h3>");

                }
                else
                {
                    return Json("<h3 class=\"text-danger\">Failed to Add link</h3>");
                }
            }
            else
            {
                return Json("<h3 class=\"text-danger\">Please Fill all Fields </h3>");
            }

        }
        public async Task<ActionResult> extraclass(string extraclass_class, DateTime extraclass_dateTime, string extraclass_subject)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            if (string.IsNullOrEmpty(extraclass_class) == false && string.IsNullOrEmpty(extraclass_dateTime.ToString()) == false && string.IsNullOrEmpty(extraclass_subject) == false && string.IsNullOrWhiteSpace(extraclass_class) == false && string.IsNullOrWhiteSpace(extraclass_dateTime.ToString()) == false && string.IsNullOrWhiteSpace(extraclass_subject) == false)
            {
                tbl_extraclass obj = new tbl_extraclass() { extraclass_class = extraclass_class, extraclass_dateTime = extraclass_dateTime.ToString(), extraclass_subject = extraclass_subject, extraclass_datetime_created_at = DateTime.Now };
                //db.tbl_extraclass.Add(obj);
                //int a = db.SaveChanges();
                //========== Due to Generic Repository Pattern ==========
                int a = await I_extraclass.Create(obj);
                if (a > 0)
                {
                    return Json("<h3 class=\"text-success\">Extra class Added</h3>");
                }
                else
                {
                    return Json("<h3 class=\"text-danger\">Failed to Add Extra class</h3>");
                }
            }
            else
            {
                return Json("<h3 class=\"text-danger\">Please Fill all Fields</h3>");
            }

        }

        public async Task<ActionResult> Pdf(string pdf_class, HttpPostedFileBase pdf_material, string pdf_title)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            if (string.IsNullOrEmpty(pdf_class) == false && string.IsNullOrEmpty(pdf_material.ToString()) == false && string.IsNullOrEmpty(pdf_title) == false && string.IsNullOrWhiteSpace(pdf_class) == false && string.IsNullOrWhiteSpace(pdf_material.ToString()) == false && string.IsNullOrWhiteSpace(pdf_title) == false)
            {
                tbl_pdf obj = new tbl_pdf() { pdf_class = pdf_class, pdf_title = pdf_title, pdf_datetime = DateTime.Now };
                string file_name = Path.GetFileNameWithoutExtension(pdf_material.FileName);
                string extension = Path.GetExtension(pdf_material.FileName);
                if (extension.ToLower() == ".pdf")
                {
                    file_name = file_name + DateTime.Now.ToString("fffffff") + extension;
                    obj.pdf_material = "~/pdf/" + file_name;
                    file_name = Path.Combine(Server.MapPath("~/pdf/"), file_name);
                    pdf_material.SaveAs(file_name);


                    //db.tbl_pdf.Add(obj);
                    //int a = db.SaveChanges();
                    //========== Due to Generic Repository Pattern ==========
                    int a = await I_pdf.Create(obj);
                    if (a > 0)
                    {
                        return Json("<h3 class=\"text-success\">Pdf Added</h3>");
                    }
                    else
                    {
                        return Json("<h3 class=\"text-danger\">Failed to Add Pdf</h3>");
                    }
                }
                else
                {
                    return Json("<h3 class=\"text-danger\">Please upload only pdf</h3>");
                }
            }
            else
            {
                return Json("<h3 class=\"text-danger\">Please Fill all Fields</h3>");
            }

        }
        [HttpPost]
        public async Task<ActionResult> PdfDelete(int id)
        {
            //tbl_pdf row = db.tbl_pdf.Where(m => m.pdf_id == id).FirstOrDefault();
            //========== Due to Generic Repository Pattern ==========
            tbl_pdf row = await I_pdf.Get_id(id);
            if (row != null)
            {
                if (System.IO.File.Exists(Request.MapPath(row.pdf_material)))
                {
                    System.IO.File.Delete(Request.MapPath(row.pdf_material));
                }

                //db.Entry(row).State = EntityState.Deleted;
                //int a = db.SaveChanges();
                //========== Due to Generic Repository Pattern ==========
                _ = await I_pdf.DeleteByRow(row);
            }

            //return PartialView("_pdfDelete", new Class3in1() { lst_tbl_pdf = db.tbl_pdf.ToList() });
            return PartialView("_pdfDelete", new Class3in1() { lst_tbl_pdf = (List<tbl_pdf>)await I_pdf.Read()});
        }
        [HttpPost]
        public async Task<ActionResult> linkDelete(int id)
        {

            //tbl_link row = db.tbl_link.Find(id);
            //if (row != null)
            //{
            //    db.Entry(row).State = EntityState.Deleted;
            //    int a = db.SaveChanges();
            //}
            //========== Due to Generic Repository Pattern ==========
            _ = await I_link.Delete(id);
            //return PartialView("_linkDelete", new Class3in1() { lst_tbl_link = db.tbl_link.ToList() });
            return PartialView("_linkDelete", new Class3in1() { lst_tbl_link = db.tbl_link.OrderByDescending(m => m.link_datetime).ToList() });
        }
        [HttpPost]
        public async Task<ActionResult> extraclassDelete(int id)
        {
            //tbl_extraclass row = db.tbl_extraclass.Find(id);
            //if (row != null)
            //{
            //    db.Entry(row).State = EntityState.Deleted;
            //    int a = db.SaveChanges();

            //}
            //========== Due to Generic Repository Pattern ==========
            _ = await I_extraclass.Delete(id);
            return PartialView("_extraclassDelete", new Class3in1() { lst_tbl_extraclass = db.tbl_extraclass.ToList() });
        }

        [AllowAnonymous]
        public ActionResult Anonymous()
        {
            return View();

        }


        public ActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Feedback(string name, string email, string subject, string message)
        {

            tbl_feedback obj = new tbl_feedback() { feedback_name = Session["name"].ToString(), feedback_email = Session["email"].ToString(), feedback_subject = subject, feedback_message = message, feedback_datetime = DateTime.Now, feedback_role = Session["role"].ToString() };
            //db.tbl_feedback.Add(obj);
            //int a = db.SaveChanges();
            //========== Due to Generic Repository Pattern ==========
            int a = await I_feedback.Create(obj);
            if (a > 0)
            {
                return Json("<div class=\"error-message h4 text-success\">Your message has been sent. Thank you!</div>");

            }
            else
            {
                return Json("<div class=\"sent-message h4 text-danger\">Failed to sent Message</div>");
            }
        }

        [HttpGet]
        public ActionResult AddMarks()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            Session["standard1"] = new SelectList(db.tbl_standard.ToList(), "standard_id", "standard_name");
            return View();

        }
        [HttpPost]
        public async Task<ActionResult> AddMarks(tbl_mark m, int std_value)
        {
            if (ModelState.IsValid)
            {
                m.fk_user_id = std_value;
                m.mark_date_time = DateTime.Now;
                //tbl_user user = db.tbl_user.Find(m.fk_user_id);
                //========== Due to Generic Repository Pattern ==========
                tbl_user user = await I_user.Get_id(m.fk_user_id);

                m.mark_std_standard = user.tbl_standard.standard_name;
                db.tbl_mark.Add(m);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    ModelState.Clear();
                    return Json("<h4 class=\"text-success\">" + m.mark_obtainedmark + " Marks of " + user.user_name + " of " + m.mark_subject + " Subject are Added</h4>");
                }
                else
                {
                    return Json("<h4 class=\"text-danger\">Failed to add Marks</h4>");
                }
            }
            else
            {
                return Json("<h4 class=\"text-danger\">Please fill all fields</h4>");
            }

        }

        public ActionResult ViewYourMark()
        {

            int a = int.Parse(Session["user_id"].ToString());
            List<tbl_mark> lst_of_tblmark = db.tbl_mark.Where(m => m.fk_user_id == a).ToList();


            return View(lst_of_tblmark);
        }

        public ActionResult View_DeleteStdMarks()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            return View(db.tbl_mark.ToList());
        }

        public ActionResult AddClass()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            ViewData["tbl_std"] = db.tbl_standard.ToList();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddClass(tbl_standard s)
        {
            //db.tbl_standard.Add(s);
            //int a = db.SaveChanges();
            //========== Due to Generic Repository Pattern ==========
            _ = await I_standard.Create(s);

            return PartialView("_AddClass", db.tbl_standard.ToList());
        }
        public async Task<ActionResult> DeleteStandard(int id)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            //tbl_standard row = db.tbl_standard.Find(id);
            //========== Due to Generic Repository Pattern ==========
            tbl_standard row = await I_standard.Get_id(id);
            if (row != null)
            {
                try
                {
                    //db.Entry(row).State = EntityState.Deleted;
                    //int a = db.SaveChanges();
                    //========== Due to Generic Repository Pattern ==========
                    _ = await I_standard.DeleteByRow(row);
                }
                catch (Exception)
                {


                }

            }
            return PartialView("_AddClass",await I_standard.Read());
        }

        public ActionResult Viewthefeedback()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            var a = db.tbl_feedback.OrderByDescending(m => m.feedback_id).ToList();
            return View(a);
        }
        public ActionResult ViewFeedback()
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian"))
            {
                return RedirectToAction("Logout", "Home");
            }
            return View(db.tbl_feedback.OrderByDescending(m => m.feedback_id).ToList());
        }
        [HttpPost]
        public async Task<ActionResult> ViewFeedback(int id)
        {
            if (Session["role"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            //tbl_feedback row = db.tbl_feedback.Find(id);
            //if (row != null)
            //{
            //    db.Entry(row).State = EntityState.Deleted;
            //    int a = db.SaveChanges();
            //}
            //========== Due to Generic Repository Pattern ==========
            _ = await I_feedback.Delete(id);


            return PartialView("_ViewFeedback", db.tbl_feedback.OrderByDescending(m => m.feedback_id).ToList());
        }


        public async Task<ActionResult> DeleteStudentRecord(int id)
        {
            if (Session["role"] == null || Session["role"].Equals("Student") || Session["role"].Equals("Parent/Guardian") || Session["role"].Equals("Teacher"))
            {
                return RedirectToAction("Logout", "Home");
            }
            //tbl_mark row = db.tbl_mark.Find(id);
            //if (row != null)
            //{
            //    db.Entry(row).State = EntityState.Deleted;
            //    int a = db.SaveChanges();

            //}
            //========== Due to Generic Repository Pattern ==========
            _ = await I_mark.Delete(id);

            return PartialView("_ShowStdMarks",await I_mark.Read());
        }




        public ActionResult ViewStdMark()
        {
            if (Session["role"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ViewStdMark(string enrollment_no)
        {
            if (Session["role"] == null)
            {
                return RedirectToAction("Logout", "Home");
            }
            try
            {
                tbl_user row = db.tbl_user.Where(m => m.user_std_enrollment_no == enrollment_no).FirstOrDefault();
                List<tbl_mark> lst_marks = db.tbl_mark.Where(m => m.fk_user_id == row.user_id).ToList();
                return PartialView("_ViewStdMark", lst_marks);
            }
            catch (Exception)
            {

                return Json("<h1 class=\"text-danger text-center\">Record not found</h1>");
            }
        }

        public ActionResult std_drpdwn(int std_drpdwn)
        {
            if (string.IsNullOrEmpty(std_drpdwn.ToString()) == false)
            {
                //List<tbl_user> lst2 = db.tbl_user.Where(m => m.user_standard == std_drpdwn && m.fk_role_id == 3).ToList();
                List<tbl_user> lst2 = db.tbl_user.Where(m => m.fk_standard_id == std_drpdwn).ToList();
                return PartialView("_AddMarks", lst2);
            }
            else
            {
                return Json("No Class Selected");
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //Session.Abandon();
            Session["name"] = null;
            Session["role"] = null;
            Session["email"] = null;
            Session["user_id"] = null;
            Session["user_img"] = null;
            Session["user_password"] = null;
            Session["enrollment_no"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}