using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Project_eStudiez.OtherMethods
{

    public static class method
    {
        //Extended methods of string
        public static string Decrypt_password(this string Encrypted_password)
        {
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(Encrypted_password);
            int charcount = decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charcount];
            decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string decrypt_password = new string(decoded_char);

            return decrypt_password;
        }

        public static string Encrypt_password(this string password)
        {
            byte[] b = new byte[password.ToString().Length];
            b = Encoding.UTF8.GetBytes(password);
            string encoded_password = Convert.ToBase64String(b);
            string encrypt_password = encoded_password;

            return encrypt_password;
        }

        public static void Send_email(this string email, string code)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("estudiez9@gmail.com");
                mail.Subject = "Welcome to eStudiez!!!";
                mail.Body = "Your Verification code is eStudiez-<b>" + code + "</b>";
                //mail.Body = "<h3>You have registered <span style=\"color:green;\">Successfully</span><br />Please wait for the admin to approve you </h3>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587; // 25 465
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("estudiez9@gmail.com", "easqvkdrzvdsnihd");
                smtp.Send(mail);
                smtp.Dispose();
            }
            catch (Exception) { }
        }

        public static void Send_emailApproved(this string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("estudiez9@gmail.com");
                mail.Subject = "Welcome to eStudiez!!!";
                mail.Body = "<h3><span style=\"color:green;\">Congratulations!!!</span><br />you have successfully approved by admin</h3>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587; // 25 465
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("estudiez9@gmail.com", "easqvkdrzvdsnihd");
                smtp.Send(mail);
                smtp.Dispose();
            }
            catch (Exception) { }
        }

    }
}